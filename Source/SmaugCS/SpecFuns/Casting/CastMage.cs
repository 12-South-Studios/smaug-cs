using Ninject;
using SmaugCS.Common;
using SmaugCS.Data.Instances;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.SpecFuns
{
    public static class CastMage
    {
        public static bool Execute(MobileInstance ch, IManager dbManager)
        {
            var handler = Program.Kernel.Get<ISpecFunHandler>();
            ch.SummonIfHating();

            if (!ch.IsInCombatPosition()) return false;

            var victim = ch.CurrentRoom.Persons.Where(v => v != ch)
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
                {0, new SpecFunSpell {Level = 0, Spell = "magic missile"}},
                {1, new SpecFunSpell {Level = 3, Spell = "chill touch"}},
                {2, new SpecFunSpell {Level = 7, Spell = "weaken"}},
                {3, new SpecFunSpell {Level = 8, Spell = "galvanic whip"}},
                {4, new SpecFunSpell {Level = 11, Spell = "colour spray"}},
                {5, new SpecFunSpell {Level = 12, Spell = "weaken"}},
                {6, new SpecFunSpell {Level = 13, Spell = "energy drain"}},
                {7, new SpecFunSpell {Level = 14, Spell = "spectral furor"}},
                {8, new SpecFunSpell {Level = 15, Spell = "fireball"}},
                {9, new SpecFunSpell {Level = 15, Spell = "fireball"}},
                {-1, new SpecFunSpell {Level = 20, Spell = "acid blast"}}
            };
    }
}
