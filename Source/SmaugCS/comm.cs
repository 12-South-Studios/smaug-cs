using Autofac;
using Library.Common.Extensions;
using Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.MudProgs;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using SmaugCS.Extensions.Character;

namespace SmaugCS;

public static class comm
{
  public static bool chk_watch(short player_level, string player_name, string player_site)
  {
    if (db.WATCHES.Count == 0)
      return false;

    return db.WATCHES.Any(watch => (watch.TargetName.EqualsIgnoreCase(player_name)
                                    || watch.PlayerSite.EqualsIgnoreCase(player_site))
                                   && player_level < watch.ImmortalLevel);
  }

  /// <summary>
  /// PArse a name for acceptability
  /// </summary>
  /// <param name="name"></param>
  /// <param name="newchar"></param>
  /// <returns></returns>
  /// <remarks> Name checking should really only be done on new characters, otherwise 
  /// we could end up with people who can't access their characters.
  /// </remarks>
  public static bool check_parse_name(string name, bool newchar)
  {
    if (name.Length < GameConstants.GetConstant<int>("MinNameLength")
        || name.Length > GameConstants.GetConstant<int>("MaxNameLength"))
      return false;

    return name.IsAlphaNum();
  }

  /// <summary>
  /// Function to strip off the "a" or "an" or "the" or "some" from an object's
  /// short description for the purpose of using it in a sentence sent to
  /// the owner of the object.  (Ie: an object with the short description
  /// "a long dark blade" would return "long dark blade" for use in a sentence
  /// like "Your long dark blade".  The object name isn't always appropriate
  /// since it contains keywords that may not look proper.
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public static string myobj(ObjectInstance obj)
  {
    if (obj.ShortDescription.StartsWithIgnoreCase("a "))
      return obj.ShortDescription.Remove(0, 2);
    if (obj.ShortDescription.StartsWithIgnoreCase("an "))
      return obj.ShortDescription.Remove(0, 3);
    if (obj.ShortDescription.StartsWithIgnoreCase("the "))
      return obj.ShortDescription.Remove(0, 4);
    return obj.ShortDescription.StartsWithIgnoreCase("some ")
      ? obj.ShortDescription.Remove(0, 5)
      : obj.ShortDescription;
  }

  public static string obj_short(ObjectInstance obj)
  {
    return obj.Count > 1
      ? $"{obj.ShortDescription} ({obj.Count})"
      : obj.ShortDescription;
  }

  public static string MORPHNAME(CharacterInstance ch)
  {
    return !string.IsNullOrEmpty(ch.CurrentMorph?.Morph?.ShortDescription)
      ? ch.CurrentMorph.Morph.ShortDescription
      : Macros.NAME(ch);
  }

  public static string act_string(string format, CharacterInstance to, CharacterInstance ch, object arg1, object arg2,
    int flags)
  {
    bool dontUpper = !format.StartsWith("$");

    int varPosition;
    int startIndex = 0;
    StringBuilder sb = new(format);
    string buffer = string.Empty;

    do
    {
      const string chars = "$";
      char[] anyOf = chars.ToCharArray();

      varPosition = sb.IndexOfAny(anyOf, startIndex);
      if (varPosition == -1) continue;

      //// Is it a valid variable?
      string var = sb.Substring(varPosition, 2);
      if (!string.IsNullOrEmpty(var))
      {
        CharacterInstance vch;
        ObjectInstance obj;

        switch (var)
        {
          case "$t":
            buffer = arg1.ToString();
            break;
          case "$T":
            buffer = arg2.ToString();
            break;
          case "$n":
            if (ch.CurrentMorph == null)
              buffer = to != null ? Macros.PERS(ch, to) : Macros.NAME(ch);
            else if (!flags.IsSet(Program.STRING_IMM))
              buffer = to != null ? Macros.MORPHERS(ch, to) : MORPHNAME(ch);
            else
              buffer = $"(MORPH) {(to != null ? Macros.PERS(ch, to) : Macros.NAME(ch))}";
            break;
          case "$N":
            vch = arg2.CastAs<CharacterInstance>();
            if (vch.CurrentMorph == null)
              buffer = to != null ? Macros.PERS(vch, to) : Macros.NAME(vch);
            else if (!flags.IsSet(Program.STRING_IMM))
              buffer = to != null ? Macros.MORPHERS(vch, to) : MORPHNAME(vch);
            else
              buffer = $"(MORPH) {(to != null ? Macros.PERS(vch, to) : Macros.NAME(vch))}";
            break;
          case "$e":
            buffer = ch.Gender.SubjectPronoun();
            break;
          case "$E":
            vch = arg2.CastAs<CharacterInstance>();
            buffer = vch.Gender.SubjectPronoun();
            break;
          case "$m":
            buffer = ch.Gender.ObjectPronoun();
            break;
          case "$M":
            vch = arg2.CastAs<CharacterInstance>();
            buffer = vch.Gender.ObjectPronoun();
            break;
          case "$s":
            buffer = ch.Gender.PossessivePronoun();
            break;
          case "$S":
            vch = arg2.CastAs<CharacterInstance>();
            buffer = vch.Gender.PossessivePronoun();
            break;
          case "$q":
            buffer = to == ch ? "" : "s";
            break;
          case "$Q":
            buffer = to == ch ? "your" : ch.Gender.PossessivePronoun();
            break;
          case "$p":
            obj = arg1.CastAs<ObjectInstance>();
            buffer = to == null || to.CanSee(obj)
              ? obj_short(obj)
              : "something";
            break;
          case "$P":
            obj = arg2.CastAs<ObjectInstance>();
            buffer = to == null || to.CanSee(obj)
              ? obj_short(obj)
              : "something";
            break;
          case "$d":
            if (string.IsNullOrEmpty(arg2?.ToString()))
              buffer = "door";
            else
            {
              Tuple<string, string> tuple = arg2.ToString().FirstArgument();
              buffer = tuple.Item1;
            }

            break;
        }

        sb.ReplaceFirst(var, buffer);
      }

      startIndex = varPosition + 1;
    } while (varPosition != -1);

    if (!dontUpper)
      sb.CapitalizeFirst();
    return sb.ToString();
  }

