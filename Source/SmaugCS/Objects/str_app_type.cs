
namespace SmaugCS
{
    public class str_app_type
    {
        public short tohit { get; set; }
        public short todam { get; set; }
        public short Carry { get; set; }
        public short Wield { get; set; }

        public str_app_type(short tohit, short todam, short carry, short wield)
        {
            this.tohit = tohit;
            this.todam = todam;
            Carry = carry;
            Wield = wield;
        }
    }
}
