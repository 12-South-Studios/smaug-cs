using NUnit.Framework;
using SmaugCS.Weather.Enums;

namespace SmaugCS.Weather.Tests
{
    [TestFixture]
    public class WeatherCellTests
    {
        [TestCase(25, 5, 30)]
        [TestCase(25, -5, 20)]
        public void TemperatueChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) {Temperature = startValue};

            cell.ChangeTemperature(changeValue);

            Assert.That(cell.Temperature, Is.EqualTo(expectedValue));
        }

        [TestCase(25, 5, 30)]
        [TestCase(25, -5, 20)]
        public void PrecipChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) {Precipitation = startValue};

            cell.ChangePrecip(changeValue);

            Assert.That(cell.Precipitation, Is.EqualTo(expectedValue));
        }

        [TestCase(25, 5, 30)]
        [TestCase(25, -5, 20)]
        public void PressureChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) {Pressure = startValue};

            cell.ChangePressure(changeValue);

            Assert.That(cell.Pressure, Is.EqualTo(expectedValue));
        }

        [TestCase(25, 5, 30)]
        [TestCase(25, -5, 20)]
        public void EnergyChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) {Energy = startValue};

            cell.ChangeEnergy(changeValue);

            Assert.That(cell.Energy, Is.EqualTo(expectedValue));
        }

        [TestCase(25, 5, 30)]
        [TestCase(25, -5, 20)]
        public void CloudCoverChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) {CloudCover = startValue};

            cell.ChangeCloudCover(changeValue);

            Assert.That(cell.CloudCover, Is.EqualTo(expectedValue));
        }

        [TestCase(25, 5, 30)]
        [TestCase(25, -5, 20)]
        public void HumidityChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) {Humidity = startValue};

            cell.ChangeHumidity(changeValue);

            Assert.That(cell.Humidity, Is.EqualTo(expectedValue));
        }

        [TestCase(25, 5, 30)]
        [TestCase(25, -5, 20)]
        public void WindSpeedXChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) {WindSpeedX = startValue};

            cell.ChangeWindSpeedX(changeValue);

            Assert.That(cell.WindSpeedX, Is.EqualTo(expectedValue));
        }

        [TestCase(25, 5, 30)]
        [TestCase(25, -5, 20)]
        public void WindSpeedYChangeTest(int startValue, int changeValue, int expectedValue)
        {
            var cell = new WeatherCell(1) {WindSpeedY = startValue};

            cell.ChangeWindSpeedY(changeValue);

            Assert.That(cell.WindSpeedY, Is.EqualTo(expectedValue));
        }

        [TestCase(90, CloudCoverTypes.Extremely)]
        [TestCase(70, CloudCoverTypes.Moderately)]
        [TestCase(50, CloudCoverTypes.Partly)]
        [TestCase(30, CloudCoverTypes.Slightly)]
        [TestCase(10, CloudCoverTypes.None)]
        [TestCase(-10, CloudCoverTypes.None)]
        public void GetCloudCoverTest(int cloudCover, CloudCoverTypes expectedValue)
        {
            Assert.That(WeatherCell.GetCloudCover(cloudCover), Is.EqualTo(expectedValue));
        }

        [TestCase(100, TemperatureTypes.Sweltering)]
        [TestCase(90, TemperatureTypes.VeryHot)]
        [TestCase(80, TemperatureTypes.Hot)]
        [TestCase(70, TemperatureTypes.Warm)]
        [TestCase(60, TemperatureTypes.Temperate)]
        [TestCase(50, TemperatureTypes.Cool)]
        [TestCase(40, TemperatureTypes.Chilly)]
        [TestCase(30, TemperatureTypes.Cold)]
        [TestCase(20, TemperatureTypes.Frosty)]
        [TestCase(10, TemperatureTypes.Freezing)]
        [TestCase(0, TemperatureTypes.ReallyCold)]
        [TestCase(-10, TemperatureTypes.VeryCold)]
        [TestCase(-20, TemperatureTypes.ExtremelyCold)]
        [TestCase(-30, TemperatureTypes.ExtremelyCold)]
        public void GetTemperatureTest(int temp, TemperatureTypes expectedValue)
        {
            Assert.That(WeatherCell.GetTemperature(temp), Is.EqualTo(expectedValue));
        }

        [TestCase(90, HumidityTypes.Extremely)]
        [TestCase(70, HumidityTypes.Moderately)]
        [TestCase(50, HumidityTypes.Minorly)]
        [TestCase(30, HumidityTypes.Humid)]
        [TestCase(10, HumidityTypes.None)]
        public void GetHumidityTest(int humid, HumidityTypes expectedValue)
        {
            Assert.That(WeatherCell.GetHumidity(humid), Is.EqualTo(expectedValue));
        }

        [TestCase(95, PrecipitationTypes.Torrential)]
        [TestCase(85, PrecipitationTypes.CatsAndDogs)]
        [TestCase(75, PrecipitationTypes.Pouring)]
        [TestCase(65, PrecipitationTypes.Heavily)]
        [TestCase(55, PrecipitationTypes.Downpour)]
        [TestCase(45, PrecipitationTypes.Steadily)]
        [TestCase(35, PrecipitationTypes.Raining)]
        [TestCase(25, PrecipitationTypes.Lightly)]
        [TestCase(15, PrecipitationTypes.Drizzling)]
        [TestCase(5, PrecipitationTypes.Misting)]
        [TestCase(-5, PrecipitationTypes.None)]
        public void GetPrecipitationTest(int precip, PrecipitationTypes expectedValue)
        {
            Assert.That(WeatherCell.GetPrecipitation(precip), Is.EqualTo(expectedValue));
        }

        [TestCase(90, WindSpeedTypes.GaleForce)]
        [TestCase(70, WindSpeedTypes.Gusty)]
        [TestCase(50, WindSpeedTypes.Windy)]
        [TestCase(30, WindSpeedTypes.Blustery)]
        [TestCase(15, WindSpeedTypes.Breezy)]
        [TestCase(5, WindSpeedTypes.Calm)]
        public void GetWindSpeedTest(int speed, WindSpeedTypes expectedValue)
        {
            Assert.That(WeatherCell.GetWindSpeed(speed), Is.EqualTo(expectedValue));
        }

        [TestCase(75, true)]
        [TestCase(25, false)]
        public void IsHighPressureTest(int pressure, bool expectedValue)
        {
            Assert.That(WeatherCell.IsHighPressure(pressure), Is.EqualTo(expectedValue));
        }
    }
}
