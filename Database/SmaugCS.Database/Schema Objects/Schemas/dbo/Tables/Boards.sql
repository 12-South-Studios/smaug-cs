CREATE TABLE [dbo].[Boards]
(
	[BoardId] INT IDENTITY NOT NULL,
	[BoardTypeId] TINYINT NOT NULL,
    [ReadGroup] VARCHAR(1024) NULL, 
    [PostGroup] VARCHAR(1024) NULL, 
    [ExtraReaders] VARCHAR(1024) NULL, 
	[ExtraRemovers] VARCHAR(1024) NULL,
	[OTakeMessage] VARCHAR(1024) NULL,
	[OPostMessage] VARCHAR(1024) NULL,
	[ORemoveMessage] VARCHAR(1024) NULL,
	[OCopyMessage] VARCHAR(1024) NULL,
	[OListMessage] VARCHAR(1024) NULL,
	[PostMessage] VARCHAR(1024) NULL,
	[OReadMessage] VARCHAR(1024) NULL,
	[MinimumReadLevel] INT NULL,
	[MinimumPostLevel] INT NULL,
	[MinimumRemoveLevel] INT NULL,
	[MaximumPosts] INT NULL,
	[BoardObjectId] BIGINT NULL,

    CONSTRAINT [PK_Boards]
		PRIMARY KEY CLUSTERED ([BoardId] ASC),

	CONSTRAINT [FK_Boards_BoardTypeId]
		FOREIGN KEY ([BoardTypeId]) REFERENCES [BoardTypes]([BoardTypeId])
)
GO

CREATE NONCLUSTERED INDEX [IX_Boards_BoardTypeId] 
	ON [dbo].[Boards] ([BoardTypeId])
GO