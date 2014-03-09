using System;
using System.Collections.Generic;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.SpecFuns
{
    public static class CastCleric
    {
        public static bool DoSpecCastCleric(CharacterInstance ch)
        {
            SpecFunHandler handler = new SpecFunHandler(DatabaseManager.Instance);
            handler.summon_if_hating(ch);

            if (!ch.IsInCombatPosition())
                return false;

            CharacterInstance victim =
                ch.CurrentRoom.Persons.Where(v => !v.Equals(ch))
                  .FirstOrDefault(vch => SmaugRandom.Bits(2) == 0 && fight.who_fighting(vch).Equals(ch));

            if (victim == null || victim.Equals(ch))
                return false;

            SkillData skill = handler.PickSpell(SpellLevelLookupTable, ch.Level);
            if (skill == null || skill.SpellFunction == null)
                return false;

            skill.SpellFunction.Value.DynamicInvoke(new object[] { skill.ID, ch.Level, ch, victim });
            return true;
        }

        private static readonly Dictionary<int, Tuple<int, string>> SpellLevelLookupTable = new Dictionary<int, Tuple<int, string>>()
            {
                {0, new Tuple<int, string>(0, "cause light")},
                {1, new Tuple<int, string>(3, "cause serious")},
                {2, new Tuple<int, string>(6, "earthquake")},
                {3, new Tuple<int, string>(7, "blindness")},
                {4, new Tuple<int, string>(9, "cause critical")},
                {5, new Tuple<int, string>(10, "dispel evil")},
                {6, new Tuple<int, string>(12, "curse")},
                {7, new Tuple<int, string>(13, "flamestrike")},
                {8, new Tuple<int, string>(15, "harm")},
                {9, new Tuple<int, string>(15, "harm")},
                {10, new Tuple<int, string>(15, "harm")},
                {-1, new Tuple<int, string>(16, "dispel magic")}
            }; 
    }
}
