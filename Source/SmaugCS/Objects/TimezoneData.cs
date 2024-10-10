
namespace SmaugCS;

public class TimezoneData
{
    public string Name { get; private set; }
    public string Zone { get; private set; }
    public int GMTOffset { get; private set; }
    public int DSTOffset { get; private set; }

    public TimezoneData(string name, string zone, int gmt_offset, int dst_offset)
    {
            Name = name;
            Zone = zone;
            GMTOffset = gmt_offset;
            DSTOffset = dst_offset;
        }
}