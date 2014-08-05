/*
--------------------------------------------------------------------------
Gets the list of Boards, alternatively by type
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
2014/08/04	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [dbo].[cp_GetBoards]
	@boardType TINYINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT 
		b.BoardId
		,bt.Name as BoardTypeName
		,b.Name
		,b.ReadGroup
		,b.PostGroup
		,b.ExtraReaders
		,b.ExtraRemovers
		,b.OTakeMessage
		,b.OPostMessage
		,b.ORemoveMessage
		,b.OCopyMessage
		,b.OListMessage
		,b.PostMessage
		,b.OReadMessage
		,b.MinimumReadLevel
		,b.MinimumPostLevel
		,b.MaximumPosts
		,b.BoardObjectId
	FROM dbo.Boards b 
	JOIN dbo.BoardTypes bt ON b.BoardTypeId = bt.BoardTypeId 
	WHERE ((@boardType IS NULL) OR (b.BoardTypeId = @boardType));
		
END

GO
GRANT EXECUTE ON [dbo].[cp_GetBoards] TO [Server] AS dbo;