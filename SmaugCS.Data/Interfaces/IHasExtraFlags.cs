using SmaugCS.Common;

// ReSharper disable CheckNamespace
namespace SmaugCS.Data
// ReSharper restore CheckNamespace
{
    public interface IHasExtraFlags
    {
        ExtendedBitvector ExtraFlags { get; set; }
    }
}
