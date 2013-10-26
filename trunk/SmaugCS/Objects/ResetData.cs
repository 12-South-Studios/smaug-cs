using System.Collections.Generic;

namespace SmaugCS.Objects
{
    public class ResetData
    {
        public string Command { get; set; }
        public int Extra { get; set; }
        public int[] Args { get; set; }
        public bool sreset { get; set; }
        public List<ResetData> Resets { get; set; }
 
        public ResetData()
        {
            Args = new int[3];
            Resets = new List<ResetData>();
        }
    }
}
