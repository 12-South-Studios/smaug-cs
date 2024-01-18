using FluentAssertions;
using SmaugCS.Weather.Enums;
using Xunit;

namespace SmaugCS.Weather.Tests
{

    public class WeatherCellTests
    {
        [Theory]
        [InlineData(25, 5, 30)]
        [InlineData(25, -5, 20)]
        public void TemperatueChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) { Temperature = startValue };

            cell.ChangeTemperature(changeValue);

            cell.Temperature.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(25, 5, 30)]
        [InlineData(25, -5, 20)]
        public void PrecipChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) { Precipitation = startValue };

            cell.ChangePrecip(changeValue);

            cell.Precipitation.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(25, 5, 30)]
        [InlineData(25, -5, 20)]
        public void PressureChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) { Pressure = startValue };

            cell.ChangePressure(changeValue);

            cell.Pressure.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(25, 5, 30)]
        [InlineData(25, -5, 20)]
        public void EnergyChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) { Energy = startValue };

            cell.ChangeEnergy(changeValue);

            cell.Energy.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(25, 5, 30)]
        [InlineData(25, -5, 20)]
        public void CloudCoverChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) { CloudCover = startValue };

            cell.ChangeCloudCover(changeValue);

            cell.CloudCover.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(25, 5, 30)]
        [InlineData(25, -5, 20)]
        public void HumidityChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) { Humidity = startValue };

            cell.ChangeHumidity(changeValue);

            cell.Humidity.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(25, 5, 30)]
        [InlineData(25, -5, 20)]
        public void WindSpeedXChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) { WindSpeedX = startValue };

            cell.ChangeWindSpeedX(changeValue);

            cell.WindSpeedX.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(25, 5, 30)]
        [InlineData(25, -5, 20)]
        public void WindSpeedYChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) { WindSpeedY = startValue };

            cell.ChangeWindSpeedY(changeValue);

            cell.WindSpeedY.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(90, CloudCoverTypes.Extremely)]
        [InlineData(70, CloudCoverTypes.Moderately)]
        [InlineData(50, CloudCoverTypes.Partly)]
        [InlineData(30, CloudCoverTypes.Slightly)]
        [InlineData(10, CloudCoverTypes.None)]
        [InlineData(-10, CloudCoverTypes.None)]
        public void GetCloudCoverTest(int cloudCover, CloudCoverTypes expectedValue)
        {
            WeatherCell.GetCloudCover(cloudCover).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(100, TemperatureTypes.Sweltering)]
        [InlineData(90, TemperatureTypes.VeryHot)]
        [InlineData(80, TemperatureTypes.Hot)]
        [InlineData(70, TemperatureTypes.Warm)]
        [InlineData(60, TemperatureTypes.Temperate)]
        [InlineData(50, TemperatureTypes.Cool)]
        [InlineData(40, TemperatureTypes.Chilly)]
        [InlineData(30, TemperatureTypes.Cold)]
        [InlineData(20, TemperatureTypes.Frosty)]
        [InlineData(10, TemperatureTypes.Freezing)]
        [InlineData(0, TemperatureTypes.ReallyCold)]
        [InlineData(-10, TemperatureTypes.VeryCold)]
        [InlineData(-20, TemperatureTypes.ExtremelyCold)]
        [InlineData(-30, TemperatureTypes.ExtremelyCold)]
        public void GetTemperatureTest(int temp, TemperatureTypes expectedValue)
        {
            WeatherCell.GetTemperature(temp).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(90, HumidityTypes.Extremely)]
        [InlineData(70, HumidityTypes.Moderately)]
        [InlineData(50, HumidityTypes.Minorly)]
        [InlineData(30, HumidityTypes.Humid)]
        [InlineData(10, HumidityTypes.None)]
        public void GetHumidityTest(int humid, HumidityTypes expectedValue)
        {
            WeatherCell.GetHumidity(humid).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(95, PrecipitationTypes.Torrential)]
        [InlineData(85, PrecipitationTypes.CatsAndDogs)]
        [InlineData(75, PrecipitationTypes.Pouring)]
        [InlineData(65, PrecipitationTypes.Heavily)]
        [InlineData(55, PrecipitationTypes.Downpour)]
        [InlineData(45, PrecipitationTypes.Steadily)]
        [InlineData(35, PrecipitationTypes.Raining)]
        [InlineData(25, PrecipitationTypes.Lightly)]
        [InlineData(15, PrecipitationTypes.Drizzling)]
        [InlineData(5, PrecipitationTypes.Misting)]
        [InlineData(-5, PrecipitationTypes.None)]
        public void GetPrecipitationTest(int precip, PrecipitationTypes expectedValue)
        {
            WeatherCell.GetPrecipitation(precip).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(90, WindSpeedTypes.GaleForce)]
        [InlineData(70, WindSpeedTypes.Gusty)]
        [InlineData(50, WindSpeedTypes.Windy)]
        [InlineData(30, WindSpeedTypes.Blustery)]
        [InlineData(15, WindSpeedTypes.Breezy)]
        [InlineData(5, WindSpeedTypes.Calm)]
        public void GetWindSpeedTest(int speed, WindSpeedTypes expectedValue)
        {
            WeatherCell.GetWindSpeed(speed).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(75, true)]
        [InlineData(25, false)]
        public void IsHighPressureTest(int pressure, bool expectedValue)
        {
            WeatherCell.IsHighPressure(pressure).Should().Be(expectedValue);
        }
    }
}
