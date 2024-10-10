using SmaugCS.Constants.Enums;

namespace SmaugCS.Data;

public class TimerData
{
    public DoFunction Action { get; set; }
    public int Value { get; set; }
    public TimerTypes Type { get; set; }
    public int Count { get; set; }
}