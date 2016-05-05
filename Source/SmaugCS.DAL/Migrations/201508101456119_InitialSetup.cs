namespace SmaugCS.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        SellerName = c.String(maxLength: 100),
                        BuyerName = c.String(maxLength: 100),
                        SoldOn = c.DateTime(false),
                        SoldFor = c.Int(false),
                        ItemSoldId = c.Long(false)
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bans",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        BanType = c.Int(false),
                        Name = c.String(maxLength: 100),
                        Note = c.String(maxLength: 1024),
                        BannedBy = c.String(maxLength: 25),
                        BannedOn = c.DateTime(false),
                        Duration = c.Int(false),
                        Level = c.Int(false),
                        IsWarning = c.Boolean(false),
                        IsPrefix = c.Boolean(false),
                        IsSuffix = c.Boolean(false),
                        IsActive = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Boards",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        BoardType = c.Int(false),
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
                        MinimumReadLevel = c.Int(false),
                        MinimumPostLevel = c.Int(false),
                        MinimumRemoveLevel = c.Int(false),
                        MaximumPosts = c.Int(false),
                        BoardObjectId = c.Long(false)
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        Sender = c.String(),
                        DateSent = c.DateTime(false),
                        RecipientList = c.String(),
                        Subject = c.String(),
                        IsPoll = c.Boolean(false),
                        Text = c.String(),
                        Board_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boards", t => t.Board_Id)
                .Index(t => t.Board_Id);
            
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        Name = c.String(),
                        Password = c.String(),
                        Description = c.String(),
                        Gender = c.Int(false),
                        School = c.Int(false),
                        Race = c.Int(false),
                        DeletedOnUtc = c.DateTime(false),
                        Age = c.Int(false),
                        Level = c.Int(false),
                        RoomId = c.Int(false),
                        AuthedBy = c.String(),
                        CouncilId = c.Long(false),
                        DeityId = c.Int(false),
                        ClanId = c.Long(false),
                        ImmortalData_Id = c.Int(),
                        Pet_Id = c.Int()
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
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        PvPKills = c.Int(false),
                        PvPDeaths = c.Int(false),
                        PvPTimer = c.Int(false),
                        PvEKills = c.Int(false),
                        PvEDeaths = c.Int(false),
                        IllegalPvP = c.Int(false),
                        PlayedTime = c.Long(false),
                        IdleTime = c.Long(false),
                        AuctionBidsPlaced = c.Int(false),
                        AuctionsWon = c.Int(false),
                        AuctionsStarted = c.Int(false),
                        CoinEarned = c.Long(false),
                        Character_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterAffects",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        AffectType = c.String(),
                        Duration = c.Int(false),
                        Modifier = c.Int(false),
                        Location = c.Int(false),
                        Flags = c.Int(false),
                        Character_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterFriends",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        FriendName = c.String(),
                        AddedOn = c.DateTime(false),
                        Character_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterIgnored",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        IgnoredName = c.String(),
                        AddedOn = c.DateTime(false),
                        Character_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterImmortals",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        BamfinMessage = c.String(),
                        BamfoutMessage = c.String(),
                        Trust = c.Int(false),
                        WizInvis = c.Int(false),
                        ImmortalRank = c.String()
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CharacterItems",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        ItemId = c.Long(false),
                        Count = c.Int(false),
                        Location = c.Int(false),
                        Flags = c.Int(false),
                        ContainedInId = c.Long(false),
                        Value1 = c.Int(false),
                        Value2 = c.Int(false),
                        Value3 = c.Int(false),
                        Value4 = c.Int(false),
                        Value5 = c.Int(false),
                        Value6 = c.Int(false),
                        OverrideName = c.String(),
                        OverrideShortDescription = c.String(),
                        OverrideLongDescription = c.String(),
                        Character_Id = c.Int(),
                        CharacterPet_Id = c.Int()
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
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        LanguageName = c.String(),
                        Character_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterLogins",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        LoginDate = c.DateTime(false),
                        IpAddress = c.String(),
                        SessionId = c.Int(false),
                        Character_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sessions", t => t.SessionId)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.SessionId)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        IpAddress = c.String(),
                        Port = c.Int(false)
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CharacterPets",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        MonsterId = c.Int(false),
                        RoomId = c.Int(false),
                        OverrideName = c.String(),
                        OverrideShortDescription = c.String(),
                        OverrideLongDescription = c.String(),
                        Position = c.Int(false),
                        Flags = c.Int(false)
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CharacterPvEHistories",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        MonsterId = c.Int(false),
                        TimesKilled = c.Int(false),
                        Character_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterSkills",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        SkillType = c.Int(false),
                        SkillName = c.String(),
                        LearnedValue = c.Int(false),
                        Character_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.CharacterStatistics",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        Statistic = c.Int(false),
                        IntValue = c.Int(),
                        StringValue = c.String(),
                        Character_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.GameStates",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        GameYear = c.Int(false),
                        GameMonth = c.Int(false),
                        GameDay = c.Int(false),
                        GameHour = c.Int(false)
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        LogType = c.Int(false),
                        Text = c.String(),
                        SessionId = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sessions", t => t.SessionId)
                .Index(t => t.SessionId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        Name = c.String(),
                        Header = c.String(),
                        Level = c.Int(false),
                        CreatedOn = c.DateTime(false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(false)
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsEntries",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        Title = c.String(),
                        Name = c.String(),
                        Text = c.String(),
                        PostedOn = c.DateTime(false),
                        PostedBy = c.String(),
                        IsActive = c.Boolean(false),
                        News_Id = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.News", t => t.News_Id)
                .Index(t => t.News_Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        OrganizationType = c.Int(false),
                        Name = c.String(),
                        Description = c.String(),
                        Leader = c.String(),
                        BoardId = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boards", t => t.BoardId)
                .Index(t => t.BoardId);
            
            CreateTable(
                "dbo.WeatherCells",
                c => new
                    {
                        Id = c.Int(false, true),
                        CreateDateUtc = c.DateTime(),
                        CellXCoordinate = c.Int(false),
                        CellYCoordinate = c.Int(false),
                        ClimateType = c.Int(false),
                        HemisphereType = c.Int(false),
                        CloudCover = c.Int(false),
                        Energy = c.Int(false),
                        Humidity = c.Int(false),
                        Precipitation = c.Int(false),
                        Pressure = c.Int(false),
                        Temperature = c.Int(false),
                        WindSpeedX = c.Int(false),
                        WindSpeedY = c.Int(false)
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organizations", "BoardId", "dbo.Boards");
            DropForeignKey("dbo.NewsEntries", "News_Id", "dbo.News");
            DropForeignKey("dbo.Logs", "SessionId", "dbo.Sessions");
            DropForeignKey("dbo.CharacterStatistics", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.CharacterSkills", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.CharacterPvEHistories", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.Characters", "Pet_Id", "dbo.CharacterPets");
            DropForeignKey("dbo.CharacterItems", "CharacterPet_Id", "dbo.CharacterPets");
            DropForeignKey("dbo.CharacterLogins", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.CharacterLogins", "SessionId", "dbo.Sessions");
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
            DropIndex("dbo.Logs", new[] { "SessionId" });
            DropIndex("dbo.CharacterStatistics", new[] { "Character_Id" });
            DropIndex("dbo.CharacterSkills", new[] { "Character_Id" });
            DropIndex("dbo.CharacterPvEHistories", new[] { "Character_Id" });
            DropIndex("dbo.CharacterLogins", new[] { "Character_Id" });
            DropIndex("dbo.CharacterLogins", new[] { "SessionId" });
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
            DropTable("dbo.Sessions");
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
