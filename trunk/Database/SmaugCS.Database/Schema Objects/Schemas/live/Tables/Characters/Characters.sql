CREATE TABLE [live].[Characters]
(
	[CharacterId] BIGINT IDENTITY(1, 1) NOT NULL, 
	[Name] VARCHAR(50) NOT NULL,
	[Password] NVARCHAR(255) NOT NULL,
	[Description] VARCHAR(1024) NULL,
	[Gender] TINYINT NOT NULL,
	[Class] TINYINT NOT NULL,
	[Race] TINYINT NOT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL,
	[CharacterAge] INT NULL,
	[Level] SMALLINT NOT NULL,
	[RoomId] BIGINT NOT NULL,
	[AuthedBy] VARCHAR(50) NULL,
	[CouncilId] BIGINT NULL,
	[DeityId] SMALLINT NULL,
	[ClanId] BIGINT NULL,

	CONSTRAINT pk_Characters
		PRIMARY KEY CLUSTERED ([CharacterId])
);
GO
