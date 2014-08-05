/*
--------------------------------------------------------------------------
Gets the list of Notes for a particular Board
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
2014/08/04	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [dbo].[cp_GetBoardNotes]
	@boardId INT
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT 
		bn.NoteId,
		n.Sender,
		n.DateSent,
		n.RecipientList,
		n.[Subject],
		n.Voting,
		n.[Text]
	FROM dbo.BoardNotes bn
	JOIN dbo.Notes n ON bn.NoteId = n.NoteId
	WHERE bn.BoardId = @boardId;
		
END

GO
GRANT EXECUTE ON [dbo].[cp_GetBoardNotes] TO [Server] AS dbo;