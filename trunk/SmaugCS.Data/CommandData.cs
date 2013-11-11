
namespace SmaugCS.Data
{
    public class CommandData
    {
        public string Name { get; set; }
        public DoFunction DoFunction { get; set; }
        public string fun_name { get; set; }
        public int flags { get; set; }
        public short position { get; set; }
        public short level { get; set; }
        public short log { get; set; }
        public UseHistory userec { get; set; }
        public int lag_count { get; set; }
    }
}
