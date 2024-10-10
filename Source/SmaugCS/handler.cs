using Library.Common;
using Library.Common.Extensions;
using SmaugCS.Commands;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Repository;
using SmaugCS.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Commands.Admin;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Helpers;
using EnumerationExtensions = Library.Common.Extensions.EnumerationExtensions;

namespace SmaugCS;

public static class handler
{
  public static ReturnTypes GlobalReturnCode { get; set; }
  public static ReturnTypes GlobalObjectCode { get; set; }

  public static CharacterInstance SavingCharacter { get; set; }
  public static CharacterInstance LoadingCharacter { get; set; }

  public static CharacterInstance CurrentCharacter { get; set; }
  public static CharacterInstance CurrentDeadCharacter { get; set; }
  public static bool CurrentCharacterDied { get; set; }
  public static RoomTemplate CurrentRoom { get; set; }
  public static ObjectInstance CurrentObject { get; set; }
  public static bool CurrentObjectExtracted { get; set; }

  public static Queue<ObjectInstance> ExtractedObjectQueue { get; set; }
  public static Queue<ExtracedCharacterData> ExtractedCharacterQueue { get; set; }

  public static int falling = 0;

  private static readonly Dictionary<int, string> ObjectMessageLargeMap = new()
  {
    { 1, "As you reach for it, you forget what it was...\r\n" },
    { 2, "As you reach for it, something inside stops you...\r\n" }
  };

  private static readonly Dictionary<int, string> ObjectMessageSmallMap = new()
  {
    { 1, "In just a second...\r\n" },
    { 2, "You can't find that...\r\n" },
    { 3, "It's just beyond your grasp...\r\n" },
    { 4, "... but it's under a pile of other stuff...\r\n" },
    { 5, "You go to reach for it, but pick your nose instead.\r\n" },
    { 6, "Which one?!?  I see two... no three...\r\n" }
  };

  /// <summary>
  /// Mental state can affect finding an object, used by get/drop/put/quaff/etc.
  /// Gets increasingly "freaky" based on mental state and drunkenness
  /// </summary>
  /// <param name="ch"></param>
  /// <returns></returns>
  public static bool FindObject_CheckMentalState(CharacterInstance ch)
  {
    int ms = ch.MentalState;

    // We're going to be nice and let nothing weird happen unless you're a tad messed up
    int drunk = 1.GetHighestOfTwoNumbers(ch.IsNpc()
      ? 0
      : ((PlayerInstance)ch).PlayerData.ConditionTable[ConditionTypes.Drunk]);
    if (Math.Abs(ms) + drunk / 3 < 30)
      return false;
    if (SmaugRandom.D100() + (ms < 0 ? 15 : 5) > Math.Abs(ms) / 2 + drunk / 4)
      return false;

    string output = ms > 15
      ? ObjectMessageLargeMap[SmaugRandom.Between(1.GetHighestOfTwoNumbers(ms / 5 - 15), (ms + 4) / 5)]
      : ObjectMessageSmallMap[SmaugRandom.Between(1, (Math.Abs(ms) / 2 + drunk).GetNumberThatIsBetween(1, 60) / 10)];

    ch.SendTo(output);
    return true;
  }

