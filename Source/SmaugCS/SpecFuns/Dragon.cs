using System.Linq;
using SmaugCS.Common;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Managers;

namespace SmaugCS.SpecFuns
{
    public static class Dragon
    {
        public static bool DoSpecDragon(MobileInstance ch, string spellName)
        {
            if (!ch.IsInCombatPosition()) return false;

            var victim = ch.CurrentRoom.Persons.Where(v => v != ch)
                  .FirstOrDefault(vch => SmaugRandom.Bits(2) == 0 && vch.GetMyTarget() == ch);
            if (victim == null) return false;

            var skill = DatabaseManager.Instance.GetEntity<SkillData>(spellName);
            if (skill == null || skill.SpellFunction == null) return false;

            skill.SpellFunction.Value.Invoke((int)skill.ID, ch.Level, ch, victim);
            return true;
        }
    }
}
