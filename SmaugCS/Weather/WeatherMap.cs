using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Managers;

namespace SmaugCS.Weather
{
    public class WeatherMap
    {
        private readonly WeatherCell[,] Map;
        private readonly WeatherCell[,] Delta;

        public List<string> StarMap { get; private set; }
        public List<string> SunMap { get; private set; }
        public List<string> MoonMap { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public static string WeatherFile
        {
            get { return SystemConstants.GetSystemFile(SystemFileTypes.Weather); }
        }

        #region Function Maps
        private static readonly Dictionary<ClimateTypes, Action<WeatherCell, WeatherCell>> EnforceFuncs = new Dictionary<ClimateTypes, Action<WeatherCell, WeatherCell>>()
            {
                 {ClimateTypes.Rainforest, EnforceRainforest },
                 {ClimateTypes.Savanna, EnforceSavanna},
                 {ClimateTypes.Desert, EnforceDesert},
                 {ClimateTypes.Steppe, EnforceSteppe},
                 {ClimateTypes.Chapparal, EnforceChapparal},
                 {ClimateTypes.Grasslands, EnforceGrasslands},
                 {ClimateTypes.Deciduous, EnforceDeciduous},
                 {ClimateTypes.Taiga, EnforceTaiga},
                 {ClimateTypes.Tundra, EnforceTundra},
                 {ClimateTypes.Alpine, EnforceAlpine},
                 {ClimateTypes.Arctic, EnforceArctic}                                                                          
            };
        private static readonly Dictionary<PrecipitationTypes, Action<WeatherCell, WeatherCell>> PrecipFuncs = new Dictionary<PrecipitationTypes, Action<WeatherCell, WeatherCell>>()
            {
                {PrecipitationTypes.Torrential, TorrentialWeatherMessage},
                {PrecipitationTypes.CatsAndDogs, CatsAndDogsWeatherMessage},
                {PrecipitationTypes.Pouring, PouringWeatherMessage},
                {PrecipitationTypes.Heavily, HeavilyWeatherMessage},
                {PrecipitationTypes.Downpour, DownpourWeatherMessage},
                {PrecipitationTypes.Steadily, SteadilyWeatherMessage},
                {PrecipitationTypes.Raining, RainingWeatherMessage},
                {PrecipitationTypes.Lightly, LightlyWeatherMessage},
                {PrecipitationTypes.Drizzling, DrizzlingWeatherMessage},
                {PrecipitationTypes.Misting, MistingWeatherMessage},
                {PrecipitationTypes.None, NoPrecipWeatherMessage}
            };
        #endregion

        public WeatherMap(int width, int height)
        {
            Width = width;
            Height = height;

            Map = new WeatherCell[width, height];
            Delta = new WeatherCell[width, height];

            StarMap = new List<string>();
            SunMap = new List<string>();
            MoonMap = new List<string>();
        }

        public void LoadMap(SystemFileTypes fileType, List<string> map)
        {
            string path = SystemConstants.GetSystemFile(fileType);
            using (
                TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                List<string> lines = proxy.ReadIntoList();
                if (lines.Count == 0)
                    throw new InvalidDataException(string.Format("Missing data for {0}", fileType));

                map.AddRange(lines);
            }
        }

        public WeatherCell GetCellFromMap(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Map.GetLength(0) || y >= Map.GetLength(1))
                return null;

            return Map[x, y];
        }
        public WeatherCell GetCellFromDelta(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Delta.GetLength(0) || y >= Delta.GetLength(1))
                return null;

            return Delta[x, y];
        }

        public void Initialize()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    WeatherCell cell = new WeatherCell
                                           {
                                               Hemisphere = EnumerationExtensions.GetEnum<HemisphereTypes>(SmaugCS.Common.SmaugRandom.Between(0, 1))
                                           };

