using System.Collections.Generic;
using SmaugCS.DAL;
using SmaugCS.Data;
using SmaugCS.Logging;
using System.Data.Common;
using System.Linq;

namespace SmaugCS.Weather;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class WeatherManager : IWeatherManager
{
  public WeatherMap Weather { get; set; }

  private static ILogManager _logManager;
  private static IDbContext _dbContext;

  public WeatherManager(ILogManager logManager, IDbContext dbContext)
  {
    _logManager = logManager;
    _dbContext = dbContext;
  }

  public WeatherCell GetWeather(AreaData area) => Weather.GetCellFromMap(area.WeatherX, area.WeatherY);

  public static bool ExceedsThreshold(int initial, int delta, int threshold)
    => initial < threshold && initial + delta >= threshold;

  public static bool DropsBelowThreshold(int initial, int delta, int threshold)
    => initial >= threshold && initial + delta < threshold;

  public void Initialize(TimeInfoData timeInfo, int height, int width)
  {
    try
    {
      if (_dbContext.Count<DAL.Models.WeatherCell>() == 0) return;

      List<WeatherCell> cells = _dbContext.GetAll<DAL.Models.WeatherCell>().Select(cell => new WeatherCell(cell.Id)
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
      }).ToList();

      Weather = new WeatherMap(timeInfo, width, height, cells);
      _logManager.Boot("Loaded {0} Weather Cells", cells.Count);
    }
    catch (DbException ex)
    {
      _logManager.Error(ex);
      throw;
    }
  }
}