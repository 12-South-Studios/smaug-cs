/*
--------------------------------------------------------------------------
Adds a new log entry
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
2014/08/01	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [dbo].[cp_AddLog]
	@tvpLogTable LogTableType READONLY
AS 
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.[Logs]
	SELECT [LogTypeId],
		   GetDate(),
		   [Text]
	FROM @tvpLogTable;

END

GO
GRANT EXECUTE ON [dbo].[cp_AddLog] TO [Server] AS dbo;