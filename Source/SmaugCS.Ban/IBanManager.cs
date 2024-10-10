using SmaugCS.Data.Instances;

namespace SmaugCS.Ban;

public interface IBanManager
{
    void ClearBans();

    bool CheckTotalBans(string host, int supremeLevel);
    bool CheckBans(PlayerInstance ch, int type);

    void Initialize();

    IBanRepository Repository { get; }
}