using System.Collections.Generic;
using SmaugCS.Enums;


namespace SmaugCS.Objects
{
    public class trv_world
    {
        public List<trv_data> trv { get; set; }
        public object limit { get; set; }
        public trv_type type { get; set; }
    }
}
