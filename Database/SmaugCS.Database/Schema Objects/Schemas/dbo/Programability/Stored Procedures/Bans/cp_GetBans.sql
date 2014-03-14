/*
--------------------------------------------------------------------------
Gets the list of Bans, alternatively by type
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
03/11/2014	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [dbo].[cp_GetBans]
	@banType TINYINT
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT 
		b.BanId
		,bt.Name as BanTypeName
		,bt.Value as BanTypeValue
		,b.Name
		,b.Note
		,b.BannedBy
		,b.BannedOn
		,b.Duration
		,b.[Level]
		,b.Warn
		,b.Prefix
		,b.Suffix
	FROM dbo.Bans b 
	JOIN dbo.BanTypes bt ON b.BanTYpeId = bt.BanTypeId 
	WHERE ((@banType IS NULL) OR (b.BanTypeId = @banType)) 
		AND Active = 1;
		
END

GO
GRANT EXECUTE ON [dbo].[cp_GetBans] TO [Server] AS dbo;