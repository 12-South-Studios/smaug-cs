namespace SmaugCS;

public static class SqlQueries
{
    public static string StartSession = @"INSERT INTO SystemData (SessionStart) VALUES (CURRENT_TIMESTAMP);";

    public static string EndSession = @"UPDATE SystemData 
                SET SessionEnd = CURRENT_TIMESTAMP
                ,HighPlayerCount = @HighPlayerCount
                ,HighPlayerTime = @HighPlayerTime
                ,PvEKills = @PvEKills
                ,PvPKills = @PvPKills
                ,DamagePvE = @DamagePvE
                ,DamagePvP = @DamagePvP 
                ,CoinLooted = @CoinLooted
            WHERE SessionID = @SessionID;";
}