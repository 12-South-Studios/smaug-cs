PRINT 'Starting dbo.StatisticTypes Syncronization'
GO

DECLARE @Source TABLE (
    StatisticTypeId TINYINT NOT NULL
    , Name VARCHAR(50) NOT NULL);

SET NOCOUNT ON;
INSERT @Source (StatisticTypeId, Name)
VALUES (1, N'Current Health'),
	   (2, N'Maximum Health'),
	   (3, N'Current Mana'),
	   (4, N'Maximum Mana'),
	   (5, N'Current Movement'),
	   (6, N'Maximum Movement'),
	   (7, N'Coin'),
	   (8, N'Experience'),
	   (9, N'Height'),
	   (10, N'Weight'),
	   (11, N'Position'),
	   (12, N'Style'),
	   (13, N'Practice'),
	   (14, N'Save Vs Poison-Death'),
	   (15, N'Save Vs Wand-Rod'),
	   (16, N'Save Vs Paralysis-Petrify'),
	   (17, N'Save Vs Breath'),
	   (18, N'Save Vs Spell-Staff'),
	   (19, N'Alignment'),
	   (20, N'Favor'),
	   (21, N'Hitroll'),
	   (22, N'Damroll'),
	   (23, N'Armor Class'),
	   (24, N'Wimpy'),
	   (25, N'Deaf'),
	   (26, N'Resistance'),
	   (27, N'NoResistance'),
	   (28, N'Immunity'),
	   (29, N'NoImmunity'),
	   (30, N'Susceptibility'),
	   (31, N'NoSusceptibility'),
	   (32, N'MentalState'),
	   (33, N'Rank'),
	   (34, N'Bestowments'),
	   (35, N'Prompt'),
	   (36, N'ActFlags'),
	   (37, N'AffectedByFlags'),
	   (38, N'NoAffectedByFlags'),
	   (39, N'Flags'),
	   (40, N'Title'),
	   (41, N'Homepage'),
	   (42, N'Biography'),
	   (43, N'PermanentStrength'),
	   (44, N'PermanentIntelligence'),
	   (45, N'PermanentWisdom'),
	   (46, N'PermanentCharisma'),
	   (47, N'PermanentDexterity'),
	   (48, N'PermanentConstitution'),
	   (49, N'PermanentLuck'),
	   (50, N'ModifiedStrength'),
	   (51, N'ModifiedIntelligence'),
	   (52, N'ModifiedWisdom'),
	   (53, N'ModifiedCharisma'),
	   (54, N'ModifiedDexterity'),
	   (55, N'ModifiedConstitution'),
	   (56, N'ModifiedLuck'),
	   (57, N'Thirst'),
	   (58, N'Full'),
	   (59, N'Drunkeness'),
	   (60, N'Bloodthirst');
SET NOCOUNT OFF;

MERGE dbo.StatisticTypes AS t
USING @Source AS s
    ON s.StatisticTypeId = t.StatisticTypeId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (StatisticTypeId, Name)
    VALUES (s.StatisticTypeId, s.Name)
WHEN MATCHED AND (t.Name <> s.Name)
    THEN UPDATE SET t.Name = s.Name
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;

PRINT 'Done Synchronizing dbo.StatisticTypes'
GO
