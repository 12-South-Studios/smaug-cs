using SmaugCS.Commands;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Managers;
using SmaugCS.Extensions;

namespace SmaugCS.Spells
{
    public static class AstralWalk
    {
        public static ReturnTypes spell_astral_walk(int sn, int level, CharacterInstance ch, object vo)
        {
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);
            CharacterInstance victim = handler.get_char_world(ch, Cast.TargetName);

            if (victim == null
                || !ch.CanAstral(victim)
                || !victim.CurrentRoom.Area.InHardRange(ch))
            {
                magic.failed_casting(skill, ch, victim, null);
                return ReturnTypes.SpellFailed;
            }

            if (!string.IsNullOrEmpty(skill.HitCharacterMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitCharacterMessage, ch, null, victim, ToTypes.Character);
            if (!string.IsNullOrEmpty(skill.HitVictimMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitVictimMessage, ch, null, victim, ToTypes.Victim);

            if (!string.IsNullOrEmpty(skill.HitRoomMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitRoomMessage, ch, null, victim, ToTypes.NotVictim);
            else 
                comm.act(ATTypes.AT_MAGIC, "$n disappears in a flash of light!", ch, null, victim, ToTypes.Room);

            ch.CurrentRoom.FromRoom(ch);
            victim.CurrentRoom.ToRoom(ch);

            if (!string.IsNullOrEmpty(skill.HitDestinationMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitDestinationMessage, ch, null, victim, ToTypes.NotVictim);
            else 
                comm.act(ATTypes.AT_MAGIC, "$n appears in a flash of light!", ch, null, victim, ToTypes.Room);

            Look.do_look(ch, "auto");

            return ReturnTypes.None;
        }
    }
}
