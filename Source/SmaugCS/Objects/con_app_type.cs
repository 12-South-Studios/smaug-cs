
namespace SmaugCS;

public class con_app_type
{
    public short hitp { get; set; }
    public short shock { get; set; }

    public con_app_type(short hitp, short shock)
    {
            this.hitp = hitp;
            this.shock = shock;
        }
}