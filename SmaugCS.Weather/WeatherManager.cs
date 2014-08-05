using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Ninject;
using SmallDBConnectivity;
using SmaugCS.Data;
using SmaugCS.Logging;

namespace SmaugCS.Weather
{
    public sealed class WeatherManager : IWeatherManager
    {
        public WeatherMap Weather { get; set; }

        private static ILogManager _logManager;
        private static IKernel _kernel;
        private static ISmallDb _smallDb;
        private static IDbConnection _connection;

        public WeatherManager(ILogManager logManager, IKernel kernel, ISmallDb smallDb, IDbConnection connection)
        {
            _logManager = logManager;
            _kernel = kernel;
            _smallDb = smallDb;
            _connection = connection;
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
                List<WeatherCell> cells = _smallDb.ExecuteQuery(_connection, "cp_GetWeatherCells", TranslateCellData);

                Weather = new WeatherMap(timeInfo, width, height, cells);
                _logManager.Boot("Loaded {0} Weather Cells", cells.Count);
            }
            catch (Exception ex)
            {
                _logManager.Error(ex);
            }
        }

        [ExcludeFromCodeCoverage]
        private static List<WeatherCell> TranslateCellData(IDataReader reader)
        {
            List<WeatherCell> cells = new List<WeatherCell>();
            using (DataTable dt = new DataTable())
            {
                dt.Load(reader);
                cells.AddRange(from DataRow row in dt.Rows select WeatherCell.Translate(row));
            }

            return cells;
        }
    }
}
