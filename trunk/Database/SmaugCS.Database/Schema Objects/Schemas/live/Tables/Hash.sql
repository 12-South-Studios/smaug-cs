﻿CREATE TABLE [live].[Hash]
(
	[HashID] SMALLINT NOT NULL, 
    [Value] NVARCHAR(8) NOT NULL
	CONSTRAINT pk_Hash
		PRIMARY KEY CLUSTERED ([HashID] ASC)  
);
GO
