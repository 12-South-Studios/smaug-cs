CREATE TABLE [live].[UserLoginHistory]
(
	[UserLoginHistoryID] BIGINT IDENTITY (1, 1) NOT NULL, 
	[UserID] BIGINT NOT NULL,
	[LoginDate] DATETIME NOT NULL,
	[IpAddress] VARCHAR(50) NULL
	CONSTRAINT pk_UserLoginHistory
		PRIMARY KEY CLUSTERED ([UserLoginHistoryID])  
);
GO

ALTER TABLE live.[UserLoginHistory]
	ADD CONSTRAINT FK_UserLoginHistory_UserID
		FOREIGN KEY (UserID) 
		REFERENCES live.[User] (UserID)
		ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

CREATE NONCLUSTERED INDEX [IDX_UserLoginHistory_UserID]
    ON [live].[UserLoginHistory]([UserID] ASC);
GO
