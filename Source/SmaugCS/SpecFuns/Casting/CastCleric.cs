using Ninject;
using SmaugCS.Common;
using SmaugCS.Data.Instances;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.SpecFuns
{
    public static class CastCleric
    {
        public static bool Execute(MobileInstance ch, IManager dbManager)
        {
            var handler = Program.Kernel.Get<ISpecFunHandler>();
            ch.SummonIfHating();

            if (!ch.IsInCombatPosition()) return false;

            var victim =
                ch.CurrentRoom.Persons.Where(v => v != ch)
                  .FirstOrDefault(vch => SmaugRandom.Bits(2) == 0 && vch.GetMyTarget() == ch);

            if (victim == null || victim == ch) return false;

            var skill = handler.PickSpell(SpellLevelLookupTable, ch.Level);
            if (skill?.SpellFunction == null) return false;

            skill.SpellFunction.Value.DynamicInvoke(skill.ID, ch.Level, ch, victim);
            return true;
        }

        private static readonly Dictionary<int, SpecFunSpell> SpellLevelLookupTable
            = new Dictionary<int, SpecFunSpell>
            {
                {0, new SpecFunSpell {Level = 0, Spell = "cause light"}},
                {1, new SpecFunSpell {Level = 3, Spell = "cause serious"}},
                {2, new SpecFunSpell {Level = 6, Spell = "earthquake"}},
                {3, new SpecFunSpell {Level = 7, Spell = "blindness"}},
                {4, new SpecFunSpell {Level = 9, Spell = "cause critical"}},
                {5, new SpecFunSpell {Level = 10, Spell = "dispel evil"}},
                {6, new SpecFunSpell {Level = 12, Spell = "curse"}},
                {7, new SpecFunSpell {Level = 13, Spell = "flamestrike"}},
                {8, new SpecFunSpell {Level = 15, Spell = "harm"}},
                {9, new SpecFunSpell {Level = 15, Spell = "harm"}},
                {10, new SpecFunSpell {Level = 15, Spell = "harm"}},
                {-1, new SpecFunSpell {Level = 16, Spell = "dispel magic"}}
            };
    }
}
