CREATE TABLE [live].[OrganizationTypes]
(
	[OrganizationTypeId] TINYINT NOT NULL,
	[Name] VARCHAR(50) NOT NULL

	CONSTRAINT [PK_OrganizationTypes]
		PRIMARY KEY CLUSTERED ([OrganizationTypeId] ASC)
)
