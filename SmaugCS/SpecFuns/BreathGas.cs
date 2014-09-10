using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Managers;

namespace SmaugCS.SpecFuns
{
    public static class BreathGas
    {
        public static bool DoSpecBreathGas(MobileInstance ch)
        {
            if (!ch.IsInCombatPosition())
                return false;

            SkillData skill = DatabaseManager.Instance.GetEntity<SkillData>("gas breath");
            if (skill == null || skill.SpellFunction == null)
                return false;

            skill.SpellFunction.Value.Invoke((int)skill.ID, ch.Level, ch, null);
            return true;
        }
    }
}
