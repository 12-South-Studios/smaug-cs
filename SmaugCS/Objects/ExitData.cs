using System.Xml.Serialization;
using SmaugCS.Common;
using SmaugCS.Enums;

namespace SmaugCS.Objects
{
    [XmlRoot("Exit")]
    public class ExitData
    {
        [XmlElement("Reverse")]
        private int ReverseID { get; set; }
        public ExitData ReverseExit
        {
            get { return Destination.GetExit(ReverseID); }
            set { ReverseID = value.vnum; }
        }

        [XmlElement("Destination")]
        private int DestinationID { get; set; }
        public RoomTemplate Destination
        {
            get { return db.ROOMS.Find(x => x.Vnum == DestinationID); }
            set { DestinationID = value.Vnum; }
        }

        [XmlElement]
        public string Keyword { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlElement("ID")]
        public int vnum { get; set; }

        [XmlElement("RoomID")]
        public int Room_vnum { get; set; }

        public int Flags { get; set; }
        public int Key { get; set; }
        public int vdir { get; set; }
        public int Distance { get; set; }
        public int Pull { get; set; }
        public DirectionPullTypes PullType { get; set; }

        public int Equals(ExitData exit)
        {
            return vdir < exit.vdir
                ? -1 : vdir > exit.vdir
                ? 1 : 0;
        }

        public void SetBExitFlag(int flag)
        {
            Flags.SetBit(flag);

            if (ReverseExit != null && ReverseExit != this)
                ReverseExit.Flags.SetBit(flag);
        }

        public void RemoveBExitFlag(int flag)
        {
            Flags.RemoveBit(flag);

            if (ReverseExit != null && ReverseExit != this)
                ReverseExit.Flags.RemoveBit(flag);
        }

        public void ToggleBExitFlag(int flag)
        {
            Flags.ToggleBit(flag);

            if (ReverseExit != null && ReverseExit != this)
                ReverseExit.Flags.ToggleBit(flag);
        }
    }
}
