CREATE TABLE [live].[CharacterAffects]
(
	[CharacterAffectsId] INT IDENTITY NOT NULL,
	[CharacterId] BIGINT NOT NULL,
	[AffectType] VARCHAR(50) NOT NULL,
	[Duration] SMALLINT NOT NULL,
	[Modifier] SMALLINT NOT NULL,
	[Location] SMALLINT NULL,
	[Flags] INT NULL,

    CONSTRAINT [PK_CharacterAffects]
		PRIMARY KEY CLUSTERED ([CharacterAffectsId] ASC),

	CONSTRAINT [FK_CharacterAffects_CharacterId]
		FOREIGN KEY ([CharacterId]) REFERENCES [live].[Characters]([CharacterId])
)
GO

CREATE NONCLUSTERED INDEX [IX_CharacterAffects_CharacterId] 
	ON [live].[CharacterAffects] ([CharacterId])
GO
