
namespace SmaugCS;

public class liq_type
{
    public string liq_name { get; set; }
    public string liq_color { get; set; }
    public short[] liq_affect { get; set; }

    public liq_type(string name, string color, short aff1, short aff2, short aff3)
    {
            liq_name = name;
            liq_color = color;
            liq_affect = new short[3];
            liq_affect[0] = aff1;
            liq_affect[1] = aff2;
            liq_affect[2] = aff3;
        }
}