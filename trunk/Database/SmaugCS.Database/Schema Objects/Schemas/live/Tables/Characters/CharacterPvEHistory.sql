CREATE TABLE [live].[CharacterPvEHistory]
(
	[CharacterPvEHistoryId] INT IDENTITY NOT NULL,
	[CharacterId] BIGINT NOT NULL,
	[MonsterId] BIGINT NOT NULL,
	[TimesKilled] INT NOT NULL,

    CONSTRAINT [PK_CharacterPvEHistory]
		PRIMARY KEY CLUSTERED ([CharacterPvEHistoryId] ASC),

	CONSTRAINT [FK_CharacterPvEHistory_CharacterId]
		FOREIGN KEY ([CharacterId]) REFERENCES [live].[Characters]([CharacterId])
)
GO

CREATE NONCLUSTERED INDEX [IX_CharacterPvEHistory_CharacterId] 
	ON [live].[CharacterPvEHistory] ([CharacterId])
GO
