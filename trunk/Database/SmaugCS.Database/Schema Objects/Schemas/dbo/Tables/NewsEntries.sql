CREATE TABLE [dbo].[NewsEntries]
(
	[NewsEntryId] INT IDENTITY NOT NULL,
	[NewsId] INT NOT NULL,
	[Title] VARCHAR(100) NULL,
	[Name] VARCHAR(100) NULL,
	[Text] VARCHAR(MAX) NOT NULL,
	[PostedOn] DATETIME NOT NULL,
	[PostedBy] VARCHAR(50) NULL,
	[Active] BIT NOT NULL DEFAULT 1, 

    CONSTRAINT [PK_NewsEntries]
		PRIMARY KEY CLUSTERED ([NewsEntryId] ASC),

	CONSTRAINT [FK_NewsEntries_NewsId]
		FOREIGN KEY ([NewsId]) REFERENCES [News]([NewsId])
)
GO

CREATE NONCLUSTERED INDEX [IX_NewsEntries_NewsId] 
	ON [dbo].[NewsEntries] ([NewsId])
GO
