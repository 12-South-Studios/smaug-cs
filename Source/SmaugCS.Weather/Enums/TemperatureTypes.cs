using Realm.Library.Common;

namespace SmaugCS.Weather.Enums
{
    public enum TemperatureTypes
    {
        [Range(Minimum = 91)]
        Sweltering,

        [Range(Minimum = 81, Maximum = 90)]
        VeryHot,

        [Range(Minimum = 71, Maximum = 80)]
        Hot,

        [Range(Minimum = 61, Maximum = 70)]
        Warm,

        [Range(Minimum = 51, Maximum = 60)]
        Temperate,

        [Range(Minimum = 41, Maximum = 50)]
        Cool,

        [Range(Minimum = 31, Maximum = 40)]
        Chilly,

        [Range(Minimum = 21, Maximum = 30)]
        Cold,

        [Range(Minimum = 11, Maximum = 20)]
        Frosty,

        [Range(Minimum = 1, Maximum = 10)]
        Freezing,

        [Range(Minimum = -9, Maximum = 0)]
        ReallyCold,

        [Range(Minimum = -19, Maximum = -10)]
        VeryCold,

        [Range(Maximum = -20)]
        ExtremelyCold
    }
}
