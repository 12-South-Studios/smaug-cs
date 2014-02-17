﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Commands.Social;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Database;
using SmaugCS.Exceptions;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.SpecFuns
{
    public static class Executioner
    {
        public static bool DoSpecExecutioner(CharacterInstance ch)
        {
            if (!ch.IsAwake() || ch.CurrentFighting != null)
                return false;

            string crime = string.Empty;
            CharacterInstance victim = null;

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
            }

            if (victim == null)
                return false;

            if (ch.CurrentRoom.Flags.IsSet((int) RoomFlags.Safe))
            {
                Yell.do_yell(ch, string.Format("{0} is a {1}! As well as a COWARD!", victim.Name, crime));
                return true;
            }

            Shout.do_shout(ch, string.Format("{0} is a {1}! PROTECT THE INNOCENT! MORE BLOOOOD!!!", victim.Name, crime));
            fight.multi_hit(ch, victim, Program.TYPE_UNDEFINED);

            if (ch.CharDied())
                return true;

            int vnum = Program.GetVnum("MobileCityGuard");
            MobTemplate cityguard = DatabaseManager.Instance.GetMobTemplate(vnum);

            CharacterInstance newGuard = DatabaseManager.Instance.CHARACTERS.Create(cityguard, null);
            ch.CurrentRoom.ToRoom(newGuard);

            newGuard = DatabaseManager.Instance.CHARACTERS.Create(cityguard, null);
            ch.CurrentRoom.ToRoom(newGuard);
            
            return true;
        }
    }
}