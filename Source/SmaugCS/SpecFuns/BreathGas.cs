using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Managers;
using SmaugCS.Repository;

namespace SmaugCS.SpecFuns
{
    public static class BreathGas
    {
        public static bool DoSpecBreathGas(CharacterInstance ch)
        {
            if (!ch.IsInCombatPosition()) return false;

            var skill = RepositoryManager.Instance.GetEntity<SkillData>("gas breath");
            if (skill == null || skill.SpellFunction == null) return false;

            skill.SpellFunction.Value.Invoke((int)skill.ID, ch.Level, ch, null);
            return true;
        }
    }
}
