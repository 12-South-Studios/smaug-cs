namespace SmaugCS.Ban
{
    public static class SqlProcedureStatics
    {
        public static string BanAdd =
            "INSERT INTO Bans (BanTypeId, Name, Note, BannedBy, BannedOn, Duration) VALUES (@BanTypeId, @Name, @Note, @BannedBy, @BannedOn, @Duration);";

        public static string BanRemove = "DELETE FROM Bans WHERE BanID = @BanId;";

        public static string BanGetByName =
            "SELECT b.BanId, bt.Name as BanType, b.Name, b.Note, b.BannedBy, b.BannedOn, b.Duration FROM Bans b JOIN BanTypes bt ON b.BanTypeId = bt.BanTypeId WHERE b.Name = @Name;";


    }
}
