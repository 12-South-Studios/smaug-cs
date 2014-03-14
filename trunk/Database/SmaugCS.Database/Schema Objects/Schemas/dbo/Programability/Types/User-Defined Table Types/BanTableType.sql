/*
-------------------------------------------------------------------------------------
Table Type for inserting into dbo.Bans
-------------------------------------------------------------------------------------
Date		Author			Description
--------	-------			---------------------------------------------------------
03/11/2014	Jason Murdick	Initial Release
-------------------------------------------------------------------------------------
*/
CREATE TYPE [dbo].[BanTableType] AS TABLE(
	[BanId] INT NULL,
	[BanTypeId] TINYINT NOT NULL,
	[Name] VARCHAR(100) NOT NULL,
	[Note] VARCHAR(1024) NULL,
	[BannedBy] VARCHAR(25) NOT NULL,
	[BannedOn] DATETIME NOT NULL,
	[Duration] SMALLINT NOT NULL,
	[Level] TINYINT NULL,
	[Warn] BIT NULL,
	[Prefix] BIT NULL,
	[Suffix] BIT NULL
)
GO

GRANT EXECUTE ON TYPE::[dbo].[BanTableType] TO [Server] AS dbo;
GO