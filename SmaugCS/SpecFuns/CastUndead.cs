using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using SmaugCS.Common;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.SpecFuns
{
    public static class CastUndead
    {
        public static bool DoSpecCastUndead(MobileInstance ch)
        {
            ISpecFunHandler handler = Program.Kernel.Get<ISpecFunHandler>();
            ch.SummonIfHating();

            if (!ch.IsInCombatPosition())
                return false;

            CharacterInstance victim =
                ch.CurrentRoom.Persons.Where(v => v != ch)
                  .FirstOrDefault(vch => SmaugRandom.Bits(2) == 0 && vch.GetMyTarget() == ch);

            if (victim == null || victim == ch)
                return false;

            SkillData skill = handler.PickSpell(SpellLevelLookupTable, ch.Level);
            if (skill == null || skill.SpellFunction == null)
                return false;

            skill.SpellFunction.Value.DynamicInvoke(new object[] { skill.ID, ch.Level, ch, victim });
            return true;
        }

        private static readonly Dictionary<int, Tuple<int, string>> SpellLevelLookupTable 
            = new Dictionary<int, Tuple<int, string>>
            {
                {0, new Tuple<int, string>(0, "chill touch")},
                {1, new Tuple<int, string>(11, "weaken")},
                {2, new Tuple<int, string>(12, "curse")},
                {3, new Tuple<int, string>(13, "blindness")},
                {4, new Tuple<int, string>(14, "poison")},
                {5, new Tuple<int, string>(15, "energy drain")},
                {6, new Tuple<int, string>(18, "harm")},
                {-1, new Tuple<int, string>(40, "gate")}
            };
    }
}
