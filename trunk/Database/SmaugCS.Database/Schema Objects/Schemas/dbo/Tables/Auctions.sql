﻿CREATE TABLE [dbo].[Auctions]
(
	[AuctionId] INT IDENTITY NOT NULL,
	[SellerName] VARCHAR(100) NOT NULL,
	[BuyerName] VARCHAR(100) NOT NULL,
	[SoldOn] DATETIME NOT NULL,
	[SoldFor] INT NOT NULL,
	[ItemSoldID] INT NOT NULL,

    CONSTRAINT [PK_Auctions]
		PRIMARY KEY CLUSTERED ([AuctionId] ASC)
)
GO
