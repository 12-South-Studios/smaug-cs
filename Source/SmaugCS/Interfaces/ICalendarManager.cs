using SmaugCS.Data;

namespace SmaugCS.Interfaces;

public interface ICalendarManager
{
    void Initialize();
    void CalculateSeason(TimeInfoData gameTime);
    TimeInfoData GameTime { get; }
}