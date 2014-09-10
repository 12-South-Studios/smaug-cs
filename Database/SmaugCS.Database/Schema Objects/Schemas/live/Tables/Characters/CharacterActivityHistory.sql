CREATE TABLE [live].[CharacterActivityHistory]
(
	[CharacterActivityHistoryId] INT IDENTITY NOT NULL,
	[CharacterId] BIGINT NOT NULL,
	[PvPKills] INT NULL,
	[PvPDeaths] INT NULL,
	[PvPTimer] SMALLINT NULL,
	[PvEKills] INT NULL,
	[PvEDeaths] INT NULL,
	[IllegalPvP] INT NULL,
	[PlayedTime] BIGINT NULL,
	[IdleTime] BIGINT NULL,
	[AuctionBidsPlaced] SMALLINT NULL,
	[AuctionsWon] SMALLINT NULL,
	[AuctionsStarted] SMALLINT NULL,
	[CoinEarned] BIGINT NULL,

    CONSTRAINT [PK_CharacterActivityHistory]
		PRIMARY KEY CLUSTERED ([CharacterActivityHistoryId] ASC),

	CONSTRAINT [FK_CharacterActivityHistory_CharacterId]
		FOREIGN KEY ([CharacterId]) REFERENCES [live].[Characters]([CharacterId])
)
GO

CREATE NONCLUSTERED INDEX [IX_CharacterActivityHistory_CharacterId] 
	ON [live].[CharacterActivityHistory] ([CharacterId])
GO
