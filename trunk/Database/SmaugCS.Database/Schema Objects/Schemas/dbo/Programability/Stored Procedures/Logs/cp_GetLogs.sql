/*
--------------------------------------------------------------------------
Gets the list of Logs, alternatively by type
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
2014/08/01	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [dbo].[cp_GetLogs]
	@logType TINYINT = NULL,
	@fromDate DATETIME = NULL,
	@toDate DATETIME = NULL
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT 
		l.LogId,
		lt.[Name],
		l.LoggedOn,
		l.[Text]
	FROM [dbo].[Logs] l
	JOIN [dbo].[LogTypes] lt ON l.LogTypeId = lt.LogTypeId
	WHERE ((@logType IS NULL) OR (l.logTypeId = @logType)) 
		AND ((@fromDate IS NULL) OR (l.LoggedOn >= @fromDate)) 
		AND ((@toDate IS NULL) OR (l.LoggedOn <= @toDate + 1));
		
END

GO
GRANT EXECUTE ON [dbo].[cp_GetLogs] TO [Server] AS dbo;