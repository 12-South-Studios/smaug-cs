CREATE TABLE [live].[CharacterPets]
(
	[CharacterPetsId] INT IDENTITY NOT NULL,
	[CharacterId] BIGINT NOT NULL,
	[MonsterId] BIGINT NOT NULL,
	[RoomId] BIGINT NULL,
	[OverrideName] VARCHAR(50) NULL,
	[OverrideShortDescription] VARCHAR(1024) NULL,
	[OverrideLongDescription] VARCHAR(1024) NULL,
	[Position] TINYINT NOT NULL,
	[Flags] INT NOT NULL,

    CONSTRAINT [PK_CharacterPets]
		PRIMARY KEY CLUSTERED ([CharacterPetsId] ASC),

	CONSTRAINT [FK_CharacterPets_CharacterId]
		FOREIGN KEY ([CharacterId]) REFERENCES [live].[Characters]([CharacterId])
)
GO

CREATE NONCLUSTERED INDEX [IX_CharacterPets_CharacterId] 
	ON [live].[CharacterPets] ([CharacterId])
GO
