using SmaugCS.Constants.Enums;

namespace SmaugCS.MudProgs;

public interface IMudProgHandler
{
    bool Execute(MudProgLocationTypes locationType, MudProgTypes mudProgType, params object[] args);
}