using SmaugCS.Data;
using System.Collections.Generic;

namespace SmaugCS.Clans
{
    public interface IClanRepository
    {
        void Add(ClanData clan);
        void Load();
        void Save();

        IEnumerable<ClanData> Clans { get; }
    }
}
