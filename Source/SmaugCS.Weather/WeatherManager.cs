﻿using System.Collections.Generic;
using System.Data.Common;
using Ninject;
using SmaugCS.Data;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Logging;

namespace SmaugCS.Weather
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class WeatherManager : IWeatherManager
    {
        public WeatherMap Weather { get; set; }

        private static ILogManager _logManager;
        private static IKernel _kernel;
        private static ISmaugDbContext _dbContext;

        public WeatherManager(ILogManager logManager, IKernel kernel, ISmaugDbContext dbContext)
        {
            _logManager = logManager;
            _kernel = kernel;
            _dbContext = dbContext;
        }

        public static IWeatherManager Instance
        {
            get { return _kernel.Get<IWeatherManager>(); }
        }

        public WeatherCell GetWeather(AreaData area)
        {
            return Weather.GetCellFromMap(area.WeatherX, area.WeatherY);
        }

        public static bool ExceedsThreshold(int initial, int delta, int threshold)
        {
            return ((initial < threshold) && (initial + delta >= threshold));
        }

        public static bool DropsBelowThreshold(int initial, int delta, int threshold)
        {
            return ((initial >= threshold) && (initial + delta < threshold));
        }

        public void Initialize(TimeInfoData timeInfo, int height, int width)
        {
            try
            {
                var cells = new List<WeatherCell>();
                foreach (var cell in _dbContext.Weather)
                {
                    var newCell = new WeatherCell(cell.Id)
                    {
                        XCoord = cell.CellXCoordinate,
                        YCoord = cell.CellYCoordinate,
                        Climate = cell.ClimateType,
                        Hemisphere = cell.HemisphereType,
                        CloudCover = cell.CloudCover,
                        Energy = cell.Energy,
                        Humidity = cell.Humidity,
                        Precipitation = cell.Precipitation,
                        Pressure = cell.Pressure,
                        Temperature = cell.Temperature,
                        WindSpeedX = cell.WindSpeedX,
                        WindSpeedY = cell.WindSpeedY
                    };

                    cells.Add(newCell);
                }

                Weather = new WeatherMap(timeInfo, width, height, cells);
                _logManager.Boot("Loaded {0} Weather Cells", cells.Count);
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
            }
        }
    }
}