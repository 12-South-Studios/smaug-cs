using System.Linq;
using SmaugCS.Commands.Social;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

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

            foreach (CharacterInstance vch in ch.CurrentRoom.Persons.Where(vch => !vch.Equals(ch)))
            {
                victim = vch;

                if (!vch.IsNpc() && vch.Act.IsSet((int) PlayerFlags.Killer))
                {
                    crime = "KILLER";
                    break;
                }

                if (!vch.IsNpc() && vch.Act.IsSet((int) PlayerFlags.Thief))
                {
                    crime = "THIEF";
                    break;
                }

                if (vch.CurrentFighting != null 
                    && !fight.who_fighting(vch).Equals(ch) 
                    && vch.CurrentAlignment < maxEvil)
                {
                    maxEvil = vch.CurrentAlignment;
                    ech = victim;
                }
            }

            if (victim != null && ch.CurrentRoom.Flags.IsSet((int) RoomFlags.Safe))
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
    }
}
