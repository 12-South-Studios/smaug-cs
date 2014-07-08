using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Weather
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
