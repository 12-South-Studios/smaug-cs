﻿using System.Linq;
using SmaugCS.Common;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.SpecFuns
{
    public static class Dragon
    {
        public static bool DoSpecDragon(CharacterInstance ch, string spellName)
        {
            if (!ch.IsInCombatPosition())
                return false;

            CharacterInstance victim =
                ch.CurrentRoom.Persons.Where(v => !v.Equals(ch))
                  .FirstOrDefault(vch => SmaugRandom.Bits(2) == 0 && fight.who_fighting(vch).Equals(ch));

            if (victim == null)
                return false;

            SkillData skill = DatabaseManager.Instance.GetSkill(spellName);
            if (skill == null || skill.SpellFunction == null)
                return false;

            skill.SpellFunction.Value.DynamicInvoke(new object[] {skill.ID, ch.Level, ch, victim});
            return true;
        }
    }
}