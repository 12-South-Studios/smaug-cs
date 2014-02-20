namespace SmaugCS.Ban
{
    public static class SqlProcedureStatics
    {
        public const string BanAdd = 
            @"INSERT INTO Bans (BanTypeId, Name, Note, BannedBy, BannedOn, Duration, 
                Level, Warn, Prefix, Suffice) VALUES (@BanTypeId, @Name, @Note, @BannedBy, 
                @BannedOn, @Duration, @Level, @Warn, @Prefix, @Suffix);";

        public const string BanRemove = @"DELETE FROM Bans WHERE BanID = @BanId;";

        public const string BanGetByName = 
            @"SELECT b.BanId, bt.Name as BanType, b.Name, b.Note, b.BannedBy, b.BannedOn, 
                b.Duration, b.Level, b.Warn, b.Prefix, b.Suffice FROM Bans b JOIN BanTypes bt 
                ON b.BanTypeId = bt.BanTypeId WHERE b.Name = @Name;";
    }
}
