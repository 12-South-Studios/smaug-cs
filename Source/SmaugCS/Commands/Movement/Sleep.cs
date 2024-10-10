using Autofac;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.MudProgs;

namespace SmaugCS.Commands.Movement;

public static class Sleep
{
  public static void do_sleep(CharacterInstance ch, string argument)
  {
    switch (ch.CurrentPosition)
    {
      case PositionTypes.Sleeping:
        ch.SendTo("You are already sleeping.");
        break;
      case PositionTypes.Resting:
        FromResting(ch);
        break;
      case PositionTypes.Sitting:
        FromSitting(ch);
        break;
      case PositionTypes.Standing:
        FromStanding(ch);
        break;
      default:
        if (ch.IsInCombatPosition())
          ch.SendTo("You are busy fighting!");
        else if (ch.CurrentPosition == PositionTypes.Mounted)
          ch.SendTo("You really should dismount first.");
        break;
    }

    MudProgHandler.ExecuteRoomProg(Program.Container.Resolve<IMudProgHandler>(), MudProgTypes.Sleep, ch);
  }

  private static bool CantSleepDueToMentalState(CharacterInstance ch, int modifier)
  {
    if (ch.MentalState <= 30 || SmaugRandom.D100() + modifier >= ch.MentalState) return false;
    ch.SendTo("You just can't seem to calm yourself down enough to sleep.");
    comm.act(ATTypes.AT_PLAIN, "$n closes $s eyes for a few moments, but just can't seem to go to sleep.",
      ch, null, null, ToTypes.Room);
    return true;
  }

  private static void FromStanding(CharacterInstance ch)
  {
    if (CantSleepDueToMentalState(ch, 0)) return;

    ch.SendTo("You close your eyes and drift into slumber.");
    comm.act(ATTypes.AT_ACTION, "$n closes $s eyes and drifts into a deep slumber.", ch, null, null, ToTypes.Room);
    ch.CurrentPosition = PositionTypes.Sleeping;
  }

  private static void FromSitting(CharacterInstance ch)
  {
    if (CantSleepDueToMentalState(ch, 5)) return;

    ch.SendTo("You slump over and fall dead asleep.");
    comm.act(ATTypes.AT_ACTION, "$n nods off and slowly slumps over, dead asleep.", ch, null, null, ToTypes.Room);
    ch.CurrentPosition = PositionTypes.Sleeping;
  }

  private static void FromResting(CharacterInstance ch)
  {
    if (CantSleepDueToMentalState(ch, 10)) return;

    ch.SendTo("You collapse into a deep sleep.");
    comm.act(ATTypes.AT_ACTION, "$n collapses into a deep sleep.", ch, null, null, ToTypes.Room);
    ch.CurrentPosition = PositionTypes.Sleeping;
  }
}