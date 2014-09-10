CREATE TABLE [live].[SkillTypes]
(
	[SkillTypeId] TINYINT NOT NULL,
	[Name] VARCHAR(50) NOT NULL

	CONSTRAINT [PK_SkillTypes]
		PRIMARY KEY CLUSTERED ([SkillTypeId] ASC)
)
