/*
--------------------------------------------------------------------------
Adds a new or updates an existing Ban
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
03/11/2014	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [dbo].[cp_AddOrUpdateBan]
	@banId INT = NULL,
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
			,[Active]
		FROM @tvpBanTable;

		SET @NewBanId = SCOPE_IDENTITY();
		END
	ELSE BEGIN
		WITH BanCTE ([BanTypeId]
			,[Name]
			,[Note]
			,[BannedBy]
			,[BannedOn]
			,[Duration]
			,[Level]
			,[Warn]
			,[Prefix]
			,[Suffix]
			,[Active])
		AS (
			SELECT TOP 1 
				[BanTypeId]
				,[Name]
				,[Note]
				,[BannedBy]
				,[BannedOn]
				,[Duration]
				,[Level]
				,[Warn]
				,[Prefix]
				,[Suffix]
				,[Active]
			FROM @tvpBanTable
		) 
		UPDATE [dbo].[Bans]
		SET [BanTypeId] = b.[BanTypeId]
			,[Name] = b.[Name]
			,[Note] = b.[Note]
			,[BannedBy] = b.[BannedBy]
			,[BannedOn] = b.[BannedOn]
			,[Duration] = b.[Duration]
			,[Level] = b.[Level]
			,[Warn] = b.[Warn]
			,[Prefix] = b.[Prefix]
			,[Suffix] = b.[Suffix]
			,[Active] = b.[Active]
		FROM BanCTE b
		WHERE [BanId] = @banId;

		SET @NewBanId = @banId;
	END

	SELECT @NewBanId AS BanId;

END

GO
GRANT EXECUTE ON [dbo].[cp_AddOrUpdateBan] 
	TO [Server] AS dbo;