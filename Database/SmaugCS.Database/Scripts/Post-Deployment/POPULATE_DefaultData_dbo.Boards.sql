PRINT 'Starting dbo.Boards Syncronization'
GO

DECLARE @Source TABLE (
	BoardTypeId TINYINT NOT NULL,
	Name VARCHAR(100) NOT NULL,
    ReadGroup VARCHAR(1024) NULL,
	PostGroup VARCHAR(1024) NULL,
	ExtraReaders VARCHAR(1024) NULL,
	ExtraRemovers VARCHAR(1024) NULL,
	OTakeMessage VARCHAR(1024) NULL,
	OPostMessage VARCHAR(1024) NULL,
	ORemoveMessage VARCHAR(1024) NULL,
	OCopyMessage VARCHAR(1024) NULL,
	OListMessage VARCHAR(1024) NULL,
	PostMessage VARCHAR(1024) NULL,
	OReadMessage VARCHAR(1024) NULL,
	MinimumReadLevel INT NULL,
	MinimumPostLevel INT NULL,
	MinimumRemoveLevel INT NULL,
	MaximumPosts INT NULL,
	BoardObjectId BIGINT NULL);

	-- Types: 1=Note, 2=Mail

SET NOCOUNT ON;
INSERT @Source (BoardTypeId, Name, ReadGroup, PostGroup, ExtraReaders, ExtraRemovers, OTakeMessage, OPostMessage, 
	ORemoveMessage, OCopyMessage, OListMessage, PostMessage, OReadMessage, MinimumReadLevel, MinimumPostLevel,
	MinimumRemoveLevel, MaximumPosts, BoardObjectId)
VALUES
	(1, N'Public', N'All', N'All', N'', N'', N'', N'', N'', N'', N'', N'You posted to the public board.', N'', 0, 0, 100, -1, 0),
	(1, N'Immortals', N'Immortals', N'Immortals', N'', N'', N'', N'', N'', N'', N'', N'You posted to the immortals board.', N'', 100, 100, 100, -1, 0);

SET NOCOUNT OFF;

MERGE dbo.Boards AS t USING @Source AS s ON s.Name = t.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT (BoardTypeId, Name, ReadGroup, PostGroup, ExtraReaders, ExtraRemovers, OTakeMessage, OPostMessage, 
	ORemoveMessage, OCopyMessage, OListMessage, PostMessage, OReadMessage, MinimumReadLevel, MinimumPostLevel,
	MinimumRemoveLevel, MaximumPosts, BoardObjectId)
    VALUES (s.BoardTypeId, s.Name, s.ReadGroup, s.PostGroup, s.ExtraReaders, s.ExtraRemovers, s.OTakeMessage, 
	s.OPostMessage, s.ORemoveMessage, s.OCopyMessage, s.OListMessage, s.PostMessage, s.OReadMessage, 
	s.MinimumReadLevel, s.MinimumPostLevel, s.MinimumRemoveLevel, s.MaximumPosts, s.BoardObjectId)
WHEN MATCHED AND (
	t.BoardTypeID <> s.BoardTypeID 
	OR t.Name <> s.Name 
	OR t.ReadGroup <> s.ReadGroup 
	OR t.PostGroup <> s.PostGroup 
	OR t.ExtraReaders <> s.ExtraReaders 
	OR t.ExtraRemovers <> s.ExtraRemovers 
	OR t.OTakeMessage <> s.OTakeMessage 
	OR t.OPostMessage <> s.OPostMessage 
	OR t.ORemoveMessage <> s.ORemoveMessage 
	OR t.OCopyMessage <> s.OCopyMessage 
	OR t.OListMessage <> s.OListMessage 
	OR t.PostMessage <> s.PostMessage 
	OR t.OReadMessage <> s.OReadMessage 
	OR t.MinimumReadLevel <> s.MinimumReadLevel 
	OR t.MinimumPostLevel <> s.MinimumPostLevel 
	OR t.MinimumRemoveLevel <> s.MinimumRemoveLevel 
	OR t.MaximumPosts <> s.MaximumPosts 
	OR t.BoardObjectId <> s.BoardObjectId)
    THEN UPDATE SET 
		t.BoardTypeId = s.BoardTypeId,
		t.Name = s.Name,
		t.ReadGroup = s.ReadGroup, 
		t.PostGroup = s.PostGroup,
		t.ExtraReaders = s.ExtraReaders,
		t.ExtraRemovers = s.ExtraRemovers,
		t.OTakeMessage = s.OTakeMessage,
		t.OPostMessage = s.OPostMessage,
		t.ORemoveMessage = s.ORemoveMessage,
		t.OCopyMessage = s.OCopyMessage,
		t.OListMessage = s.OListMessage, 
		t.PostMessage = s.PostMessage,
		t.OReadMessage = s.OReadMessage,
		t.MinimumReadLevel = s.MinimumReadLevel,
		t.MinimumPostLevel = s.MinimumPostLevel,
		t.MinimumRemoveLevel = s.MinimumRemoveLevel,
		t.MaximumPosts = s.MaximumPosts,
		t.BoardObjectId = s.BoardObjectId;

PRINT 'Done Synchronizing dbo.Boards'
GO