  public static ObjectInstance FindObject(CharacterInstance ch, string argument, bool carryonly)
  {
    Tuple<string, string> tuple = argument.FirstArgument();
    string arg1 = tuple.Item1;
    tuple = tuple.Item2.FirstArgument();
    string arg2 = tuple.Item1;
    string remainder = tuple.Item2;

    if (arg2.EqualsIgnoreCase("from") && !string.IsNullOrEmpty(remainder))
    {
      tuple = remainder.FirstArgument();
      arg2 = tuple.Item1;
      remainder = tuple.Item2;
    }

    ObjectInstance obj;
    if (string.IsNullOrEmpty(arg2))
    {
      obj = carryonly ? ch.GetCarriedObject(arg1) : ch.GetObjectOnMeOrInRoom(arg1);
      if (CheckFunctions.CheckIfTrue(ch, obj == null && carryonly, "You do not have that item.")) return null;

      if (obj == null)
      {
        comm.act(ATTypes.AT_PLAIN, "I see no $T here.", ch, null, arg1, ToTypes.Character);
        return null;
      }

      return obj;
    }

    ObjectInstance container = null;
    if (CheckFunctions.CheckIfTrue(ch,
          carryonly && (container = ch.GetCarriedObject(arg2)) == null &&
          (container = ch.GetWornObject(arg2)) == null, "You do not have that item.")) return null;

    if (!carryonly && (container = ch.GetObjectOnMeOrInRoom(arg2)) == null)
    {
      comm.act(ATTypes.AT_PLAIN, "I see no $T here.", ch, null, arg2, ToTypes.Character);
      return null;
    }

    if (!container.ExtraFlags.IsSet((int)ItemExtraFlags.Covering) &&
        container.Value.ToList()[1].IsSet(ContainerFlags.Closed))
    {
      comm.act(ATTypes.AT_PLAIN, "The $d is closed.", ch, null, container.Name, ToTypes.Character);
      return null;
    }

    obj = ch.GetObjectInList(container.Contents, arg1);
    if (obj == null)
      comm.act(ATTypes.AT_PLAIN, container.ExtraFlags.IsSet((int)ItemExtraFlags.Covering)
        ? "I see nothing like that beneath $p."
        : "I see nothing like that in $p.", ch, container, null, ToTypes.Character);

    return obj;
  }

  public static string affect_loc_name(int location)
  {
    ApplyTypes type = EnumerationExtensions.GetEnum<ApplyTypes>(location);
    return type.GetName();
  }

  public static string affect_bit_name(ExtendedBitvector vector)
  {
    StringBuilder sb = new();
    foreach (AffectedByTypes type in Enum.GetValues(typeof(AffectedByTypes))
               .Cast<AffectedByTypes>()
               .Where(type => vector.IsSet((int)type)))
    {
      sb.Append($" {type.GetName()}");
    }

    return sb.ToString();
  }

  public static string extra_bit_name(ExtendedBitvector extra_flags)
  {
    StringBuilder sb = new();
    foreach (ItemExtraFlags type in Enum.GetValues(typeof(ItemExtraFlags))
               .Cast<ItemExtraFlags>()
               .Where(type => extra_flags.IsSet((int)type)))
    {
      sb.Append($" {type.GetName()}");
    }

    return sb.ToString();
  }

  public static string magic_bit_name(int magic_flags)
  {
    return (magic_flags & (int)ItemMagicFlags.Returning) > 0 ? " returning" : "none";
  }

  public static string pull_type_name(int pulltype)
  {
    foreach (DirectionPullTypes type in Enum.GetValues(typeof(DirectionPullTypes)))
    {
      if ((int)type == pulltype || pulltype == type.GetValue())
        return type.GetName();
    }

    return "ERROR";
  }

  public static ObjectInstance get_trap(ObjectInstance obj)
  {
    return !obj.IsTrapped() ? null : obj.Contents.FirstOrDefault(check => check.ItemType == ItemTypes.Trap);
  }

