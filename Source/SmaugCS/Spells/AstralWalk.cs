using System.Diagnostics.CodeAnalysis;
using SmaugCS.Commands;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Helpers;
using SmaugCS.Repository;

namespace SmaugCS.Spells
{
    public static class AstralWalk
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "vo"), 
        SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "level")]
        public static ReturnTypes spell_astral_walk(int sn, int level, CharacterInstance ch, object vo)
        {
            var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);
            var victim = ch.GetCharacterInWorld(Cast.TargetName);

            if (CheckFunctions.CheckIfTrueCasting(victim == null
                                                  || !ch.CanAstral(victim)
                                                  || !victim.CurrentRoom.Area.IsInHardRange(ch), skill, ch,
                CastingFunctionType.Failed, victim)) return ReturnTypes.SpellFailed;
            

            if (!string.IsNullOrEmpty(skill.HitCharacterMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitCharacterMessage, ch, null, victim, ToTypes.Character);
            if (!string.IsNullOrEmpty(skill.HitVictimMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitVictimMessage, ch, null, victim, ToTypes.Victim);

            if (!string.IsNullOrEmpty(skill.HitRoomMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitRoomMessage, ch, null, victim, ToTypes.NotVictim);
            else 
                comm.act(ATTypes.AT_MAGIC, "$n disappears in a flash of light!", ch, null, victim, ToTypes.Room);

            ch.CurrentRoom.RemoveFrom(ch);
            victim.CurrentRoom.AddTo(ch);

            if (!string.IsNullOrEmpty(skill.HitDestinationMessage))
                comm.act(ATTypes.AT_MAGIC, skill.HitDestinationMessage, ch, null, victim, ToTypes.NotVictim);
            else 
                comm.act(ATTypes.AT_MAGIC, "$n appears in a flash of light!", ch, null, victim, ToTypes.Room);

            Look.do_look(ch, "auto");

            return ReturnTypes.None;
        }
    }
}
