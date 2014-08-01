﻿CREATE TABLE [dbo].[BoardTypes]
(
	[BoardTypeId] TINYINT NOT NULL,
	[Name] VARCHAR(50) NOT NULL

	CONSTRAINT [PK_BoardTypes]
		PRIMARY KEY CLUSTERED ([BoardTypeId] ASC)
)