                    cell.ChangeTemperature(SmaugCS.Common.SmaugRandom.Between(-30, 100));
                    cell.ChangePressure(SmaugCS.Common.SmaugRandom.Between(0, 100));
                    cell.ChangeCloudCover(SmaugCS.Common.SmaugRandom.Between(0, 100));
                    cell.ChangeHumidity(SmaugCS.Common.SmaugRandom.Between(0, 100));
                    cell.ChangePrecip(SmaugCS.Common.SmaugRandom.Between(0, 100));
                    cell.ChangeWindSpeedX(SmaugCS.Common.SmaugRandom.Between(-100, 100));
                    cell.ChangeWindSpeedY(SmaugCS.Common.SmaugRandom.Between(-100, 100));
                    cell.ChangeEnergy(SmaugCS.Common.SmaugRandom.Between(0, 100));

                    Map[x, y] = cell;
                }
            }
        }

        public void EnforceClimateConditions()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    WeatherCell cell = GetCellFromMap(x, y);
                    WeatherCell delta = GetCellFromDelta(x, y);

                    if (!EnforceFuncs.ContainsKey(cell.Climate))
                        continue;

                    EnforceFuncs[cell.Climate].Invoke(cell, delta);
                }
            }
        }

        #region Enforce Climate Functions
        private static void EnforceRainforest(WeatherCell cell, WeatherCell delta)
        {
            if (cell.Temperature < 80)
                delta.ChangeTemperature(3);
            else if (cell.Temperature < 50)
                delta.ChangeHumidity(2);
        }
        private static void EnforceSavanna(WeatherCell cell, WeatherCell delta)
        {
            if (db.GameTime.Season == SeasonTypes.Winter
                && cell.Hemisphere == HemisphereTypes.North
                && cell.Humidity > 0)
                delta.ChangeHumidity(-5);
            else if (cell.Temperature < 60)
                delta.ChangePrecip(2);
            else if (db.GameTime.Season == SeasonTypes.Summer
                && cell.Hemisphere == HemisphereTypes.North
                && cell.Humidity < 50)
                delta.ChangeHumidity(5);
            else if (db.GameTime.Season == SeasonTypes.Summer
                && cell.Hemisphere == HemisphereTypes.South
                && cell.Humidity > 0)
                delta.ChangeHumidity(-5);
            else if (db.GameTime.Season == SeasonTypes.Winter
                && cell.Hemisphere == HemisphereTypes.South
                && cell.Humidity < 50)
                delta.ChangeHumidity(5);
        }
        private static void EnforceDesert(WeatherCell cell, WeatherCell delta)
        {
            if ((db.GameTime.Sunlight == SunPositionTypes.Sunset
                || db.GameTime.Sunlight == SunPositionTypes.Dark)
                && cell.Temperature > 30)
                delta.ChangeTemperature(-5);
            else if ((db.GameTime.Sunlight == SunPositionTypes.Sunrise
                || db.GameTime.Sunlight == SunPositionTypes.Light)
                && cell.Temperature < 64)
                delta.ChangeTemperature(2);
            else if (cell.Humidity > 10)
                delta.ChangeHumidity(-2);
            else if (cell.Pressure < 50)
                delta.ChangePressure(2);
        }
        private static void EnforceSteppe(WeatherCell cell, WeatherCell delta)
        {

        }
        private static void EnforceChapparal(WeatherCell cell, WeatherCell delta)
        {

        }
        private static void EnforceGrasslands(WeatherCell cell, WeatherCell delta)
        {

        }
        private static void EnforceDeciduous(WeatherCell cell, WeatherCell delta)
        {

        }
        private static void EnforceTaiga(WeatherCell cell, WeatherCell delta)
        {

        }
        private static void EnforceTundra(WeatherCell cell, WeatherCell delta)
        {

        }
        private static void EnforceAlpine(WeatherCell cell, WeatherCell delta)
        {

        }
        private static void EnforceArctic(WeatherCell cell, WeatherCell delta)
        {

        }
        #endregion

        public void Save()
        {
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(WeatherFile)))
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        WeatherCell cell = GetCellFromMap(x, y);

                        proxy.Write("#CELL		  {0} {1}\n", x, y);
                        proxy.Write("Climate      {0}\n", cell.Climate);
                        proxy.Write("Hemisphere   {0}\n", cell.Hemisphere);
                        proxy.Write("State        {0}\n",
                            String.Format("{0} {1} {2} {3} {4} {5} {6} {7}",
                            cell.CloudCover, cell.Energy, cell.Humidity,
                            cell.Precipitation, cell.Pressure, cell.Temperature,
                            cell.WindSpeedX, cell.WindSpeedY));
                        proxy.Write("End\n\n");
                    }
                }
                proxy.Write("\n#END\n\n");
            }
        }

        public void ApplyDeltaChanges()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    WeatherCell cell = GetCellFromMap(x, y);
                    WeatherCell delta = GetCellFromDelta(x, y);

                    PrecipitationTypes precipType = WeatherCell.GetPrecipitation(cell.Precipitation);
                    if (!PrecipFuncs.ContainsKey(precipType))
                        continue;

                    PrecipFuncs[precipType].Invoke(cell, delta);

                    cell.Temperature = SmaugCS.Common.Check.Range(-30, cell.Temperature + delta.Temperature, 100);
                    cell.Pressure = SmaugCS.Common.Check.Range(0, cell.Pressure + delta.Pressure, 100);
                    cell.CloudCover = SmaugCS.Common.Check.Range(0, cell.CloudCover + delta.CloudCover, 100);
                    cell.Energy = SmaugCS.Common.Check.Range(0, cell.Energy + delta.Energy, 100);
                    cell.Humidity = SmaugCS.Common.Check.Range(0, cell.Humidity + delta.Humidity, 100);
                    cell.Precipitation = SmaugCS.Common.Check.Range(0, cell.Precipitation + delta.Precipitation, 100);
                    cell.WindSpeedX = SmaugCS.Common.Check.Range(-100, cell.WindSpeedX + delta.WindSpeedX, 100);
                    cell.WindSpeedY = SmaugCS.Common.Check.Range(-100, cell.WindSpeedY + delta.WindSpeedY, 100);
                }
            }
        }

        #region Weather Message Functions

        private static void TorrentialWeatherMessage(WeatherCell cell, WeatherCell delta)
        {

        }

        private static void CatsAndDogsWeatherMessage(WeatherCell cell, WeatherCell delta)
        {

        }

        private static void PouringWeatherMessage(WeatherCell cell, WeatherCell delta)
        {

        }

        private static void HeavilyWeatherMessage(WeatherCell cell, WeatherCell delta)
        {

        }

        private static void DownpourWeatherMessage(WeatherCell cell, WeatherCell delta)
        {

        }

        private static void SteadilyWeatherMessage(WeatherCell cell, WeatherCell delta)
        {

        }

        private static void RainingWeatherMessage(WeatherCell cell, WeatherCell delta)
        {

        }

        private static void LightlyWeatherMessage(WeatherCell cell, WeatherCell delta)
        {

        }

        private static void DrizzlingWeatherMessage(WeatherCell cell, WeatherCell delta)
        {

        }

        private static void MistingWeatherMessage(WeatherCell cell, WeatherCell delta)
        {

        }

        private static void NoPrecipWeatherMessage(WeatherCell cell, WeatherCell delta)
        {

        }

        #endregion

        private void PickAndUpdateRandomCell()
        {
            int x = SmaugCS.Common.SmaugRandom.Between(0, Width);
            int y = SmaugCS.Common.SmaugRandom.Between(0, Height);

            WeatherCell cell = GetCellFromMap(x, y);

            int rand = SmaugCS.Common.SmaugRandom.Between(-10, 10);

            switch (SmaugCS.Common.SmaugRandom.RollDice(1, 8))
            {
                case 1:
                    cell.ChangeCloudCover(rand);
                    break;
                case 2:
                    cell.ChangeEnergy(rand);
                    break;
                case 3:
                    cell.ChangeHumidity(rand);
                    break;
                case 4:
                    cell.ChangePrecip(rand);
                    break;
                case 5:
                    cell.ChangePressure(rand);
                    break;
                case 6:
                    cell.ChangeTemperature(rand);
                    break;
                case 7:
                    cell.ChangeWindSpeedX(rand);
                    break;
                case 8:
                    cell.ChangeWindSpeedY(rand);
                    break;
            }
        }

        public void CalculateCellToCellChanges()
        {
            PickAndUpdateRandomCell();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    WeatherCell cell = GetCellFromMap(x, y);
                    WeatherCell delta = GetCellFromDelta(x, y);

                    // Take day/night into account for temperatue changes
                    if (db.GameTime.Sunlight == SunPositionTypes.Sunrise
                        || db.GameTime.Sunlight == SunPositionTypes.Light)
                        delta.ChangeTemperature((SmaugCS.Common.SmaugRandom.Between(-1, 2) + (((cell.CloudCover / 10) > 5) ? -1 : 1)));
                    else
                        delta.ChangeTemperature((SmaugCS.Common.SmaugRandom.Between(-2, 0) + (((cell.CloudCover / 10) > 5) ? 2 : -3)));

                    // Precipitation drops humidity by 5% of precip level
                    if (cell.Precipitation > 40)
                        delta.ChangeHumidity(0 - (cell.Precipitation / 20));
                    else
                        delta.ChangeHumidity(SmaugCS.Common.SmaugRandom.Between(0, 3));

                    // Humidity and pressure can affect precip level
                    int humidityAndPressure = cell.Humidity + cell.Pressure;
                    if ((humidityAndPressure / 2) >= 60)
                        cell.ChangePrecip(cell.Humidity / 10);
                    else if (((humidityAndPressure / 2) < 60) && ((humidityAndPressure / 2) > 40))
                        cell.ChangePrecip(SmaugCS.Common.SmaugRandom.Between(-2, 2));
                    else
                        cell.ChangePrecip(0 - (cell.Humidity / 5));

                    // Humidity and precip can affect cloud cover
                    int humidityAndPrecip = cell.Humidity + cell.Precipitation;
                    if ((humidityAndPrecip / 2) >= 60)
                        delta.ChangeCloudCover(0 - (cell.Humidity / 10));
                    else if (((humidityAndPrecip / 2) < 60) && ((humidityAndPrecip / 2) > 40))
                        delta.ChangeCloudCover(SmaugCS.Common.SmaugRandom.Between(-2, 2));
                    else
                        delta.ChangeCloudCover(cell.Humidity / 5);

                    int totalPressure = cell.Pressure;
                    int numPressureCells = -1;

                    for (int dy = -1; dy <= 1; ++dy)
                    {
                        for (int dx = -1; dx <= 1; ++dx)
                        {
                            int nx = x + dx;
                            int ny = y + dy;

                            if (dx == 0 && dy == 0)
                                continue;

                            if ((nx < 0 || nx >= Width)
                                || (ny < 0 || ny >= Height))
                                continue;

                            WeatherCell neighborCell = GetCellFromMap(nx, ny);
                            WeatherCell neighborDelta = GetCellFromDelta(nx, ny);

                            /*
                                                 *  We'll apply wind changes here
                                                 *  Wind speeds up in a given direction based on pressure

                                                 *  1/4 of the pressure difference applied to wind speed

                                                 *  Wind should move from higher pressure to lower pressure
                                                 *  and some of our pressure difference should go with it!
                                                 *  If we are pressure 60, and they are pressure 40
                                                 *  then with a difference of 20, lets make that a 4 mph
                                                 *  wind increase towards them!
                                                 *  So if they are west neighbor (dx < 0)
                                                 */
                            int pressureDelta = cell.Pressure - neighborCell.Pressure;
                            int windSpeedDleta = pressureDelta / 4;

                            if (dx != 0)
                                delta.ChangeWindSpeedX(windSpeedDleta * dx);
                            if (dy != 0)
                                delta.ChangeWindSpeedY(windSpeedDleta * dy);

                            totalPressure += neighborCell.Pressure;
                            ++numPressureCells;

                            // Temperature and Humidity change IF wind is blowing towards them
                            int temperatureDelta = cell.Temperature + neighborCell.Temperature;
                            temperatureDelta /= 16;

                            int humidityDelta = cell.Humidity - neighborCell.Humidity;
                            humidityDelta /= 16;

                            if ((cell.WindSpeedX < 0 && dx < 0)
                                || (cell.WindSpeedX > 0 && dx > 0)
                                || (cell.WindSpeedY < 0 && dy < 0)
                                || (cell.WindSpeedY > 0 && dy > 0))
                            {
                                neighborDelta.ChangeTemperature(temperatureDelta);
                                neighborDelta.ChangeHumidity(humidityDelta);
                                delta.ChangeTemperature(0 - temperatureDelta);
                                delta.ChangeHumidity(0 - humidityDelta);
                            }

                            delta.ChangeEnergy(SmaugCS.Common.SmaugRandom.Between(-10, 10));
                        }
                    }

                    delta.Pressure = (totalPressure / numPressureCells) - cell.Pressure;

                    if (cell.Precipitation >= 70)
                        delta.ChangePressure(0 - (cell.Pressure / 2));
                    else if (cell.Pressure < 70 && cell.Precipitation > 30)
                        delta.ChangePressure(SmaugCS.Common.SmaugRandom.Between(-5, 5));
                    else
                        delta.ChangePressure(cell.Pressure / 2);
                }
            }
        }

        public void ClearWeatherDeltas()
        {
            Delta.Initialize();
        }

        public void RandomizeCells()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    WeatherCell cell = GetCellFromMap(x, y);

                    WeatherRangeData rangeData = WeatherConstants.WeatherData.FirstOrDefault(d => d.Hemisphere == cell.Hemisphere
                                                                                  && d.Season == db.GameTime.Season
                                                                                  && d.Climate == cell.Climate);
                    if (rangeData == null)
                    {
                        // TODO Error
                        continue;
                    }

                    cell.ChangeTemperature(SmaugCS.Common.SmaugRandom.Between(rangeData.Temperature[0], rangeData.Temperature[1]));
                    cell.ChangePressure(SmaugCS.Common.SmaugRandom.Between(rangeData.Pressure[0], rangeData.Pressure[1]));
                    cell.ChangeCloudCover(SmaugCS.Common.SmaugRandom.Between(rangeData.CloudCover[0], rangeData.CloudCover[1]));
                    cell.ChangeHumidity(SmaugCS.Common.SmaugRandom.Between(rangeData.Humidity[0], rangeData.Humidity[1]));
                    cell.ChangePrecip(SmaugCS.Common.SmaugRandom.Between(rangeData.Precipitation[0], rangeData.Precipitation[1]));
                    cell.ChangeEnergy(SmaugCS.Common.SmaugRandom.Between(rangeData.Energy[0], rangeData.Energy[1]));
                    cell.ChangeWindSpeedX(SmaugCS.Common.SmaugRandom.Between(rangeData.WindSpeedX[0], rangeData.WindSpeedX[1]));
                    cell.ChangeWindSpeedY(SmaugCS.Common.SmaugRandom.Between(rangeData.WindSpeedY[0], rangeData.WindSpeedY[1]));
                }
            }
        }

        public bool Load()
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(WeatherFile)))
            {
                do
                {
                    char letter = Convert.ToChar(proxy.Read());
                    if (letter == '*')
                    {
                        proxy.ReadToEndOfLine();
                        continue;
                    }

                    if (letter != '#')
                    {
                        LogManager.Bug("%s: # not found (%c)", "load_weathermap", letter);
                        return false;
                    }

                    string word = proxy.ReadNextWord();
                    if (word.EqualsIgnoreCase("cell"))
                    {
                        WeatherCell cell = GetCellFromMap(proxy.ReadNumber(), proxy.ReadNumber());
                        cell.Load(proxy);
                    }
                    else if (word.EqualsIgnoreCase("end"))
                        break;
                    else
                        LogManager.Bug("No match for %s", word);

                } while (!proxy.EndOfStream);
            }
            return true;
        }

        public void Update(bool save = false)
        {
            ClearWeatherDeltas();
            CalculateCellToCellChanges();
            EnforceClimateConditions();
            ApplyDeltaChanges();

            if (save)
                Save();
        }
    }
}
