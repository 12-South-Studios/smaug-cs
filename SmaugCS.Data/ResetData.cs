using System.Collections.Generic;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    public class ResetData
    {
        public ResetTypes Type { get; set; }
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

        public void SetArgs(int v1, int v2, int v3)
        {
            Args[0] = v1;
            Args[1] = v2;
            Args[2] = v3;
        }

        public void AddReset(string type, int extra, int arg1, int arg2, int arg3)
        {
            ResetData newReset = new ResetData
            {
                Type = EnumerationExtensions.GetEnumIgnoreCase<ResetTypes>(type),
                Extra = extra
            };
            newReset.SetArgs(arg1, arg2, arg3);
            Resets.Add(newReset);
        }
    }
}
