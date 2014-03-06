using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS
{
    public static class SqlQueries
    {
        public const string START_SESSION = 
            @"INSERT INTO SystemData (SessionStart) 
                VALUES (CURRENT_TIMESTAMP);";

        public const string END_SESSION =
            @"UPDATE SystemData 
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
}
