PRINT 'Starting dbo.ClimateTypes Syncronization'
GO

DECLARE @Source TABLE (
    ClimateTypeId TINYINT NOT NULL
    , Name VARCHAR(50) NOT NULL);

SET NOCOUNT ON;
INSERT @Source (ClimateTypeId, Name)
VALUES (1, N'Rainforest'),
	   (2, N'Savanna'),
	   (3, N'Desert'),
	   (4, N'Steppe'),
	   (5, N'Chapparal'),
	   (6, N'Grasslands'),
	   (7, N'Deciduous'),
	   (8, N'Taiga'),
	   (9, N'Tundra'),
	   (10, N'Alpine'),
	   (11, N'Arctic'),
	   (12, N'Subarctic'),
	   (13, N'Coastal'),
	   (14, N'Humid'),
	   (15, N'Tropical'),
	   (16, N'Arid');
SET NOCOUNT OFF;

MERGE dbo.ClimateTypes AS t
USING @Source AS s
    ON s.ClimateTypeId = t.ClimateTypeId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (ClimateTypeId, Name)
    VALUES (s.ClimateTypeId, s.Name)
WHEN MATCHED AND (t.Name <> s.Name)
    THEN UPDATE SET t.Name = s.Name
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;

PRINT 'Done Synchronizing dbo.ClimateTypes'
GO
