using Realm.Library.Common.Attributes;

namespace Realm.Library.Common
{
    public enum DoublePrecisionComparisonTypes
    {
        [Precision(0.01)]
        TwoDigits,

        [Precision(0.001)]
        ThreeDigits,

        [Precision(0.0001)]
        FourDigits,

        [Precision(0.00001)]
        FiveDigits,

        [Precision(0.000001)]
        SixDigits,

        [Precision(0.0000001)]
        SevenDigits
    }
}
