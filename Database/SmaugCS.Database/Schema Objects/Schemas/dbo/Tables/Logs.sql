﻿CREATE TABLE [dbo].[Logs]
(
	[LogId] INT IDENTITY NOT NULL,
	[LogTypeId] TINYINT NOT NULL,
    [LoggedOn] DATETIME NOT NULL, 
    [Text] VARCHAR(MAX) NOT NULL,

    CONSTRAINT [PK_Logs]
		PRIMARY KEY CLUSTERED ([LogId] ASC),

	CONSTRAINT [FK_Logs_LogTypeId]
		FOREIGN KEY ([LogTypeId]) REFERENCES [LogTypes]([LogTypeId])
)
GO

CREATE NONCLUSTERED INDEX [IX_Logs_LogTypeId] 
	ON [dbo].[Logs] ([LogTypeId])
GO