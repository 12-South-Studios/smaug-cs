CREATE TABLE [live].[CharacterItems]
(
	[CharacterItemsId] INT IDENTITY NOT NULL,
	[CharacterId] BIGINT NOT NULL,
	[CharacterPetId] INT NULL,
	[ItemId] BIGINT NOT NULL,
	[Count] INT NOT NULL,
	[Location] INT NOT NULL,
	[Flags] INT NOT NULL,
	[ContainedInId] BIGINT NULL,
	[Value1] INT NULL,
	[Value2] INT NULL,
	[Value3] INT NULL,
	[Value4] INT NULL,
	[Value5] INT NULL,
	[Value6] INT NULL,
	[OverrideName] VARCHAR(50) NULL,
	[OverrideShortDescription] VARCHAR(1024) NULL,
	[OverrideLongDescription] VARCHAR(1024) NULL,

    CONSTRAINT [PK_CharacterItems]
		PRIMARY KEY CLUSTERED ([CharacterItemsId] ASC),

	CONSTRAINT [FK_CharacterItems_CharacterId]
		FOREIGN KEY ([CharacterId]) REFERENCES [live].[Characters]([CharacterId]),
		
	CONSTRAINT [FK_CharacterItems_CharacterPetsId]
		FOREIGN KEY ([CharacterPetId]) REFERENCES [live].[CharacterPets]([CharacterPetsId])
)
GO

CREATE NONCLUSTERED INDEX [IX_CharacterItems_CharacterId] 
	ON [live].[CharacterItems] ([CharacterId])
GO

CREATE NONCLUSTERED INDEX [IX_CharacterItems_CharacterPetId] 
	ON [live].[CharacterPets] ([CharacterPetsId])
GO