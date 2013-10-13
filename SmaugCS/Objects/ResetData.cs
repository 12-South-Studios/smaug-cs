using System.Collections.Generic;

namespace SmaugCS.Objects
{
    public class ResetData
    {
        public string command { get; set; }
        public int extra { get; set; }
        public int[] arg { get; set; }
        public bool sreset { get; set; }
        
        public ResetData()
        {
            arg = new int[3];
        }
    }
}
