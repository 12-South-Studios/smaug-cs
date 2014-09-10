CREATE TABLE [live].[StatisticTypes]
(
	[StatisticTypeId] TINYINT NOT NULL,
	[Name] VARCHAR(50) NOT NULL

	CONSTRAINT [PK_StatisticTypes]
		PRIMARY KEY CLUSTERED ([StatisticTypeId] ASC)
)
