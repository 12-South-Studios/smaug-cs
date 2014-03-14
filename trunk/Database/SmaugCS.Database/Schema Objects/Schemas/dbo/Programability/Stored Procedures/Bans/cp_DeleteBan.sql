/*
--------------------------------------------------------------------------
Deletes a ban
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
03/11/2014	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [dbo].[cp_DeleteBan]
	@banId INT
AS 
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Bans 
	SET Active = 0 
	WHERE BanId = @banId;
		
END

GO
GRANT EXECUTE ON [dbo].[cp_DeleteBan] TO [Server] AS dbo;