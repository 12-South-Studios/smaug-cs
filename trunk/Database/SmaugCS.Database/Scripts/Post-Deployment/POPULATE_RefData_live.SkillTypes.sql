PRINT 'Starting dbo.SkillTypes Syncronization'
GO

DECLARE @Source TABLE (
    SkillTypeId TINYINT NOT NULL
    , Name VARCHAR(50) NOT NULL);

SET NOCOUNT ON;
INSERT @Source (SkillTypeId, Name)
VALUES (1, N'Skill'),
	   (2, N'Ability'),
	   (3, N'Spell'),
	   (4, N'Weapon'),
	   (5, N'Tongue');
SET NOCOUNT OFF;

MERGE dbo.SkillTypes AS t
USING @Source AS s
    ON s.SkillTypeId = t.SkillTypeId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (SkillTypeId, Name)
    VALUES (s.SkillTypeId, s.Name)
WHEN MATCHED AND (t.Name <> s.Name)
    THEN UPDATE SET t.Name = s.Name
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;

PRINT 'Done Synchronizing dbo.SkillTypes'
GO
