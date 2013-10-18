﻿using System.Collections.Generic;
using SmaugCS.Weather;

namespace SmaugCS.Objects
{
    public class plane_data
    {
        public List<afswap_data> afswap { get; set; }
        public List<snswap_data> snswap { get; set; }
        public string name { get; set; }
        public int stronger { get; set; }
        public int weaker { get; set; }
        public int nullified { get; set; }
        public int reverse { get; set; }
        public int reflected { get; set; }
        public short month_ofs { get; set; }
        public short mintemp { get; set; }
        public short maxtemp { get; set; }
        public short climate { get; set; }
        public short gravity { get; set; }
        public TimeInfoData time_info { get; set; }
        public weather_data weather_data { get; set; }
    }
}