CREATE TABLE [live].[CharacterStatistics]
(
	[CharacterStatisticsId] INT IDENTITY NOT NULL,
	[CharacterId] BIGINT NOT NULL,
	[StatisticTypeId] TINYINT NOT NULL,
	[IntValue] BIGINT NULL,
	[StringValue] VARCHAR(MAX) NULL,

    CONSTRAINT [PK_CharacterStatistics]
		PRIMARY KEY CLUSTERED ([CharacterStatisticsId] ASC),

	CONSTRAINT [FK_CharacterStatistics_CharacterId]
		FOREIGN KEY ([CharacterId]) REFERENCES [live].[Characters]([CharacterId]),

	CONSTRAINT [FK_CharacterStatistics_StatisticTypeId]
		FOREIGN KEY ([StatisticTypeId]) REFERENCES [live].[StatisticTypes]([StatisticTypeId])
)
GO

CREATE NONCLUSTERED INDEX [IX_CharacterStatistics_CharacterId] 
	ON [live].[CharacterStatistics] ([CharacterId])
GO

CREATE NONCLUSTERED INDEX [IX_CharacterStatistics_StatisticTypeId] 
	ON [live].[CharacterStatistics] ([StatisticTypeId])
GO
