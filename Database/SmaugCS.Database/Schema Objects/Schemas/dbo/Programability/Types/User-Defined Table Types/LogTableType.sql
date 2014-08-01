/*
-------------------------------------------------------------------------------------
Table Type for inserting into dbo.Logs
-------------------------------------------------------------------------------------
Date		Author			Description
--------	-------			---------------------------------------------------------
2014/08/01	Jason Murdick	Initial Release
-------------------------------------------------------------------------------------
*/
CREATE TYPE [dbo].[LogTableType] AS TABLE(
	[LogTypeId] TINYINT NOT NULL,
	[Text] VARCHAR(MAX) NOT NULL
)
GO

GRANT EXECUTE ON TYPE::[dbo].[LogTableType] TO [Server] AS dbo;
GO