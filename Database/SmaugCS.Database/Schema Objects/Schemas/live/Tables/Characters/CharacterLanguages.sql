CREATE TABLE [live].[CharacterLanguages]
(
	[CharacterLanguagesId] INT IDENTITY NOT NULL,
	[CharacterId] BIGINT NOT NULL,
	[LanguageName] VARCHAR(50) NOT NULL,

    CONSTRAINT [PK_CharacterLanguages]
		PRIMARY KEY CLUSTERED ([CharacterLanguagesId] ASC),

	CONSTRAINT [FK_CharacterLanguages_CharacterId]
		FOREIGN KEY ([CharacterId]) REFERENCES [live].[Characters]([CharacterId])
)
GO

CREATE NONCLUSTERED INDEX [IX_CharacterLanguages_CharacterId] 
	ON [live].[CharacterLanguages] ([CharacterId])
GO
