using System.Collections.Generic;
using SmaugCS.Constants.Enums;

// ReSharper disable once CheckNamespace
namespace SmaugCS.Data
{
    public interface IHasMudProgs
    {
        IEnumerable<MudProgData> MudProgs { get; }
        void AddMudProg(MudProgData mprog);
        bool HasProg(int prog);
        bool HasProg(MudProgTypes type);
    }
}
