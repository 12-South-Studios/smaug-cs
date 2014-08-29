using System.Linq;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.SpecFuns
{
    public static class Guard
    {
        public static bool DoSpecGuard(CharacterInstance ch)
        {
            if (!ch.IsAwake() || ch.CurrentFighting != null)
                return false;

            int maxEvil = 300;
            string crime = string.Empty;
            CharacterInstance victim = null;
            CharacterInstance ech = null;

            foreach (CharacterInstance vch in ch.CurrentRoom.Persons.Where(vch => vch != ch))
            {
                victim = vch;

                crime = GetCrime(victim);
                if (!string.IsNullOrEmpty(crime))
                    break;

                if (vch.CurrentFighting != null 
                    && fight.GetMyTarget(vch) != ch
                    && vch.CurrentAlignment < maxEvil)
                {
                    maxEvil = vch.CurrentAlignment;
                    ech = victim;
                }
            }

            if (victim != null && ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe))
            {
                Yell.do_yell(ch, string.Format("{0} is a {1}! As well as a COWARD!", victim.Name, crime));
                return true;
            }

            if (victim != null)
            {
                Shout.do_shout(ch, string.Format("{0} is a {1}! PROTECT THE INNOCENT! BANZAI!!!", victim.Name, crime));
                fight.multi_hit(ch, victim, Program.TYPE_UNDEFINED);
                return true;
            }

            if (ech != null)
            {
                comm.act(ATTypes.AT_YELL, "$n screams 'PROTECT THE INNOCENT!! BANZAI!!", ch, null, null, ToTypes.Room);
                fight.multi_hit(ch, ech, Program.TYPE_UNDEFINED);
                return true;
            }

            return false;
        }

        private static string GetCrime(CharacterInstance victim)
        {
            if (!victim.IsNpc() && victim.Act.IsSet(PlayerFlags.Killer))
                return "KILLER";

            if (!victim.IsNpc() && victim.Act.IsSet(PlayerFlags.Thief))
                return "THIEF";

            return string.Empty;
        }
    }
}
