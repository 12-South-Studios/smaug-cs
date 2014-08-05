PRINT 'Starting live.GameState Syncronization'
GO

DECLARE @Source TABLE (
	GameYear SMALLINT NOT NULL,
	GameMonth SMALLINT NOT NULL,
	GameDay SMALLINT NOT NULL,
	GameHour SMALLINT NOT NULL);

SET NOCOUNT ON;
INSERT @Source (GameYear, GameMonth, GameDay, GameHour)
VALUES (628, 6, 28, 0);

SET NOCOUNT OFF;

MERGE live.GameState AS t USING @Source AS s 
	ON s.GameYear = t.GameYear
	AND s.GameMonth = t.GameMonth
	AND s.GameDay = t.GameDay
	AND s.GameHour = t.GameHour
WHEN NOT MATCHED BY TARGET THEN
    INSERT (GameYear, GameMonth, GameDay, GameHour)
    VALUES (s.GameYear, s.GameMonth, s.GameDay, s.GameHour)
WHEN MATCHED AND (
	t.GameYear <> s.GameYear 
	OR t.GameMonth <> s.GameMonth
	OR t.GameDay <> s.GameDay
	OR t.GameHour <> s.GameHour)
    THEN UPDATE SET 
		t.GameYear = s.GameYear,
		t.GameMonth = s.GameMonth,
		t.GameDay = s.GameDay,
		t.GameHour = s.GameHour;

PRINT 'Done Synchronizing live.GameState'
GO
