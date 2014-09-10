CREATE TABLE [live].[CharacterImmortals]
(
	[CharacterImmortalsId] INT IDENTITY NOT NULL,
	[CharacterId] BIGINT NOT NULL,
	[BamfInMessage] VARCHAR(1024) NULL,
	[BamfOutMessage] VARCHAR(1024) NULL,
	[Trust] INT NULL,
	[WizInvis] INT NULL,
	[ImmortalRank] VARCHAR(100) NOT NULL,

    CONSTRAINT [PK_CharacterImmortals]
		PRIMARY KEY CLUSTERED ([CharacterImmortalsId] ASC),

	CONSTRAINT [FK_CharacterImmortals_CharacterId]
		FOREIGN KEY ([CharacterId]) REFERENCES [live].[Characters]([CharacterId])
)
GO

CREATE NONCLUSTERED INDEX [IX_CharacterImmortals_CharacterId] 
	ON [live].[CharacterImmortals] ([CharacterId])
GO
