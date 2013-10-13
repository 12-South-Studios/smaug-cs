using SmaugCS.Common;
using SmaugCS.Objects;

namespace SmaugCS.Interfaces
{
    public interface IHasExtraFlags
    {
        ExtendedBitvector ExtraFlags { get; set; }
    }
}
