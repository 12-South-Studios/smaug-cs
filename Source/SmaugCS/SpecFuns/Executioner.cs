using System.Linq;
using SmaugCS.Commands.Social;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Managers;

namespace SmaugCS.SpecFuns
{
    public static class Executioner
    {
        public static bool DoSpecExecutioner(MobileInstance ch)
        {
            if (!ch.IsAwake() || ch.CurrentFighting != null)
                return false;

            string crime = string.Empty;
            CharacterInstance victim = null;

            foreach (CharacterInstance vch in ch.CurrentRoom.Persons.Where(vch => vch != ch))
            {
                victim = vch;
                crime = GetCrime(victim);
                if (!string.IsNullOrEmpty(crime))
                    break;
            }

            if (victim == null)
                return false;

            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe))
            {
                Yell.do_yell(ch, string.Format("{0} is a {1}! As well as a COWARD!", victim.Name, crime));
                return true;
            }

            Shout.do_shout(ch, string.Format("{0} is a {1}! PROTECT THE INNOCENT! MORE BLOOOOD!!!", victim.Name, crime));
            fight.multi_hit(ch, victim, Program.TYPE_UNDEFINED);

            if (ch.CharDied())
                return true;

            int vnum = GameConstants.GetVnum("MobileCityGuard");
            MobTemplate cityguard = DatabaseManager.Instance.GetEntity<MobTemplate>(vnum);

            CharacterInstance newGuard = DatabaseManager.Instance.CHARACTERS.Create(cityguard, null);
            ch.CurrentRoom.AddTo(newGuard);

            newGuard = DatabaseManager.Instance.CHARACTERS.Create(cityguard, null);
            ch.CurrentRoom.AddTo(newGuard);
            
            return true;
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
