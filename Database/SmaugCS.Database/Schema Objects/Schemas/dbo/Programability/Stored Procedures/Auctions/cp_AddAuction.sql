/*
--------------------------------------------------------------------------
Adds a new auction
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
2014/08/27	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [dbo].[cp_AddAuction]
	@sellerName VARCHAR(100),
	@buyerName VARCHAR(100),
	@soldOn DATETIME,
	@soldFor INT,
	@itemSoldID INT
AS 
BEGIN
	SET NOCOUNT ON;

	DECLARE @NewBanId INT;

	INSERT INTO [dbo].[Auctions] (
		SellerName,
		BuyerName,
		SoldOn,
		SoldFor,
		ItemSoldID ) 
	VALUES (
		@sellerName,
		@buyerName,
		@soldOn,
		@soldFor,
		@itemSoldID);

	SELECT SCOPE_IDENTITY() AS AuctionId;

END

GO
GRANT EXECUTE ON [dbo].[cp_AddAuction] 
	TO [Server] AS dbo;