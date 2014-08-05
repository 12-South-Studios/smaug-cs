PRINT 'Starting dbo.LogTypes Syncronization'
GO

DECLARE @Source TABLE (
    LogTypeId TINYINT NOT NULL
    , Name VARCHAR(50) NOT NULL);

SET NOCOUNT ON;
INSERT @Source (LogTypeId, Name)
VALUES (1, N'Info'),
	   (2, N'Error'),
	   (3, N'Bug'),
	   (4, N'Debug'),
	   (5, N'Fatal');
SET NOCOUNT OFF;

MERGE dbo.LogTypes AS t
USING @Source AS s
    ON s.LogTypeId = t.LogTypeId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (LogTypeId, Name)
    VALUES (s.LogTypeId, s.Name)
WHEN MATCHED AND (t.Name <> s.Name)
    THEN UPDATE SET t.Name = s.Name
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;

PRINT 'Done Synchronizing dbo.LogTypes'
GO
