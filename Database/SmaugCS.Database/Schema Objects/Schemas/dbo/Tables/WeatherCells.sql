CREATE TABLE [dbo].[WeatherCells]
(
	[WeatherCellId] INT IDENTITY NOT NULL,
	[CellXCoord] SMALLINT NOT NULL,
    [CellYCoord] SMALLINT NOT NULL, 
    [HemisphereTypeId] TINYINT NOT NULL,
	[ClimateTypeId] TINYINT NOT NULL,
	[CloudCover] INT NOT NULL,
	[Energy] INT NOT NULL,
	[Humidity] INT NOT NULL,
	[Precipitation] INT NOT NULL,
	[Pressure] INT NOT NULL,
	[Temperature] INT NOT NULL,
	[WindSpeedX] INT NOT NULL,
	[WindSpeedY] INT NOT NULL,

    CONSTRAINT [PK_WeatherCells]
		PRIMARY KEY CLUSTERED ([WeatherCellId] ASC),

	CONSTRAINT [FK_WeatherCells_HemisphereTypeId]
		FOREIGN KEY ([HemisphereTypeId]) REFERENCES [HemisphereTypes]([HemisphereTypeId]),

	CONSTRAINT [FK_WeatherCells_ClimateTypeId]
		FOREIGN KEY ([ClimateTypeId]) REFERENCES [ClimateTypes]([ClimateTypeId])
)
GO

CREATE NONCLUSTERED INDEX [IX_WeatherCells_HemisphereTypeId] 
	ON [dbo].[WeatherCells] ([HemisphereTypeId])
GO

CREATE NONCLUSTERED INDEX [IX_WeatherCells_ClimateTypeId] 
	ON [dbo].[WeatherCells] ([ClimateTypeId])
GO
