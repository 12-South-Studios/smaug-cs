﻿using SmaugCS.Commands;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Helpers;
using SmaugCS.Managers;

namespace SmaugCS.Spells
{
    public static class AstralWalk
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "vo"), 
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "level")]
        public static ReturnTypes spell_astral_walk(int sn, int level, CharacterInstance ch, object vo)
        {
            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>(sn);
            CharacterInstance victim = ch.GetCharacterInWorld(Cast.TargetName);

            if (CheckFunctions.CheckIfTrueCasting(victim == null
                                                  || !ch.CanAstral(victim)
                                                  || !victim.CurrentRoom.Area.InHardRange(ch), skill, ch,
                CastingFunctionType.Failed, victim)) return ReturnTypes.SpellFailed;
            

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
