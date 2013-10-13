using System;
using Realm.Library.Common;
using SmaugCS.Enums;


namespace SmaugCS.Weather
{
    public class WeatherCell
    {
        public ClimateTypes Climate { get; set; }
        public HemisphereTypes Hemisphere { get; set; }
        public int Temperature { get; set; }
        public int Pressure { get; set; }
        public int CloudCover { get; set; }
        public int Humidity { get; set; }
        public int Precipitation { get; set; }
        public int Energy { get; set; }
        public int WindSpeedX { get; set; }
        public int WindSpeedY { get; set; }

        #region Setters

        public void ChangeTemperature(int change)
        {
            if (change > 0)
                Temperature += change;
            else
                Temperature -= change;
        }

        public void ChangePrecip(int change)
        {
            if (change > 0)
                Precipitation += change;
            else
                Precipitation -= change;
        }

        public void ChangePressure(int change)
        {
            if (change > 0)
                Pressure += change;
            else
                Pressure -= change;
        }

        public void ChangeEnergy(int change)
        {
            if (change > 0)
                Energy += change;
            else
                Energy -= change;
        }

        public void ChangeCloudCover(int change)
        {
            if (change > 0)
                CloudCover += change;
            else
                CloudCover -= change;
        }

        public void ChangeHumidity(int change)
        {
            if (change > 0)
                Humidity += change;
            else
                Humidity -= change;
        }

        public void ChangeWindSpeedX(int change)
        {
            if (change > 0)
                WindSpeedX += change;
            else
                WindSpeedX -= change;
        }

        public void ChangeWindSpeedY(int change)
        {
            if (change > 0)
                WindSpeedY += change;
            else
                WindSpeedY -= change;
        }


        #endregion

        #region Statics

        public static CloudCoverTypes GetCloudCover(int cloudCover)
        {
            if (cloudCover > 80)
                return CloudCoverTypes.Extremely;
            if (cloudCover > 60 && cloudCover <= 80)
                return CloudCoverTypes.Moderately;
            if (cloudCover > 40 && cloudCover <= 60)
                return CloudCoverTypes.Partly;
            if (cloudCover > 20 && cloudCover <= 40)
                return CloudCoverTypes.Slightly;
            return CloudCoverTypes.None;
        }

        public static TemperatureTypes GetTemperature(int temp)
        {
            if (temp > 90)
                return TemperatureTypes.Sweltering;
            if (temp > 80 && temp <= 90)
                return TemperatureTypes.VeryHot;
            if (temp > 70 && temp <= 80)
                return TemperatureTypes.Hot;
            if (temp > 50 && temp <= 60)
                return TemperatureTypes.Temperate;
            if (temp > 40 && temp <= 50)
                return TemperatureTypes.Cool;
            if (temp > 30 && temp <= 40)
                return TemperatureTypes.Chilly;
            if (temp > 20 && temp <= 30)
                return TemperatureTypes.Cold;
            if (temp > 10 && temp <= 20)
                return TemperatureTypes.Frosty;
            if (temp > 0 && temp <= 10)
                return TemperatureTypes.Freezing;
            if (temp > -10 && temp <= 0)
                return TemperatureTypes.ReallyCold;
            if (temp > -20 && temp <= -10)
                return TemperatureTypes.VeryCold;
            return TemperatureTypes.ExtremelyCold;
        }

        public static bool IsHighPressure(int pressure)
        {
            return pressure > 50;
        }

        public static HumidityTypes GetHumidity(int humidity)
        {
            if (humidity > 60)
                return HumidityTypes.Extremely;
            if (humidity > 60 && humidity <= 80)
                return HumidityTypes.Moderately;
            if (humidity > 40 && humidity <= 60)
                return HumidityTypes.Minorly;
            if (humidity > 20 && humidity <= 40)
                return HumidityTypes.Humid;
            return HumidityTypes.None;
        }

        public static PrecipitationTypes GetPrecipitation(int precip)
        {
            if (precip > 90)
                return PrecipitationTypes.Torrential;
            if (precip > 80 && precip <= 90)
                return PrecipitationTypes.CatsAndDogs;
            if (precip > 70 && precip <= 80)
                return PrecipitationTypes.Pouring;
            if (precip > 60 && precip <= 70)
                return PrecipitationTypes.Heavily;
            if (precip > 50 && precip <= 60)
                return PrecipitationTypes.Downpour;
            if (precip > 40 && precip <= 50)
                return PrecipitationTypes.Steadily;
            if (precip > 30 && precip <= 40)
                return PrecipitationTypes.Raining;
            if (precip > 20 && precip <= 30)
                return PrecipitationTypes.Lightly;
            if (precip > 10 && precip <= 20)
                return PrecipitationTypes.Drizzling;
            if (precip > 0 && precip <= 10)
                return PrecipitationTypes.Misting;
            return PrecipitationTypes.None;
        }

        public static WindSpeedTypes GetWindSpeed(int speed)
        {
            if (speed > 80)
                return WindSpeedTypes.GaleForce;
            if (speed < 60 && speed <= 80)
                return WindSpeedTypes.Gusty;
            if (speed > 40 && speed <= 60)
                return WindSpeedTypes.Windy;
            if (speed > 20 && speed <= 40)
                return WindSpeedTypes.Blustery;
            if (speed > 10 && speed <= 20)
                return WindSpeedTypes.Breezy;
            return WindSpeedTypes.Calm;
        }

        #endregion

        public void Load(TextReaderProxy proxy)
        {
            string word = string.Empty;

            do
            {
                word = proxy.EndOfStream ? "End" : proxy.ReadNextWord();

                switch (Char.ToUpper(word.ToCharArray()[0]))
                {
                    case '*':
                        proxy.ReadToEndOfLine();
                        break;
                    case 'C':
                        Climate = EnumerationExtensions.GetEnum<ClimateTypes>(proxy.ReadNumber());
                        break;
                    case 'E':
                        if (word.Equals("End"))
                            return;
                        break;
                    case 'H':
                        Hemisphere = EnumerationExtensions.GetEnum<HemisphereTypes>(proxy.ReadNumber());
                        break;
                    case 'S':
                        if (word.Equals("State", StringComparison.OrdinalIgnoreCase))
                        {
                            CloudCover = proxy.ReadNumber();
                            Energy = proxy.ReadNumber();
                            Humidity = proxy.ReadNumber();
                            Precipitation = proxy.ReadNumber();
                            Pressure = proxy.ReadNumber();
                            Temperature = proxy.ReadNumber();
                            WindSpeedX = proxy.ReadNumber();
                            WindSpeedY = proxy.ReadNumber();
                        }
                        break;
                }
            } while (!proxy.EndOfStream && !word.Equals("End", StringComparison.OrdinalIgnoreCase));
        }
    }
}
