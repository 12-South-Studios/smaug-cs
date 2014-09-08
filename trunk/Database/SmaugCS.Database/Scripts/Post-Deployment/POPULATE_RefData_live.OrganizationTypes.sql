PRINT 'Starting dbo.OrganizationTypes Syncronization'
GO

DECLARE @Source TABLE (
    OrganizationTypeId TINYINT NOT NULL
    , Name VARCHAR(50) NOT NULL);

SET NOCOUNT ON;
INSERT @Source (OrganizationTypeId, Name)
VALUES (1, N'Clan'),
	   (2, N'Council');
SET NOCOUNT OFF;

MERGE dbo.OrganizationTypes AS t
USING @Source AS s
    ON s.OrganizationTypeId = t.OrganizationTypeId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (OrganizationTypeId, Name)
    VALUES (s.OrganizationTypeId, s.Name)
WHEN MATCHED AND (t.Name <> s.Name)
    THEN UPDATE SET t.Name = s.Name
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;

PRINT 'Done Synchronizing dbo.OrganizationTypes'
GO
