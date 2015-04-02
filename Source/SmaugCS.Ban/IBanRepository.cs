using System.Collections.Generic;

namespace SmaugCS.Ban
{
    public interface IBanRepository
    {
        void Add(BanData ban);
        void Load();
        void Save();

        IEnumerable<BanData> Bans { get; }
    }
}
