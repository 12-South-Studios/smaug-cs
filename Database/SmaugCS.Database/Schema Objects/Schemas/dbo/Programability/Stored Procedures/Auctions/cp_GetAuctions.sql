/*
--------------------------------------------------------------------------
Gets the list of Auctions
--------------------------------------------------------------------------
Date		Author			Description
--------	-------			-------------------------------------
2014/08/27	Jason Murdick	Initial Creation
--------------------------------------------------------------------------
*/
CREATE PROCEDURE [dbo].[cp_GetAuctions]
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT 
		AuctionId,
		SellerName,
		BuyerName,
		SoldOn,
		SoldFor,
		ItemSoldID
	FROM [dbo].[Auctions] 
	ORDER BY AuctionId;
		
END

GO
GRANT EXECUTE ON [dbo].[cp_GetAuctions] TO [Server] AS dbo;