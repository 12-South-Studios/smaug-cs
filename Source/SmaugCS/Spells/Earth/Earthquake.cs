using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Helpers;
using SmaugCS.Repository;

namespace SmaugCS.Spells
{
    public static class Earthquake
    {
        public static ReturnTypes spell_earthquake(int sn, int level, CharacterInstance ch, object vo)
        {
            var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);

            if (CheckFunctions.CheckIfTrueCasting(ch.CurrentRoom.Flags.IsSet(RoomFlags.Safe), skill, ch))
                return ReturnTypes.SpellFailed;

            comm.act(ATTypes.AT_MAGIC, "The earth trembles beneath your feet!", ch, null, null, ToTypes.Character);
            comm.act(ATTypes.AT_MAGIC, "$n makes the earth tremble and shiver", ch, null, null, ToTypes.Room);

            var charDied = false;

            // TODO Don't display to everyone in the world, thats dumb

            return charDied ? ReturnTypes.CharacterDied : ReturnTypes.None;
        }
    }
}
