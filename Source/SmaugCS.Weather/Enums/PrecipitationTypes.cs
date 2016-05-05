using Realm.Library.Common;

namespace SmaugCS.Weather.Enums
{
    /// <summary>
    /// Messages start in weather.c:176
    /// </summary>
    public enum PrecipitationTypes
    {
        [Range(Minimum = 91)]
        [WeatherMessage(Threshold = WeatherThresholdTypes.DropsBelowTempThreshold,
            Message = "&WThe rain turns to snow as it continues to come down blizzard-like.&D")]
        [WeatherMessage(Threshold = WeatherThresholdTypes.ExceedsTempThreshold, 
            Message = "&BThe blizzard turns to a cold rain as it continues to come in a torrential downpour.&D")]
        [WeatherMessage(Threshold = WeatherThresholdTypes.ExceedsPrecipThresholdInStorm, 
            Message = "&BThe rain begins to increase in intensity falling heavily and quickly.&D\r\n&YThunder and lightning shake the ground and light up the sky.&D")]
        [WeatherMessage(Threshold = WeatherThresholdTypes.ExceedsPrecipThreshold, 
            Message = "&BThe rain begins to increase in intensity falling heavily and quickly.&D")]
        [WeatherMessage(Threshold = WeatherThresholdTypes.ExceedsPrecipThresholdAndDropsBelowFreezing, 
            Message = "&WThe rain changes over to snow as the intensity increases, making a blinding white wall.&D")]
        [WeatherMessage(Threshold = WeatherThresholdTypes.ExceedsPrecipThresholdIsFreezing, 
            Message = "&WThe heavy snow increases and freezes creating a blizzard.&D")]
        [WeatherMessage(Threshold = WeatherThresholdTypes.ExceedsPrecipAndTempThresholdsInStorm, 
            Message = "&BThe snow changes over to rain as it pounds down heavier.&D\r\n&YThunder and lightning begin to shake the gound and light up the sky.&D")]
        [WeatherMessage(Threshold = WeatherThresholdTypes.ExceedsPrecipAndTempThresholds, 
            Message = "&BThe snow changes over to rain as it pounds down heavier.&D")]
        [WeatherMessage(Threshold = WeatherThresholdTypes.IsBelowFreezing, 
            Message = "&WThe snow falls down fast and steady creating a blizzard.&D")]
        [WeatherMessage(Threshold = WeatherThresholdTypes.IsStormy, 
            Message = "&BThe rain continues to pound the earth in a downpour.&D\r\n&YThunder and lightning boom and cackle and light up the sky.&D")]
        [WeatherMessage(Threshold = WeatherThresholdTypes.None, Message = "&BThe rain continues to pound the earth in a downpour.&D")]
        Torrential,

        [Range(Minimum = 81, Maximum = 90)]
        CatsAndDogs,

        [Range(Minimum = 71, Maximum = 80)]
        Pouring,

        [Range(Minimum = 61, Maximum = 70)]
        Heavily,

        [Range(Minimum = 51, Maximum = 60)]
        Downpour,

        [Range(Minimum = 41, Maximum = 50)]
        Steadily,

        [Range(Minimum = 31, Maximum = 40)]
        Raining,

        [Range(Minimum = 21, Maximum = 30)]
        Lightly,

        [Range(Minimum = 11, Maximum = 20)]
        Drizzling,

        [Range(Minimum = 1, Maximum = 10)]
        Misting,

        [Range(Maximum = 0)]
        None
    }
}
