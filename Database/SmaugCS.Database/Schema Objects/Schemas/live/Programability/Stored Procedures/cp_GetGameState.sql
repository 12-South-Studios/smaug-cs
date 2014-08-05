/*
--------------------------------------------------------------------------
Gets the current game state
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
2014/08/04	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [live].[cp_GetGameState]
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT TOP(1) 
		g.GameYear,
		g.GameMonth,
		g.GameDay,
		g.GameHour 
	FROM live.GameState g
	ORDER BY g.GameStateID DESC;
		
END

GO
GRANT EXECUTE ON [live].[cp_GetGameState] TO [Server] AS dbo;