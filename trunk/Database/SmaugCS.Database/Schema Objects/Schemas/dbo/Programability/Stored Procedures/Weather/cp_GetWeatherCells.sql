/*
--------------------------------------------------------------------------
Gets the list of weather cells
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
2014/08/04	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [dbo].[cp_GetWeatherCells]
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT 
		w.WeatherCellId,
		w.CellXCoord,
		w.CellYCoord,
		ht.Name AS HemisphereName,
		ct.Name AS ClimateName,
		w.CloudCover,
		w.Energy,
		w.Humidity,
		w.Precipitation,
		w.Pressure,
		w.Temperature,
		w.WindSpeedX,
		w.WindSpeedY 
	FROM dbo.WeatherCells w
	JOIN dbo.HemisphereTypes ht ON w.HemisphereTypeId = ht.HemisphereTypeId 
	JOIN dbo.ClimateTypes ct ON w.ClimateTypeId = ct.ClimateTypeId
	ORDER BY w.CellXCoord, w.CellYCoord;

END

GO
GRANT EXECUTE ON [dbo].[cp_GetWeatherCells] TO [Server] AS dbo;