  public static void name_stamp_stats(CharacterInstance ch)
  {
    ch.PermanentStrength = 18.GetLowestOfTwoNumbers(ch.PermanentStrength);
    ch.PermanentWisdom = 18.GetLowestOfTwoNumbers(ch.PermanentWisdom);
    ch.PermanentDexterity = 18.GetLowestOfTwoNumbers(ch.PermanentDexterity);
    ch.PermanentIntelligence = 18.GetLowestOfTwoNumbers(ch.PermanentIntelligence);
    ch.PermanentConstitution = 18.GetLowestOfTwoNumbers(ch.PermanentConstitution);
    ch.PermanentCharisma = 18.GetLowestOfTwoNumbers(ch.PermanentCharisma);
    ch.PermanentLuck = 18.GetLowestOfTwoNumbers(ch.PermanentLuck);
    ch.PermanentStrength = 9.GetHighestOfTwoNumbers(ch.PermanentStrength);
    ch.PermanentWisdom = 9.GetHighestOfTwoNumbers(ch.PermanentWisdom);
    ch.PermanentDexterity = 9.GetHighestOfTwoNumbers(ch.PermanentDexterity);
    ch.PermanentIntelligence = 9.GetHighestOfTwoNumbers(ch.PermanentIntelligence);
    ch.PermanentConstitution = 9.GetHighestOfTwoNumbers(ch.PermanentConstitution);
    ch.PermanentCharisma = 9.GetHighestOfTwoNumbers(ch.PermanentCharisma);
    ch.PermanentLuck = 9.GetHighestOfTwoNumbers(ch.PermanentLuck);

    foreach (char c in ch.Name)
    {
      int b = c % 14;
      int a = c % 1 + 1;

      switch (b)
      {
        case 0:
          ch.PermanentStrength = 18.GetLowestOfTwoNumbers(ch.PermanentStrength + a);
          break;
        case 1:
          ch.PermanentDexterity = 18.GetLowestOfTwoNumbers(ch.PermanentDexterity + a);
          break;
        case 2:
          ch.PermanentWisdom = 18.GetLowestOfTwoNumbers(ch.PermanentWisdom + a);
          break;
        case 3:
          ch.PermanentIntelligence = 18.GetLowestOfTwoNumbers(ch.PermanentIntelligence + a);
          break;
        case 4:
          ch.PermanentConstitution = 18.GetLowestOfTwoNumbers(ch.PermanentConstitution + a);
          break;
        case 5:
          ch.PermanentCharisma = 18.GetLowestOfTwoNumbers(ch.PermanentCharisma + a);
          break;
        case 6:
          ch.PermanentLuck = 9.GetLowestOfTwoNumbers(ch.PermanentLuck + a);
          break;
        case 7:
          ch.PermanentStrength = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
          break;
        case 8:
          ch.PermanentDexterity = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
          break;
        case 9:
          ch.PermanentWisdom = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
          break;
        case 10:
          ch.PermanentIntelligence = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
          break;
        case 11:
          ch.PermanentConstitution = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
          break;
        case 12:
          ch.PermanentCharisma = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
          break;
        case 13:
          ch.PermanentLuck = 9.GetHighestOfTwoNumbers(ch.PermanentStrength - a);
          break;
      }
    }
  }

  public static void fix_char(CharacterInstance ch)
  {
    save.de_equip_char(ch);

    foreach (AffectData af in ch.Affects)
      ch.ModifyAffect(af, false);

    if (ch.CurrentRoom != null)
    {
      foreach (AffectData af in ch.CurrentRoom.Affects)
      {
        if (af.Location != ApplyTypes.WearSpell
            && af.Location != ApplyTypes.RemoveSpell
            && af.Location != ApplyTypes.StripSN)
          ch.ModifyAffect(af, false);
      }

      foreach (AffectData af in ch.CurrentRoom.PermanentAffects)
      {
        if (af.Location != ApplyTypes.WearSpell
            && af.Location != ApplyTypes.RemoveSpell
            && af.Location != ApplyTypes.StripSN)
          ch.ModifyAffect(af, false);
      }
    }

    ch.AffectedBy = new ExtendedBitvector();

    RaceData race = Program.RepositoryManager.GetRace(ch.CurrentRace);
    ch.AffectedBy.SetBits(race.AffectedBy);
    ch.MentalState = -10;
    ch.CurrentHealth = 1.GetHighestOfTwoNumbers(ch.CurrentHealth);
    ch.CurrentMana = 1.GetHighestOfTwoNumbers(ch.CurrentMana);
    ch.CurrentMovement = 1.GetHighestOfTwoNumbers(ch.CurrentMovement);
    ch.ArmorClass = 100;
    ch.ModStrength =
      ch.ModDexterity =
        ch.ModWisdom = ch.ModIntelligence = ch.ModConstitution = ch.ModCharisma = ch.ModLuck = 0;
    ch.DamageRoll = new DiceData();
    ch.HitRoll = new DiceData();
    ch.CurrentAlignment = -1000.GetNumberThatIsBetween(ch.CurrentAlignment, 1000);
    ch.SavingThrows = new SavingThrowData();

    foreach (AffectData af in ch.Affects)
      ch.ModifyAffect(af, true);

    if (ch.CurrentRoom != null)
    {
      foreach (AffectData af in ch.CurrentRoom.Affects)
      {
        if (af.Location != ApplyTypes.WearSpell
            && af.Location != ApplyTypes.RemoveSpell
            && af.Location != ApplyTypes.StripSN)
          ch.ModifyAffect(af, true);
      }

      foreach (AffectData af in ch.CurrentRoom.PermanentAffects)
      {
        if (af.Location != ApplyTypes.WearSpell
            && af.Location != ApplyTypes.RemoveSpell
            && af.Location != ApplyTypes.StripSN)
          ch.ModifyAffect(af, true);
      }
    }

    ch.CarryWeight = ch.CarryNumber = 0;

    foreach (ObjectInstance obj in ch.Carrying)
    {
      if (obj.WearLocation == WearLocations.None)
        ch.CarryNumber += obj.ObjectNumber;
      if (!obj.ExtraFlags.IsSet((int)ItemExtraFlags.Magical))
        ch.CarryWeight += obj.GetWeight();
    }

    save.re_equip_char(ch);
  }

