using System.Collections.Generic;
using SmaugCS.Constants.Enums;

// ReSharper disable CheckNamespace
namespace SmaugCS.Data
// ReSharper restore CheckNamespace
{
    public interface IHasMudProgs
    {
        List<MudProgData> MudProgs { get; set; }
        void AddMudProg(MudProgData mprog);
        bool HasProg(int prog);
        bool HasProg(MudProgTypes type);
    }
}
