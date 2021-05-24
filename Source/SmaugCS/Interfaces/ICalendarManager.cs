using SmaugCS.Data;

namespace SmaugCS
{
    public interface ICalendarManager
    {
        void Initialize();
        void CalculateSeason(TimeInfoData gameTime);
        TimeInfoData GameTime { get; }
    }
}
