CREATE TABLE [live].[CharacterIgnored]
(
	[CharacterIgnoredId] INT IDENTITY NOT NULL,
	[CharacterId] BIGINT NOT NULL,
	[IgnoredName] VARCHAR(50) NOT NULL,
	[AddedOn] DATETIME NULL,

    CONSTRAINT [PK_CharacterIgnored]
		PRIMARY KEY CLUSTERED ([CharacterIgnoredId] ASC),

	CONSTRAINT [FK_CharacterIgnored_CharacterId]
		FOREIGN KEY ([CharacterId]) REFERENCES [live].[Characters]([CharacterId])
)
GO

CREATE NONCLUSTERED INDEX [IX_CharacterIgnored_CharacterId] 
	ON [live].[CharacterIgnored] ([CharacterId])
GO
