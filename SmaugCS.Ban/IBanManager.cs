using System.Collections.Generic;
using SmaugCS.Data.Instances;

namespace SmaugCS.Ban
{
    public interface IBanManager
    {
        void ClearBans();

        bool AddBan(BanData ban);
        bool RemoveBan(int id);
        BanData GetBan(string name);
        BanData GetBan(int id);
        IEnumerable<BanData> GetBans(string bannedBy);
        IEnumerable<BanData> GetBans();

        bool CheckTotalBans(string host, int supremeLevel);
        bool CheckBans(PlayerInstance ch, int type);

        void Initialize();
    }
}
