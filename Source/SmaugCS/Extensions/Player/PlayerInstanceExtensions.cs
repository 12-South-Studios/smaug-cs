using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions.Character;

namespace SmaugCS.Extensions.Player;

public static class PlayerInstanceExtensions
{
  public static void ProcessUpdate(this PlayerInstance ch)
  {
    handler.set_cur_char(ch);
    update.drunk_randoms(ch);
    update.hallucinations(ch);
  }

  public static void StopIdling(this PlayerInstance ch)
  {
    if (ch?.Descriptor is not { ConnectionStatus: ConnectionTypes.Playing })
      return;

    ch.Timer = 0;
    RoomTemplate wasInRoom = ch.PreviousRoom;
    ch.CurrentRoom.RemoveFrom(ch);
    wasInRoom.AddTo(ch);
    ch.PreviousRoom = ch.CurrentRoom;

    ch.PlayerData.Flags.RemoveBit((int)PCFlags.Idle);
    comm.act(ATTypes.AT_ACTION, "$n has returned from the world.", ch, null, null, ToTypes.Room);
  }

  public static long GetLearned(this PlayerInstance ch, long skillId)
  {
    return ch.PlayerData?.GetSkillMastery(skillId) ?? 0;
  }

  public static int CalculateAge(this PlayerInstance ch)
  {
    int numDays = (Program.GameManager.GameTime.Month + 1) * GameConstants.GetSystemValue<int>("DaysPerMonth") +
                  Program.GameManager.GameTime.Day;
    int chDays = (ch.PlayerData.Month + 1) *
      GameConstants.GetSystemValue<int>("DaysPerMonth") + ch.PlayerData.Day;
    int age = Program.GameManager.GameTime.Year - ch.PlayerData.Year;

    if (chDays - numDays > 0)
      age -= 1;
    return age;
  }

  private static readonly List<AffectedByTypes> AffectedByList = new()
  {
    AffectedByTypes.FireShield, AffectedByTypes.ShockShield, AffectedByTypes.AcidMist,
    AffectedByTypes.IceShield, AffectedByTypes.VenomShield, AffectedByTypes.Charm
  };

  [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
  public static void ShowVisibleAffectsOn(this PlayerInstance ch, CharacterInstance victim)
  {
    ATTypes atType = ATTypes.AT_WHITE;
    string description = string.Empty;
    VisibleAffectAttribute attrib = null;

    if (victim.IsAffected(AffectedByTypes.Sanctuary))
    {
      string name = (victim.IsNpc() ? victim.ShortDescription : victim.Name).CapitalizeFirst();

      atType = ATTypes.AT_WHITE;
      if (victim.IsGood())
        description = $"{name} glows with an aura of divine radiance.\r\n";
      else if (victim.IsEvil())
        description = $"{name} shimmers beneath an aura of dark energy.\r\n";
      else
        description = $"{name} is shrouded in flowing shadow and light.\r\n";
    }

    if (!victim.IsNpc() && victim.Switched.IsAffected(AffectedByTypes.Possess))
      attrib = AffectedByTypes.Possess.GetAttribute<VisibleAffectAttribute>();
    else
    {
      AffectedByTypes affectedBy = AffectedByList.FirstOrDefault(victim.IsAffected);
      if (affectedBy != AffectedByTypes.None)
        attrib = AffectedByTypes.Possess.GetAttribute<VisibleAffectAttribute>();
    }

    if (attrib != null)
    {
      atType = attrib.ATType;
      description = attrib.Description;
    }

    ch.SetColor(atType);
    ch.Printf(description);
  }
}