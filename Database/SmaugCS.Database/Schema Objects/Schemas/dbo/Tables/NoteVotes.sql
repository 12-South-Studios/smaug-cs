CREATE TABLE [dbo].[NoteVotes]
(
	[NoteVoteId] INT IDENTITY NOT NULL,
	[Name] VARCHAR(1024) NOT NULL,
	[NoteVoteTypeId] TINYINT NOT NULL,
	[NoteId] INT NOT NULL,
	 
    CONSTRAINT [PK_NoteVotes]
		PRIMARY KEY CLUSTERED ([NoteVoteId] ASC),
	
	CONSTRAINT [FK_NoteVotes_NoteVoteTypeId]
		FOREIGN KEY ([NoteVoteTypeId]) REFERENCES [NoteVoteTypes]([NoteVoteTypeId]),

	CONSTRAINT [FK_NoteVotes_NoteId] 
		FOREIGN KEY ([NoteId]) REFERENCES [Notes]([NoteId])
)
GO

CREATE NONCLUSTERED INDEX [IX_NoteVotes_NoteVoteTypeId] 
	ON [dbo].[NoteVotes] ([NoteVoteTypeId])
GO

CREATE NONCLUSTERED INDEX [IX_NoteVotes_NoteId] 
	ON [dbo].[NoteVotes] ([NoteId])
GO
