/*
--------------------------------------------------------------------------
Adds a new or updates an existing Ban
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
03/11/2014	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [dbo].[cp_AddOrUpdateBans]
	@banId INT,
	@tvpBanTable BanTableType READONLY
AS 
BEGIN
	SET NOCOUNT ON;

	DECLARE @NewBanId INT;

	
	IF @banId IS NULL BEGIN
		INSERT INTO dbo.[Bans]
		SELECT [BanTypeId]
			,[Name]
			,[Note]
			,[BannedBy]
			,[BannedOn]
			,[Duration]
			,[Level]
			,[Warn]
			,[Prefix]
			,[Suffix]
		FROM @tvpBanTable;

		SET @NewBanId = SCOPE_IDENTITY();
		END
	ELSE BEGIN
		UPDATE dbo.[Bans]
		SET [Name] = @tvpBanTable.[Name]
			,[Note] = @tvpBanTable.[Note]
			,[BannedBy] = @tvpBanTable.[BannedBy]
			,[BannedOn] = @tvpBanTable.[BannedOn]
			,[Duration] = @tvpBanTable.[Duration]
			,[Level] = @tvpBanTable.[Level]
			,[Warn] = @tvpBanTable.[Warn]
			,[Prefix] = @tvpBanTable.[Prefix]
			,[Suffix] = @tvpBanTable.[Suffix]
		WHERE [BanId] = @banId;

		SET @NewBanId = @banId;
	END

	SELECT @NewBanId AS BanId;

END

GO
GRANT EXECUTE ON [dbo].[cp_GetBans] TO [Server] AS dbo;