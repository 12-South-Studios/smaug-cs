using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.Spells
{
    public static class Earthquake
    {
        public static ReturnTypes spell_earthquake(int sn, int level, CharacterInstance ch, object vo)
        {
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);

            if (ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe))
            {
                ch.FailedCast(skill);
                return ReturnTypes.SpellFailed;
            }

            comm.act(ATTypes.AT_MAGIC, "The earth trembles beneath your feet!", ch, null, null, ToTypes.Character);
            comm.act(ATTypes.AT_MAGIC, "$n makes the earth tremble and shiver", ch, null, null, ToTypes.Room);

            bool charDied = false;

            // TODO Don't display to everyone in the world, thats dumb

            return charDied ? ReturnTypes.CharacterDied : ReturnTypes.None;
        }
    }
}
