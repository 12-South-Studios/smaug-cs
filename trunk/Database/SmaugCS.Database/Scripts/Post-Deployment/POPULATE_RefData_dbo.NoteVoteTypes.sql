PRINT 'Starting dbo.NoteVoteTypes Syncronization'
GO

DECLARE @Source TABLE (
    NoteVoteTypeId TINYINT NOT NULL
    , Name VARCHAR(50) NOT NULL);

SET NOCOUNT ON;
INSERT @Source (NoteVoteTypeId, Name)
VALUES (1, N'Yes'),
	   (2, N'No'),
	   (3, N'Abstain');
SET NOCOUNT OFF;

MERGE dbo.NoteVoteTypes AS t
USING @Source AS s
    ON s.NoteVoteTypeId = t.NoteVoteTypeId
WHEN NOT MATCHED BY TARGET THEN
    INSERT (NoteVoteTypeId, Name)
    VALUES (s.NoteVoteTypeId, s.Name)
WHEN MATCHED AND (t.Name <> s.Name)
    THEN UPDATE SET t.Name = s.Name
WHEN NOT MATCHED BY SOURCE THEN
    DELETE;

PRINT 'Done Synchronizing dbo.NoteVoteTypes'
GO
