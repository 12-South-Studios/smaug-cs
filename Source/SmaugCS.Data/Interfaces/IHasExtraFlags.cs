using SmaugCS.Common;

namespace SmaugCS.Data.Interfaces;

public interface IHasExtraFlags
{
    ExtendedBitvector ExtraFlags { get; set; }
}