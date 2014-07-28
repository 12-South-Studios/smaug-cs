using System;

namespace SmaugCS.Constants
{
    public class PullcheckAttribute : Attribute
    {
        public string ToChar { get; set; }
        public string ToRoom { get; set; }
        public string DestRoom { get; set; }
        public string ObjMsg { get; set; }
        public string DestObj { get; set; }

        public PullcheckAttribute(string ToChar = "", string ToRoom = "",
            string DestRoom = "", string ObjMsg = "", string DestObj = "")
        {
            this.ToChar = ToChar;
            this.ToRoom = ToRoom;
            this.DestRoom = DestRoom;
            this.ObjMsg = ObjMsg;
            this.DestObj = DestObj;
        }
    }
}
