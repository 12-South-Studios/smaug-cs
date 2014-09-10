CREATE TABLE [live].[Organizations]
(
	[OrganizationId] BIGINT IDENTITY(1, 1) NOT NULL, 
	[OrganizationTypeId] TINYINT NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	[Description] VARCHAR(1024) NULL,
	[Leader] VARCHAR(50) NOT NULL,
	[BoardId] BIGINT NULL,

	CONSTRAINT pk_Organizations
		PRIMARY KEY CLUSTERED ([OrganizationId]),

	CONSTRAINT [FK_Organizations_OrganizationTypeId]
		FOREIGN KEY ([OrganizationTypeId]) REFERENCES [live].[OrganizationTypes]([OrganizationTypeId])
);
GO

CREATE NONCLUSTERED INDEX [IX_Organizations_OrganizationTypeId] 
	ON [live].[Organizations] ([OrganizationTypeId])
GO
