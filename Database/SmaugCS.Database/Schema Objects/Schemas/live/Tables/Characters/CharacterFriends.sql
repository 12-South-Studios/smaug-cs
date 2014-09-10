CREATE TABLE [live].[CharacterFriends]
(
	[CharacterFriendsId] INT IDENTITY NOT NULL,
	[CharacterId] BIGINT NOT NULL,
	[FriendName] VARCHAR(50) NOT NULL,
	[AddedOn] DATETIME NULL,

    CONSTRAINT [PK_CharacterFriends]
		PRIMARY KEY CLUSTERED ([CharacterFriendsId] ASC),

	CONSTRAINT [FK_CharacterFriends_CharacterId]
		FOREIGN KEY ([CharacterId]) REFERENCES [live].[Characters]([CharacterId])
)
GO

CREATE NONCLUSTERED INDEX [IX_CharacterFriends_CharacterId] 
	ON [live].[CharacterFriends] ([CharacterId])
GO
