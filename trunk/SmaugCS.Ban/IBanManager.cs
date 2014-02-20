using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.Ban
{
    public interface IBanManager
    {
        void LoadBans();

        bool AddBan(BanData ban);
        bool RemoveBan(int id);
        BanData GetBan(string name);
        BanData GetBan(int id);
        IEnumerable<BanData> GetBans(string bannedBy);
        IEnumerable<BanData> GetBans();
    }
}
