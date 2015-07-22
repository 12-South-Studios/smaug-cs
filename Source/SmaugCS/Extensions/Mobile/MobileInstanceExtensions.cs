using System.Linq;
using SmaugCS.Commands.Movement;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;

namespace SmaugCS.Extensions.Mobile
{
    public static class MobileInstanceExtensions
    {
        public static void ProcessUpdate(this MobileInstance ch)
        {
            handler.set_cur_char(ch);

            if (ch.CurrentRoom == null || ch.IsAffected(AffectedByTypes.Charm) ||
                ch.IsAffected(AffectedByTypes.Paralysis))
                return;

            if (ch.MobIndex.ID == VnumConstants.MOB_VNUM_ANIMATED_CORPSE &&
                !ch.IsAffected(AffectedByTypes.Charm))
            {
                if (ch.CurrentRoom.Persons.Any())
                    comm.act(ATTypes.AT_MAGIC, "$n returns to the dust from whence $e came.", ch, null, null,
                        ToTypes.Room);

                if (ch.IsNpc())
                    ch.Extract(true);
                return;
            }

            if (!ch.Act.IsSet(ActFlags.Running) && !ch.Act.IsSet(ActFlags.Sentinel) && ch.CurrentFighting == null &&
                ch.CurrentHunting == null)
            {
                Macros.WAIT_STATE(ch, 2*GameConstants.GetSystemValue<int>("PulseViolence"));
                track.hunt_victim(ch);
                return;
            }

            if (!ch.Act.IsSet(ActFlags.Running) && ch.SpecialFunction != null)
            {
                if (ch.SpecialFunction.Value.Invoke(ch))
                    return;
                if (ch.CharDied())
                    return;
            }

            if (ch.MobIndex.HasProg(MudProgTypes.Script))
            {
                mud_prog.mprog_script_trigger(ch);
                return;
            }

            if (ch != handler.CurrentCharacter)
            {
                // TODO BUG: ch does not equal CurrentCharacter after spec_fun");
                return;
            }

            if (ch.CurrentPosition != PositionTypes.Standing)
                return;

            if (ch.Act.IsSet(ActFlags.Mounted))
            {
                if (ch.Act.IsSet(ActFlags.Aggressive) || ch.Act.IsSet(ActFlags.MetaAggr))
                    Emote.do_emote(ch, "snarls and growls.");
                return;
            }

            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe)
                && (ch.Act.IsSet(ActFlags.Aggressive) || ch.Act.IsSet(ActFlags.MetaAggr)))
                Emote.do_emote(ch, "glares around and snarls.");

            if (ch.CurrentRoom.Area.NumberOfPlayers > 0)
            {
                mud_prog.mprog_random_trigger(ch);
                if (ch.CharDied())
                    return;
                if ((int) ch.CurrentPosition < (int) PositionTypes.Standing)
                    return;
            }

            mud_prog.mprog_hour_trigger(ch);
            if (ch.CharDied())
                return;

            mud_prog.rprog_hour_trigger(ch);
            if (ch.CharDied())
                return;

            if ((int) ch.CurrentPosition < (int) PositionTypes.Standing)
                return;

            if (ch.Act.IsSet(ActFlags.Scavenger) && ch.CurrentRoom.Contents.Any() && SmaugRandom.Bits(2) == 0)
                Scavenge(ch);

            if (!ch.Act.IsSet(ActFlags.Running)
                && !ch.Act.IsSet(ActFlags.Sentinel)
                && !ch.Act.IsSet(ActFlags.Prototype)
                && !ch.Act.IsSet(ActFlags.StayArea))
            {
                var door = SmaugRandom.Bits(5);
                if (door > 9)
                    return;

                var exit = ch.CurrentRoom.GetExit(door);
                if (exit == null)
                    return;

                if (exit.Flags.IsSet(ExitFlags.Window) || exit.Flags.IsSet(ExitFlags.Closed))
                    return;

                var room = exit.GetDestination();
                if (room == null)
                    return;

                if (room.Flags.IsSet(RoomFlags.NoMob) || room.Flags.IsSet(RoomFlags.Death))
                    return;

                if (room.Area != ch.CurrentRoom.Area)
                    return;

                var retcode = Move.move_char(ch, exit, 0);
                if (ch.CharDied())
                    return;
                if (retcode != ReturnTypes.None || ch.Act.IsSet(ActFlags.Sentinel) ||
                    (int) ch.CurrentPosition < (int) PositionTypes.Standing)
                    return;
            }

            if (ch.CurrentHealth < ch.MaximumHealth/2)
            {
                var door = SmaugRandom.Bits(4);
                if (door > 9)
                    return;

                var exit = ch.CurrentRoom.GetExit(door);
                if (exit == null)
                    return;

                if (exit.Flags.IsSet(ExitFlags.Window) || exit.Flags.IsSet(ExitFlags.Closed))
                    return;

                var room = exit.GetDestination();
                if (room == null)
                    return;

                if (room.Flags.IsSet(RoomFlags.NoMob) || room.Flags.IsSet(RoomFlags.Death))
                    return;

                var found = false;
                foreach (var rch in ch.CurrentRoom.Persons)
                {
                    if (ch.IsFearing(rch))
                    {
                        var buf = string.Empty;
                        switch (SmaugRandom.Bits(2))
                        {
                            case 0:
                                buf = string.Format("Get away from me, {0}!", rch.Name);
                                break;
                            case 1:
                                buf = string.Format("Leave me be, {0}!", rch.Name);
                                break;
                            case 2:
                                buf = string.Format("{0} is trying to kill me!  Help!", rch.Name);
                                break;
                            case 3:
                                buf = string.Format("Someone save me from {0}!", rch.Name);
                                break;
                        }

                        Yell.do_yell(ch, buf);
                        found = true;
                        break;
                    }
                }

                if (found)
                    Move.move_char(ch, exit, 0);
            }
        }

        private static void Scavenge(CharacterInstance ch)
        {
            var max = 1;
            ObjectInstance best = null;

            foreach (var obj in ch.CurrentRoom.Contents)
            {
                if (obj.ExtraFlags.IsSet(ItemExtraFlags.Prototype) && !ch.Act.IsSet(ActFlags.Prototype))
                    continue;
                if (obj.WearFlags.IsSet(ItemWearFlags.Take) && obj.Cost > max &&
                    !obj.ExtraFlags.IsSet(ItemExtraFlags.Buried))
                {
                    best = obj;
                    max = obj.Cost;
                }
            }

            if (best != null)
            {
                best.InRoom.RemoveFrom(best);
                best.AddTo(ch);
                comm.act(ATTypes.AT_ACTION, "$n gets $p.", ch, best, null, ToTypes.Room);
            }
        }
    }
}
