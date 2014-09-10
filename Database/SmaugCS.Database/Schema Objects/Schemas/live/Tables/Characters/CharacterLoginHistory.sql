CREATE TABLE [live].[CharacterLoginHistory]
(
	[CharacterLoginHistoryId] BIGINT IDENTITY (1, 1) NOT NULL, 
	[CharacterId] BIGINT NOT NULL,
	[LoginDate] DATETIME NOT NULL,
	[IpAddress] VARCHAR(50) NULL,

	CONSTRAINT pk_CharacterLoginHistory
		PRIMARY KEY CLUSTERED ([CharacterLoginHistoryId])  
);
GO

CREATE NONCLUSTERED INDEX [IDX_CharacterLoginHistory_CharacterId]
    ON [live].[CharacterLoginHistory]([CharacterId] ASC);
GO
