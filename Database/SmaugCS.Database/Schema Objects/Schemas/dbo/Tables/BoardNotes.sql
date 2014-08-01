CREATE TABLE [dbo].[BoardNotes]
(
	[BoardNoteId] INT IDENTITY NOT NULL,
	[BoardId] INT NOT NULL,
	[NoteId] INT NOT NULL,

    CONSTRAINT [PK_BoardNotes]
		PRIMARY KEY CLUSTERED ([BoardNoteId] ASC),

	CONSTRAINT [FK_BoardNotes_BoardId]
		FOREIGN KEY ([BoardId]) REFERENCES [Boards]([BoardId]),

	CONSTRAINT [FK_BoardNotes_NoteId]
		FOREIGN KEY ([NoteId]) REFERENCES [Notes]([NoteId])
)
GO

CREATE NONCLUSTERED INDEX [IX_BoardNotes_BoardId] 
	ON [dbo].[BoardNotes] ([BoardId])
GO

CREATE NONCLUSTERED INDEX [IX_BoardNotes_NoteId] 
	ON [dbo].[BoardNotes] ([NoteId])
GO