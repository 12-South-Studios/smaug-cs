using Realm.Library.Common;
using SmaugCS.Common.Enumerations;

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

        public void ChangeTemperature(int change)
        {
            Temperature += change;
        }

        public void ChangePrecip(int change)
        {
            Precipitation += change;
        }

        public void ChangePressure(int change)
        {
            Pressure += change;
        }

        public void ChangeEnergy(int change)
        {
            Energy += change;
        }

        public void ChangeCloudCover(int change)
        {
            CloudCover += change;
        }

        public void ChangeHumidity(int change)
        {
            Humidity += change;
        }

        public void ChangeWindSpeedX(int change)
        {
            WindSpeedX += change;
        }

        public void ChangeWindSpeedY(int change)
        {
            WindSpeedY += change;
        }


        #endregion

        #region Statics

        public static CloudCoverTypes GetCloudCover(int cloudCover)
        {
            return EnumerationExtensions.GetValueInRange(cloudCover, CloudCoverTypes.None);
        }

        public static TemperatureTypes GetTemperature(int temp)
        {
            return EnumerationExtensions.GetValueInRange(temp, TemperatureTypes.Temperate);
        }

        public static bool IsHighPressure(int pressure)
        {
            return pressure > 50;
        }

        public static HumidityTypes GetHumidity(int humidity)
        {
            return EnumerationExtensions.GetValueInRange(humidity, HumidityTypes.None);
        }

        public static PrecipitationTypes GetPrecipitation(int precip)
        {
            return EnumerationExtensions.GetValueInRange(precip, PrecipitationTypes.None);
        }

        public static WindSpeedTypes GetWindSpeed(int speed)
        {
            return EnumerationExtensions.GetValueInRange(speed, WindSpeedTypes.Calm);
        }

        #endregion

    }
}
