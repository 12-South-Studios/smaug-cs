using System.Collections.Generic;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data.Interfaces
{
    public interface IHasMudProgs
    {
        List<MudProgData> MudProgs { get; set; }
        void AddMudProg(MudProgData mprog);
        bool HasProg(int prog);
        bool HasProg(MudProgTypes type);
    }
}
