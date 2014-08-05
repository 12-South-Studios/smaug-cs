CREATE TABLE [dbo].[HemisphereTypes]
(
	[HemisphereTypeId] TINYINT NOT NULL,
	[Name] VARCHAR(50) NOT NULL,

	CONSTRAINT [PK_HemisphereTypes]
		PRIMARY KEY CLUSTERED ([HemisphereTypeId] ASC)
)
