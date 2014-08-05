CREATE TABLE [dbo].[ClimateTypes]
(
	[ClimateTypeId] TINYINT NOT NULL,
	[Name] VARCHAR(50) NOT NULL,

	CONSTRAINT [PK_ClimateTypes]
		PRIMARY KEY CLUSTERED ([ClimateTypeId] ASC)
)