  enum ActFFlags
  {
    None = 0,
    Text = 1 << 0,
    CH = 1 << 1,
    OBJ = 1 << 2
  }

  public static void act(ATTypes attype, string format, CharacterInstance ch, object arg1, object arg2,
    ToTypes type)
  {
    if (string.IsNullOrEmpty(format) || ch == null)
      return;

    int flags1 = (int)ActFFlags.None;
    int flags2 = (int)ActFFlags.None;
    ObjectInstance obj1 = arg1.CastAs<ObjectInstance>();
    ObjectInstance obj2 = arg2.CastAs<ObjectInstance>();
    CharacterInstance vch = arg2.CastAs<CharacterInstance>();
    CharacterInstance to;

    #region Nasty type checking

    // Do some proper type checking here..  Sort of.  We base it on the $* params.
    // This is kinda lame really, but I suppose in some weird sense it beats having
    // to pass like 8 different NULL parameters every time we need to call act()..
    if (format.Contains("$t"))
    {
      flags1 |= (int)ActFFlags.Text;
      obj1 = null;
    }

    if (format.Contains("$T") || format.Contains("$d"))
    {
      flags2 |= (int)ActFFlags.Text;
      vch = null;
      obj2 = null;
    }

    if (format.Contains("$N")
        || format.Contains("$E")
        || format.Contains("$M")
        || format.Contains("$S")
        || format.Contains("$Q"))
    {
      flags2 |= (int)ActFFlags.CH;
      obj2 = null;
    }

    if (format.Contains("$p"))
      flags1 |= (int)ActFFlags.OBJ;

    if (format.Contains("$P"))
    {
      flags2 |= (int)ActFFlags.OBJ;
      vch = null;
    }

    if (flags1 != (int)ActFFlags.None && flags1 != (int)ActFFlags.Text
                                      && flags1 != (int)ActFFlags.CH && flags1 != (int)ActFFlags.OBJ)
    {
      Program.LogManager.Bug("More than one type {0} defined. Setting all null.", flags1);
      obj1 = null;
    }

    if (flags2 != (int)ActFFlags.None && flags2 != (int)ActFFlags.Text
                                      && flags2 != (int)ActFFlags.CH && flags2 != (int)ActFFlags.OBJ)
    {
      Program.LogManager.Bug("More than one type {0} defined. Setting all null.", flags2);
      vch = null;
      obj2 = null;
    }

    if (ch.CurrentRoom == null)
      to = null;
    else if (type == ToTypes.Character)
      to = ch;
    else
      to = ch.CurrentRoom.Persons.First();

    #endregion

    if (ch.IsNpc() && ch.Act.IsSet((int)ActFlags.Secretive) && type != ToTypes.Character)
      return;

    if (type == ToTypes.Victim)
    {
      if (vch?.CurrentRoom == null)
        return;
      to = vch;
    }

    string txt = string.Empty;
    if (to != null && type != ToTypes.Character && type != ToTypes.Victim)
    {
      txt = act_string(format, null, ch, arg1, arg2, Program.STRING_IMM);
      if (to.CurrentRoom.HasProg(MudProgTypes.Act))
        MudProgHandler.ExecuteRoomProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Act, txt,
          to.CurrentRoom, ch, (ObjectInstance)arg1, arg2);

      foreach (ObjectInstance toObj in to.CurrentRoom.Contents
                 .Where(toObj => to.CurrentRoom.HasProg(MudProgTypes.Act)))
      {
        MudProgHandler.ExecuteObjectProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Act, txt, toObj, ch,
          (ObjectInstance)arg1, arg2);
      }
    }

    if (type == ToTypes.Character || type == ToTypes.Victim)
      return;

    foreach (CharacterInstance rch in ch.CurrentRoom.Persons)
    {
      PlayerInstance playerInstance = to as PlayerInstance;
      if (playerInstance != null && playerInstance.Descriptor == null)
        continue;

      MobileInstance instance = to as MobileInstance;
      if (instance != null && !instance.MobIndex.HasProg(MudProgTypes.Act))
        continue;

      if (!to.IsAwake())
        continue;

      switch (type)
      {
        case ToTypes.Character when to != ch:
        case ToTypes.Victim when to != vch || to == ch:
        case ToTypes.Room when to == ch:
        case ToTypes.NotVictim when to == ch || to == vch:
        case ToTypes.CanSee when
          to == ch ||
          (!to.IsImmortal() && !ch.IsNpc() && ch.Act.IsSet((int)PlayerFlags.WizardInvisibility) && to.Trust <
            (playerInstance != null
              ? playerInstance.PlayerData?.WizardInvisible ?? 0
              : 0)):
          continue;
      }

      txt = act_string(format, to, ch, arg1, arg2, to.IsImmortal() ? Program.STRING_IMM : Program.STRING_NONE);

      if (playerInstance != null)
      {
        to.SetColor(attype);
        to.SendTo(txt);
      }

      MudProgHandler.ExecuteMobileProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Act, txt, to, ch,
        arg1, arg2);
    }
  }

  public static string default_fprompt(CharacterInstance ch)
  {
    StringBuilder sb = new("&w<&Y%hhp ");
    sb.Append(ch.IsVampire() ? "&R%bbp" : "&C%mm");
    sb.Append("&G%vmv&w> ");

    if (ch.IsNpc() || ch.IsImmortal())
      sb.Append("%i%R");

    return sb.ToString();
  }

  /// <remarks>Not sure why this exists since its a perfect copy of default_fprompt</remarks>
  public static string default_prompt(CharacterInstance ch)
  {
    return default_fprompt(ch);
  }

  private const string Colors = "xrgObpcwzRGYBPCW";

  public static int getcolor(char clr)
  {
    for (int x = 0; x < 16; x++)
      if (clr == Colors[x])
        return x;
    return -1;
  }

  public static void display_prompt(DescriptorData d)
  {
    PlayerInstance ch = d.Character;
    PlayerInstance och = d.Original ?? d.Character;
    string buffer = string.Empty;

    bool ansi = !och.IsNpc() && och.Act.IsSet((int)PlayerFlags.Ansi);

    const string helpstart = "<Type HELP START>";

    if (ch == null)
    {
      Program.LogManager.Bug("Null ch");
      return;
    }

    string prompt;
    if (!ch.IsNpc() && !ch.PlayerData.Flags.IsSet(PCFlags.HelpStart))
      prompt = helpstart;
    else if (!ch.IsNpc() && ch.SubState != CharacterSubStates.None
                         && !ch.PlayerData.SubPrompt.IsNullOrEmpty())
      prompt = ch.PlayerData.SubPrompt;
    else if (ch.IsNpc() || (ch.CurrentFighting == null && ch.PlayerData.Prompt.IsNullOrEmpty()))
      prompt = default_prompt(ch);
    else if (ch.CurrentFighting != null)
    {
      prompt = ch.PlayerData.FPrompt.IsNullOrEmpty()
        ? default_fprompt(ch)
        : ch.PlayerData.FPrompt;
    }
    else
      prompt = ch.PlayerData.Prompt;

    if (ansi)
    {
      prompt = prompt.Insert(0, AnsiCodes.Reset.GetName());
      d.prevcolor = Convert.ToChar(0x08);
    }

    // Clear out old color stuff
    {
      // TODO
    }

    ch.SendTo(buffer);
  }

  public static void set_pager_input(DescriptorData d, string argument)
  {
    d.PageCommand = argument.Trim();
  }

  public static bool pager_output(DescriptorData d)
  {
    // TODO
    return false;
  }

  public static void bailout()
  {
    act_wiz.echo_to_all(ATTypes.AT_IMMORT, "MUD shutting down by system operator NOW!!", (int)EchoTypes.All);
    db.shutdown_mud("MUD shutdown by system operator");
    Program.LogManager.Info("MUD shutdown by system operator");

    Thread.Sleep(5000);

    // stop game loop
    // shutdown socket and save characters
  }
}