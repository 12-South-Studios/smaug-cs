PRINT 'Starting dbo.HemisphereTypes Syncronization'
GO

DECLARE @Source TABLE (
    HemisphereTypeId TINYINT NOT NULL
    , Name VARCHAR(50) NOT NULL);

SET NOCOUNT ON;
INSERT @Source (HemisphereTypeId, Name)
VALUES (1, N'North'),
	   (2, N'South');
SET NOCOUNT OFF;

MERGE dbo.HemisphereTypes AS t
USING @Source AS s
    ON s.HemisphereTypeId = t.HemisphereTypeId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (HemisphereTypeId, Name)
    VALUES (s.HemisphereTypeId, s.Name)
WHEN MATCHED AND (t.Name <> s.Name)
    THEN UPDATE SET t.Name = s.Name
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;

PRINT 'Done Synchronizing dbo.HemisphereTypes'
GO
