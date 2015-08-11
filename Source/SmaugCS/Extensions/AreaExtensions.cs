using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.Extensions
{
    public static class AreaExtensions
    {
        public static bool IsInSoftRange(this AreaData area, CharacterInstance ch)
        {
            if (ch.IsImmortal() || ch.IsNpc()) return true;
            return ch.Level >= area.LowSoftRange && ch.Level <= area.HighSoftRange;
        }

        public static bool IsInHardRange(this AreaData area, CharacterInstance ch)
        {
            if (ch.IsImmortal() || ch.IsNpc()) return true;
            return ch.Level >= area.LowHardRange && ch.Level <= area.HighHardRange;
        }
    }
}
