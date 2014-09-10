using Realm.Library.Common;

namespace SmaugCS.Weather
{
    public enum PrecipitationTypes
    {
        [Range(Minimum = 91)] [WeatherMessage(Threshold = WeatherThresholdTypes.DropsBelowTempThreshold,
            Message = "&WThe rain turns to snow as it continues to come down blizzard-like.&D\r\n")] 
        Torrential,

        [Range(Minimum = 81, Maximum = 90)] CatsAndDogs,

        [Range(Minimum = 71, Maximum = 80)] Pouring,

        [Range(Minimum = 61, Maximum = 70)] Heavily,

        [Range(Minimum = 51, Maximum = 60)] Downpour,

        [Range(Minimum = 41, Maximum = 50)] Steadily,

        [Range(Minimum = 31, Maximum = 40)] Raining,

        [Range(Minimum = 21, Maximum = 30)] Lightly,

        [Range(Minimum = 11, Maximum = 20)] Drizzling,

        [Range(Minimum = 1, Maximum = 10)] Misting,

        [Range(Maximum = 0)] None
    }
}
