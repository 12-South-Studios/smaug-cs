namespace SmaugCS.Weather.Enums
{
    public enum WeatherThresholdTypes
    {
        DropsBelowTempThreshold,
        ExceedsTempThreshold,
        ExceedsPrecipThresholdInStorm,
        ExceedsPrecipThreshold,
        ExceedsPrecipThresholdAndDropsBelowFreezing,
        ExceedsPrecipThresholdIsFreezing,
        ExceedsPrecipAndTempThresholdsInStorm,
        ExceedsPrecipAndTempThresholds,
        IsBelowFreezing,
        IsStormy,
        None
    }
}