  public static void showaffect(CharacterInstance ch, AffectData paf)
  {
    Validation.IsNotNull(paf, "paf");

    string buf = string.Empty;

    if (paf.Location == ApplyTypes.None || paf.Modifier == 0)
      return;

    switch (paf.Location)
    {
      default:
        buf = $"Affects {affect_loc_name((int)paf.Location)} by {paf.Modifier}.";
        break;
      case ApplyTypes.Affect:
        buf = $"Affects {affect_loc_name((int)paf.Location)} by {paf.Modifier}.";

        for (int i = 0; i < 32; i++)
        {
          if (paf.Modifier.IsSet(1 << i))
            buf += " " + BuilderConstants.a_flags[i];
        }

        break;
      case ApplyTypes.WeaponSpell:
      case ApplyTypes.WearSpell:
      case ApplyTypes.RemoveSpell:
        string name = "unknown";
        if (Macros.IS_VALID_SN(paf.Modifier))
        {
          SkillData skill = Program.RepositoryManager.SKILLS.Get(paf.Modifier);
          name = skill.Name;
        }

        buf = $"Casts spell '{name}'";
        break;
      case ApplyTypes.Resistance:
      case ApplyTypes.Immunity:
      case ApplyTypes.Susceptibility:
        buf = $"Affects {affect_loc_name((int)paf.Location)} by {paf.Modifier}";

        for (int i = 0; i < 32; i++)
        {
          if (paf.Modifier.IsSet(1 << i))
            buf += " " + BuilderConstants.ris_flags[i];
        }

        break;
    }

    ch.SendTo(buf);
  }

  public static void set_cur_obj(ObjectInstance obj)
  {
    CurrentObject = obj;
    CurrentObjectExtracted = false;
    GlobalObjectCode = ReturnTypes.None;
  }

  public static bool obj_extracted(ObjectInstance obj)
  {
    if (obj == CurrentObject && CurrentObjectExtracted)
      return true;

    return ExtractedObjectQueue.Any(extractObj => obj == extractObj);
  }

  public static void queue_extracted_obj(ObjectInstance obj)
  {
    ExtractedObjectQueue.Enqueue(obj);
  }

  public static void clean_obj_queue()
  {
    ExtractedObjectQueue.Clear();
  }

  public static void set_cur_char(CharacterInstance ch)
  {
    CurrentCharacter = ch;
    CurrentCharacterDied = false;
    CurrentRoom = ch.CurrentRoom;
    GlobalReturnCode = ReturnTypes.None;
  }

  public static void queue_extracted_char(CharacterInstance ch, bool extract)
  {
    if (ch == null)
      throw new ArgumentNullException(nameof(ch));

    ExtracedCharacterData ecd = new()
    {
      Character = ch,
      Room = ch.CurrentRoom,
      Extract = extract,
      ReturnCode = ch == CurrentCharacter ? GlobalReturnCode : ReturnTypes.CharacterDied
    };

    ExtractedCharacterQueue.Enqueue(ecd);
  }

