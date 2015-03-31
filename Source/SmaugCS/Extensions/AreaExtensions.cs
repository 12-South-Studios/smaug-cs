using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.Extensions
{
    public static class AreaExtensions
    {
        public static bool IsInSoftRange(this AreaData area, CharacterInstance ch)
        {
            if (ch.IsImmortal()) return true;
            if (ch.IsNpc()) return true;
            if (ch.Level >= area.LowSoftRange && ch.Level <= area.HighSoftRange) return true;
            return false;
        }

        public static bool IsInHardRange(this AreaData area, CharacterInstance ch)
        {
            if (ch.IsImmortal()) return true;
            if (ch.IsNpc()) return true;
            if (ch.Level >= area.LowHardRange && ch.Level <= area.HighHardRange) return true;
            return false;
        }
    }
}
