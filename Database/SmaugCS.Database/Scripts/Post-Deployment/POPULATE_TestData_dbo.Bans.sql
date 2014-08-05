PRINT 'Starting dbo.Bans Syncronization'
GO

DECLARE @Source TABLE (
	BanTypeId TINYINT NOT NULL,
    Name VARCHAR(100) NOT NULL,
	Note VARCHAR(1024) NOT NULL,
	BannedBy VARCHAR(25) NOT NULL,
	BannedOn DATETIME NOT NULL,
	Duration SMALLINT NOT NULL,
	[Level] TINYINT NULL,
	Warn BIT NULL,
	Prefix BIT NULL,
	Suffix BIT NULL,
	Active BIT NOT NULL);

SET NOCOUNT ON;
INSERT @Source (BanTypeId, Name, Note, BannedBy, BannedOn, Duration, [Level], Warn, Prefix, Suffix, Active)
VALUES
	(1, N'192.168.', N'Test Ban of Localhost', 'Gwareth', '2014-08-03 06:04:00', -1, 0, 0, 1, 0, 1),
	(2, N'Warrior', N'Test Ban of Warrior Class', 'Gwareth', '2014-08-03 06:04:00', -1, 0, 0, 0, 0, 1),
	(3, N'Elf', N'Test Ban of Elf Class', 'Gwareth', '2014-08-03 06:04:00', -1, 0, 0, 0, 0, 1),
	(4, N'John Doe', N'Test Ban of John Doe', 'Gwareth', '2014-08-03 06:04:00', -1, 0, 1, 0, 0, 1);

SET NOCOUNT OFF;

MERGE dbo.Bans AS t USING @Source AS s ON s.Name = t.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT (BanTypeId, Name, Note, BannedBy, BannedOn, Duration, [Level], Warn, Prefix, Suffix, Active)
    VALUES (s.BanTypeId, s.Name, s.Note, s.BannedBy, s.BannedOn, s.Duration, s.[Level], s.Warn, s.Prefix, s.Suffix, s.Active)
WHEN MATCHED AND (
	t.BanTypeID <> s.BanTypeID 
	OR t.Note <> s.Note
	OR t.BannedBy <> s.BannedBy
	OR t.BannedOn <> s.BannedOn
	OR t.Duration <> s.Duration
	OR t.[Level] <> s.[Level]
	OR t.Warn <> s.Warn
	OR t.Prefix <> s.Prefix
	OR t.Suffix <> s.Suffix
	OR t.Active <> s.Active)
    THEN UPDATE SET 
		t.BanTypeId = s.BanTypeId,
		t.Name = s.Name,
		t.Note = s.Note,
		t.BannedBy = s.BannedBy,
		t.BannedOn = s.BannedOn,
		t.Duration = s.Duration,
		t.[Level] = s.[Level],
		t.Warn = s.Warn,
		t.Prefix = s.Prefix,
		t.Suffix = s.Suffix,
		t.Active = s.Active;

PRINT 'Done Synchronizing dbo.Bans'
GO