  public static void clean_char_queue()
  {
    ExtractedCharacterQueue.Clear();
  }

  public static bool chance_attrib(CharacterInstance ch, short percent, short attrib)
  {
    return SmaugRandom.D100() - ch.GetCurrentLuck() + 13 - attrib + 13 +
      (ch.IsDevoted() ? ((PlayerInstance)ch).PlayerData.Favor / -500 : 0) <= percent;
  }

  public static void economize_mobgold(CharacterInstance mob)
  {
    mob.CurrentCoin = mob.CurrentCoin.GetLowestOfTwoNumbers(mob.Level * mob.Level * 400);
    if (mob.CurrentRoom == null)
      return;

    int gold = (mob.CurrentRoom.Area.HighEconomy > 0 ? 1 : 0) * 1000000000 * mob.CurrentRoom.Area.LowEconomy;
    mob.CurrentCoin = 0.GetNumberThatIsBetween(mob.CurrentCoin, gold / 10);
    if (mob.CurrentCoin > 0)
      mob.CurrentRoom.Area.LowerEconomy(mob.CurrentCoin);
  }

  public static void check_switches(bool possess)
  {
    foreach (CharacterInstance ch in Program.RepositoryManager.CHARACTERS.Values)
      check_switch(ch, possess);
  }

  public static void check_switch(CharacterInstance ch, bool possess)
  {
    if (ch.Switched == null) return;
    if (!possess)
    {
      foreach (AffectData af in ch.Switched.Affects.Where(x => x.Duration != -1))
      {
        SkillData skill = Program.RepositoryManager.SKILLS.Get((int)af.Type);
        if (af.Type != AffectedByTypes.None && skill != null &&
            skill.SpellFunction.Value == Possess.spell_possess)
          return;
      }
    }

    foreach (CommandData cmd in Program.RepositoryManager.COMMANDS.Values
               .Where(x => x.DoFunction.Value == Switch.do_switch)
               .Where(x => x.Level > ch.Trust))
    {
      if (!ch.IsNpc() && ((PlayerInstance)ch).PlayerData.Bestowments.Any(x => cmd.Name.EqualsIgnoreCase(x))
                      && cmd.Level <= ch.Trust + Program.GameManager.SystemData.BestowDifference)
        return;
    }

    if (!possess)
    {
      ch.Switched.SetColor(ATTypes.AT_BLUE);
      ch.Switched.SendTo("You suddenly forfeit the power to switch!");
    }

    Return.do_return(ch.Switched, string.Empty);
  }

  public static void separate_obj(ObjectInstance obj)
  {
    obj.Split(1);
  }

  private static int GetMaxKillTrack()
  {
    return GameConstants.GetConstant<int>("MaxKillTrack");
  }

  private static int GetLevelAvatar()
  {
    return GameConstants.GetConstant<int>("level_avatar");
  }

  /// <summary>
  /// How much experience is required for CH to get to a certain level
  /// </summary>
  /// <param name="ch"></param>
  /// <param name="level"></param>
  /// <returns></returns>
  public static int exp_level(CharacterInstance ch, int level)
  {
    int lvl = Macros.UMAX(0, level - 1);
    return lvl * lvl * lvl * ch.GetExperienceBase();
  }

  public static int times_killed(CharacterInstance ch, CharacterInstance mob)
  {
    if (ch.IsNpc()) return 0;
    if (!mob.IsNpc()) return 0;

    long templateId = (mob.Parent as MobileTemplate).Id;
    int track = Macros.URANGE(2, (ch.Level + 3) * GetMaxKillTrack() / GetLevelAvatar(), GetMaxKillTrack());

    PlayerInstance pc = (PlayerInstance)ch;
    KilledData killedData = pc.PlayerData.Killed.FirstOrDefault(y => y.ID == templateId);
    return killedData?.Count ?? 0;
  }
}