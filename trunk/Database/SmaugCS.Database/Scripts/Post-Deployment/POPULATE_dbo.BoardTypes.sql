PRINT 'Starting dbo.BoardTypes Syncronization'
GO

DECLARE @Source TABLE (
    BoardTypeId TINYINT NOT NULL
    , Name VARCHAR(50) NOT NULL);

SET NOCOUNT ON;
INSERT @Source (BoardTypeId, Name)
VALUES (1, N'Note'),
	   (2, N'Mail');
SET NOCOUNT OFF;

MERGE dbo.BoardTypes AS t
USING @Source AS s
    ON s.BoardTypeId = t.BoardTypeId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (BoardTypeId, Name)
    VALUES (s.BoardTypeId, s.Name)
WHEN MATCHED AND (t.Name <> s.Name)
    THEN UPDATE SET t.Name = s.Name
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;

PRINT 'Done Synchronizing dbo.BoardTypes'
GO
