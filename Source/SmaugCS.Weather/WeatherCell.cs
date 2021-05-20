using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using Realm.Library.Common.Objects;
using SmaugCS.Common.Enumerations;
using SmaugCS.Weather.Enums;

namespace SmaugCS.Weather
{
    public class WeatherCell : Cell
    {
        public ClimateTypes Climate { get; set; }
        public HemisphereTypes Hemisphere { get; set; }
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public int Temperature { get; set; }
        public int Pressure { get; set; }
        public int CloudCover { get; set; }
        public int Humidity { get; set; }
        public int Precipitation { get; set; }
        public int Energy { get; set; }
        public int WindSpeedX { get; set; }
        public int WindSpeedY { get; set; }

        public WeatherCell(int id)
        {
            ID = id;
        }

        #region Setters

        public void ChangeTemperature(int change) => Temperature += change;
        public void ChangePrecip(int change) => Precipitation += change;
        public void ChangePressure(int change) => Pressure += change;
        public void ChangeEnergy(int change) => Energy += change;
        public void ChangeCloudCover(int change) => CloudCover += change;
        public void ChangeHumidity(int change) => Humidity += change;
        public void ChangeWindSpeedX(int change) => WindSpeedX += change;
        public void ChangeWindSpeedY(int change) => WindSpeedY += change;

        #endregion

        #region Statics

        public static CloudCoverTypes GetCloudCover(int cloudCover)
            => EnumerationExtensions.GetValueInRange(cloudCover, CloudCoverTypes.None);

        public static TemperatureTypes GetTemperature(int temp)
            => EnumerationExtensions.GetValueInRange(temp, TemperatureTypes.Temperate);

        public static bool IsHighPressure(int pressure) => pressure > 50;

        public static HumidityTypes GetHumidity(int humidity)
            => EnumerationExtensions.GetValueInRange(humidity, HumidityTypes.None);

        public static PrecipitationTypes GetPrecipitation(int precip)
            => EnumerationExtensions.GetValueInRange(precip, PrecipitationTypes.None);

        public static WindSpeedTypes GetWindSpeed(int speed)
            => EnumerationExtensions.GetValueInRange(speed, WindSpeedTypes.Calm);

        #endregion

    }
}
