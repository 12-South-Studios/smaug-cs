using System.Collections.Generic;
using System.Data;
using SmallDBConnectivity;
using SmaugCS.Data.Instances;
using SmaugCS.Logging;

namespace SmaugCS.Ban
{
    public interface IBanManager
    {
        void Initialize(ILogManager logManager, ISmallDb smallDb, IDbConnection connection);

        void LoadBans();
        void ClearBans();

        bool AddBan(BanData ban);
        bool RemoveBan(int id);
        BanData GetBan(string name);
        BanData GetBan(int id);
        IEnumerable<BanData> GetBans(string bannedBy);
        IEnumerable<BanData> GetBans();

        bool CheckTotalBans(string host, int supremeLevel);
        bool CheckBans(CharacterInstance ch, int type);
    }
}
