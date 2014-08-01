CREATE TABLE [dbo].[NoteVoteTypes]
(
	[NoteVoteTypeId] TINYINT NOT NULL,
	[Name] VARCHAR(50) NOT NULL

	CONSTRAINT [PK_NoteVoteTypes]
		PRIMARY KEY CLUSTERED ([NoteVoteTypeId] ASC)
)
