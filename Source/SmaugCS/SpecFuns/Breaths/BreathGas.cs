using SmaugCS.Common;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions.Character;
using SmaugCS.Repository;

namespace SmaugCS.SpecFuns.Breaths
{
    public static class BreathGas
    {
        public static bool Execute(CharacterInstance ch, IManager dbManager)
        {
            if (!ch.IsInCombatPosition()) return false;

            var databaseMgr = (IRepositoryManager)(dbManager ?? RepositoryManager.Instance);
            var skill = databaseMgr.GetEntity<SkillData>("gas breath");
            if (skill?.SpellFunction == null) return false;

            skill.SpellFunction.Value.Invoke((int)skill.ID, ch.Level, ch, null);
            return true;
        }
    }
}
