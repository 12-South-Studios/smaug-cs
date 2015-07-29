namespace SmaugCS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SellerName = c.String(maxLength: 100),
                        BuyerName = c.String(maxLength: 100),
                        SoldOn = c.DateTime(nullable: false),
                        SoldFor = c.Int(nullable: false),
                        ItemSoldId = c.Long(nullable: false),
                        CreateDateUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BanType = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Note = c.String(maxLength: 1024),
                        BannedBy = c.String(maxLength: 25),
                        BannedOn = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        IsWarning = c.Boolean(nullable: false),
                        IsPrefix = c.Boolean(nullable: false),
                        IsSuffix = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreateDateUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Boards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BoardType = c.Int(nullable: false),
                        Name = c.String(),
                        ReadGroup = c.String(),
                        PostGroup = c.String(),
                        ExtraReaders = c.String(),
                        ExtraRemovers = c.String(),
                        OTakeMessage = c.String(),
                        OPostMessage = c.String(),
                        ORemoveMessage = c.String(),
                        OCopyMessage = c.String(),
                        OListMessage = c.String(),
                        PostMessage = c.String(),
                        OReadMessage = c.String(),
                        MinimumReadLevel = c.Int(nullable: false),
                        MinimumPostLevel = c.Int(nullable: false),
                        MinimumRemoveLevel = c.Int(nullable: false),
                        MaximumPosts = c.Int(nullable: false),
                        BoardObjectId = c.Long(nullable: false),
                        CreateDateUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sender = c.String(),
                        DateSent = c.DateTime(nullable: false),
                        RecipientList = c.String(),
                        Subject = c.String(),
                        IsPoll = c.Boolean(nullable: false),
                        Text = c.String(),
                        CreateDateUtc = c.DateTime(),
                        Board_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boards", t => t.Board_Id)
                .Index(t => t.Board_Id);
            
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                        Description = c.String(),
                        Gender = c.Int(nullable: false),
                        School = c.Int(nullable: false),
                        Race = c.Int(nullable: false),
                        DeletedOnUtc = c.DateTime(nullable: false),
                        Age = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        RoomId = c.Int(nullable: false),
                        AuthedBy = c.String(),
                        CouncilId = c.Long(nullable: false),
                        DeityId = c.Int(nullable: false),
                        ClanId = c.Long(nullable: false),
                        CreateDateUtc = c.DateTime(),
                        ImmortalData_Id = c.Int(),
                        Pet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CharacterImmortals", t => t.ImmortalData_Id)
                .ForeignKey("dbo.CharacterPets", t => t.Pet_Id)
                .Index(t => t.ImmortalData_Id)
                .Index(t => t.Pet_Id);
            
            CreateTable(
                "dbo.CharacterActivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PvPKills = c.Int(nullable: false),
                        PvPDeaths = c.Int(nullable: false),
                        PvPTimer = c.Int(nullable: false),
                        PvEKills = c.Int(nullable: false),
                        PvEDeaths = c.Int(nullable: false),
                        IllegalPvP = c.Int(nullable: false),
                        PlayedTime = c.Long(nullable: false),
                        IdleTime = c.Long(nullable: false),
                        AuctionBidsPlaced = c.Int(nullable: false),
                        AuctionsWon = c.Int(nullable: false),
                        AuctionsStarted = c.Int(nullable: false),
                        CoinEarned = c.Long(nullable: false),
                        CreateDateUtc = c.DateTime(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterAffects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AffectType = c.String(),
                        Duration = c.Int(nullable: false),
                        Modifier = c.Int(nullable: false),
                        Location = c.Int(nullable: false),
                        Flags = c.Int(nullable: false),
                        CreateDateUtc = c.DateTime(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterFriends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FriendName = c.String(),
                        AddedOn = c.DateTime(nullable: false),
                        CreateDateUtc = c.DateTime(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterIgnored",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IgnoredName = c.String(),
                        AddedOn = c.DateTime(nullable: false),
                        CreateDateUtc = c.DateTime(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterImmortals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BamfinMessage = c.String(),
                        BamfoutMessage = c.String(),
                        Trust = c.Int(nullable: false),
                        WizInvis = c.Int(nullable: false),
                        ImmortalRank = c.String(),
                        CreateDateUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CharacterItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Long(nullable: false),
                        Count = c.Int(nullable: false),
                        Location = c.Int(nullable: false),
                        Flags = c.Int(nullable: false),
                        ContainedInId = c.Long(nullable: false),
                        Value1 = c.Int(nullable: false),
                        Value2 = c.Int(nullable: false),
                        Value3 = c.Int(nullable: false),
                        Value4 = c.Int(nullable: false),
                        Value5 = c.Int(nullable: false),
                        Value6 = c.Int(nullable: false),
                        OverrideName = c.String(),
                        OverrideShortDescription = c.String(),
                        OverrideLongDescription = c.String(),
                        CreateDateUtc = c.DateTime(),
                        Character_Id = c.Int(),
                        CharacterPet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .ForeignKey("dbo.CharacterPets", t => t.CharacterPet_Id)
                .Index(t => t.Character_Id)
                .Index(t => t.CharacterPet_Id);
            
            CreateTable(
                "dbo.CharacterLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                        CreateDateUtc = c.DateTime(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterLogins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LoginDate = c.DateTime(nullable: false),
                        IpAddress = c.String(),
                        CreateDateUtc = c.DateTime(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterPets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MonsterId = c.Int(nullable: false),
                        RoomId = c.Int(nullable: false),
                        OverrideName = c.String(),
                        OverrideShortDescription = c.String(),
                        OverrideLongDescription = c.String(),
                        Position = c.Int(nullable: false),
                        Flags = c.Int(nullable: false),
                        CreateDateUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CharacterPvEHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MonsterId = c.Int(nullable: false),
                        TimesKilled = c.Int(nullable: false),
                        CreateDateUtc = c.DateTime(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterSkills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SkillType = c.Int(nullable: false),
                        SkillName = c.String(),
                        LearnedValue = c.Int(nullable: false),
                        CreateDateUtc = c.DateTime(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Statistic = c.Int(nullable: false),
                        IntValue = c.Int(),
                        StringValue = c.String(),
                        CreateDateUtc = c.DateTime(),
                        Character_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.GameStates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameYear = c.Int(nullable: false),
                        GameMonth = c.Int(nullable: false),
                        GameDay = c.Int(nullable: false),
                        GameHour = c.Int(nullable: false),
                        CreateDateUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogType = c.Int(nullable: false),
                        LoggedOn = c.DateTime(nullable: false),
                        Text = c.String(),
                        CreateDateUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Header = c.String(),
                        Level = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreateDateUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Name = c.String(),
                        Text = c.String(),
                        PostedOn = c.DateTime(nullable: false),
                        PostedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreateDateUtc = c.DateTime(),
                        News_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.News", t => t.News_Id)
                .Index(t => t.News_Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganizationType = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Leader = c.String(),
                        BoardId = c.Int(nullable: false),
                        CreateDateUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boards", t => t.BoardId)
                .Index(t => t.BoardId);
            
            CreateTable(
                "dbo.WeatherCells",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CellXCoordinate = c.Int(nullable: false),
                        CellYCoordinate = c.Int(nullable: false),
                        ClimateType = c.Int(nullable: false),
                        HemisphereType = c.Int(nullable: false),
                        CloudCover = c.Int(nullable: false),
                        Energy = c.Int(nullable: false),
                        Humidity = c.Int(nullable: false),
                        Precipitation = c.Int(nullable: false),
                        Pressure = c.Int(nullable: false),
                        Temperature = c.Int(nullable: false),
                        WindSpeedX = c.Int(nullable: false),
                        WindSpeedY = c.Int(nullable: false),
                        CreateDateUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organizations", "BoardId", "dbo.Boards");
            DropForeignKey("dbo.NewsEntries", "News_Id", "dbo.News");
            DropForeignKey("dbo.CharacterStatistics", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.CharacterSkills", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.CharacterPvEHistories", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.Characters", "Pet_Id", "dbo.CharacterPets");
            DropForeignKey("dbo.CharacterItems", "CharacterPet_Id", "dbo.CharacterPets");
            DropForeignKey("dbo.CharacterLogins", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.CharacterLanguages", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.CharacterItems", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.Characters", "ImmortalData_Id", "dbo.CharacterImmortals");
            DropForeignKey("dbo.CharacterIgnored", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.CharacterFriends", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.CharacterAffects", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.CharacterActivities", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.Notes", "Board_Id", "dbo.Boards");
            DropIndex("dbo.Organizations", new[] { "BoardId" });
            DropIndex("dbo.NewsEntries", new[] { "News_Id" });
            DropIndex("dbo.CharacterStatistics", new[] { "Character_Id" });
            DropIndex("dbo.CharacterSkills", new[] { "Character_Id" });
            DropIndex("dbo.CharacterPvEHistories", new[] { "Character_Id" });
            DropIndex("dbo.CharacterLogins", new[] { "Character_Id" });
            DropIndex("dbo.CharacterLanguages", new[] { "Character_Id" });
            DropIndex("dbo.CharacterItems", new[] { "CharacterPet_Id" });
            DropIndex("dbo.CharacterItems", new[] { "Character_Id" });
            DropIndex("dbo.CharacterIgnored", new[] { "Character_Id" });
            DropIndex("dbo.CharacterFriends", new[] { "Character_Id" });
            DropIndex("dbo.CharacterAffects", new[] { "Character_Id" });
            DropIndex("dbo.CharacterActivities", new[] { "Character_Id" });
            DropIndex("dbo.Characters", new[] { "Pet_Id" });
            DropIndex("dbo.Characters", new[] { "ImmortalData_Id" });
            DropIndex("dbo.Notes", new[] { "Board_Id" });
            DropTable("dbo.WeatherCells");
            DropTable("dbo.Organizations");
            DropTable("dbo.NewsEntries");
            DropTable("dbo.News");
            DropTable("dbo.Logs");
            DropTable("dbo.GameStates");
            DropTable("dbo.CharacterStatistics");
            DropTable("dbo.CharacterSkills");
            DropTable("dbo.CharacterPvEHistories");
            DropTable("dbo.CharacterPets");
            DropTable("dbo.CharacterLogins");
            DropTable("dbo.CharacterLanguages");
            DropTable("dbo.CharacterItems");
            DropTable("dbo.CharacterImmortals");
            DropTable("dbo.CharacterIgnored");
            DropTable("dbo.CharacterFriends");
            DropTable("dbo.CharacterAffects");
            DropTable("dbo.CharacterActivities");
            DropTable("dbo.Characters");
            DropTable("dbo.Notes");
            DropTable("dbo.Boards");
            DropTable("dbo.Bans");
            DropTable("dbo.Auctions");
        }
    }
}
