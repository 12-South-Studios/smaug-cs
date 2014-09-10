CREATE TABLE [live].[CharacterSkills]
(
	[CharacterSkillsId] INT IDENTITY NOT NULL,
	[CharacterId] BIGINT NOT NULL,
	[SkillTypeId] TINYINT NOT NULL,
	[SkillName] VARCHAR(50) NOT NULL,
	[LearnedValue] SMALLINT NOT NULL,

    CONSTRAINT [PK_CharacterSkills]
		PRIMARY KEY CLUSTERED ([CharacterSkillsId] ASC),

	CONSTRAINT [FK_CharacterSkills_CharacterId]
		FOREIGN KEY ([CharacterId]) REFERENCES [live].[Characters]([CharacterId]),

	CONSTRAINT [FK_CharacterSkills_SkillTypeId]
		FOREIGN KEY ([SkillTypeId]) REFERENCES [live].[SkillTypes] ([SkillTypeId])
)
GO

CREATE NONCLUSTERED INDEX [IX_CharacterSkills_CharacterId] 
	ON [live].[CharacterSkills] ([CharacterId])
GO

CREATE NONCLUSTERED INDEX [IX_CharacterSkills_SkillTypeId] 
	ON [live].[CharacterSkills] ([SkillTypeId])
GO
