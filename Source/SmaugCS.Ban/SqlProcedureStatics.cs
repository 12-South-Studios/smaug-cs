namespace SmaugCS.Ban
{
    public static class SqlProcedureStatics
    {
        /*public const string BanSave = 
            @"INSERT INTO Bans (BanTypeId, Name, Note, BannedBy, BannedOn, Duration, 
                Level, Warn, Prefix, Suffix) VALUES (@BanTypeId, @Name, @Note, @BannedBy, 
                @BannedOn, @Duration, @Level, @Warn, @Prefix, @Suffix);";*/
        public const string BanSave = "cp_AddOrUpdateBan";

        /*public const string BanDelete = 
            @"DELETE FROM Bans WHERE BanID = @BanId;";*/
        public const string BanDelete = "cp_DeleteBan";

        /*public const string BanGetAll = 
            @"SELECT b.BanId, bt.Name as BanType, b.Name, b.Note, b.BannedBy, b.BannedOn, 
                b.Duration, b.Level, b.Warn, b.Prefix, b.Suffix FROM Bans b JOIN BanTypes bt 
                ON b.BanTypeId = bt.BanTypeId;";*/
        public const string BanGetAll = "cp_GetBans";

        /*public const string BanUpdate =
            @"UPDATE Bans SET BanTypeId = @BanTypeId, Name = @Name, Note = @Note, 
                BannedBy = @BannedBy, BannedOn = @BannedOn, Duration = @Duration, 
                Level = @Level, Warn = @Warn, Prefix = @Prefix, Suffix = @Suffix 
                WHERE BanId = @BanId;";*/
        public const string BanUpdate = "cp_AddOrupdateBan";
    }
}
