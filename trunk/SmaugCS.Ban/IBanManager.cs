﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using SmaugCS.Data;

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
        bool CheckBans(CharacterInstance ch, int type);

        void Initialize();
    }
}
