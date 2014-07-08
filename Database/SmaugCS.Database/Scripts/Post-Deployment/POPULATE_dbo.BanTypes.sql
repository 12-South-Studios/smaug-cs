PRINT 'Starting dbo.BanTypes Syncronization'
GO

DECLARE @Source TABLE (
    BanTypeId TINYINT NOT NULL
    , Name VARCHAR(50) NOT NULL
	, Value TINYINT NOT NULL);

SET NOCOUNT ON;
INSERT @Source (BanTypeId, Name, Value)
VALUES (1, N'Site', 1),
	   (2, N'Class', 2),
	   (3, N'Race', 4),
	   (4, N'Warn', 8);
SET NOCOUNT OFF;

MERGE dbo.BanTypes AS t
USING @Source AS s
    ON s.BanTypeId = t.BanTypeId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (BanTypeId, Name, Value)
    VALUES (s.BanTypeId, s.Name, s.Value)
WHEN MATCHED AND (t.Name <> s.Name OR t.Value <> s.Value)
    THEN UPDATE SET t.Name = s.Name
		, t.Value = s.Value
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;

PRINT 'Done Synchronizing dbo.BanTypes'
GO
