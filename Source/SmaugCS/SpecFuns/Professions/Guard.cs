using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using System.Linq;

namespace SmaugCS.SpecFuns.Professions
{
    public static class Guard
    {
        public static bool Execute(MobileInstance ch, IManager dbManager)
        {
            if (!ch.IsAwake() || ch.CurrentFighting != null) return false;

            var maxEvil = 300;
            var crime = string.Empty;
            CharacterInstance victim = null;
            CharacterInstance ech = null;

            foreach (var vch in ch.CurrentRoom.Persons.Where(vch => vch != ch))
            {
                victim = vch;

                crime = GetCrime(victim);
                if (!string.IsNullOrEmpty(crime))
                    break;

                if (vch.CurrentFighting == null || vch.GetMyTarget() == ch || vch.CurrentAlignment >= maxEvil) continue;

                maxEvil = vch.CurrentAlignment;
                ech = victim;
            }

            if (victim != null && ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe))
            {
                Yell.do_yell(ch, $"{victim.Name} is a {crime}! As well as a COWARD!");
                return true;
            }

            if (victim != null)
            {
                Shout.do_shout(ch, $"{victim.Name} is a {crime}! PROTECT THE INNOCENT! BANZAI!!!");
                fight.multi_hit(ch, victim, Program.TYPE_UNDEFINED);
                return true;
            }

            if (ech == null) return false;

            comm.act(ATTypes.AT_YELL, "$n screams 'PROTECT THE INNOCENT!! BANZAI!!", ch, null, null, ToTypes.Room);
            fight.multi_hit(ch, ech, Program.TYPE_UNDEFINED);
            return true;
        }

        private static string GetCrime(CharacterInstance victim)
        {
            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Killer)) return "KILLER";
            if (!victim.IsNpc() && victim.Act.IsSet((int)PlayerFlags.Thief)) return "THIEF";
            return string.Empty;
        }
    }
}
