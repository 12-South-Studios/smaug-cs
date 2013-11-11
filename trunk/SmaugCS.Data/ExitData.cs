using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    [XmlRoot("Exit")]
    public class ExitData : Entity
    {
        [XmlElement("Reverse")]
        public long Reverse { get; set; }

        [XmlElement("Destination")]
        public long Destination { get; set; }

        [XmlElement]
        public string Keyword { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlElement("ID")]
        public long vnum { get { return ID; } }

        [XmlElement("RoomID")]
        public long Room_vnum { get; set; }

        public int Flags { get; set; }
        public int Key { get; set; }
        public int vdir { get; set; }
        public int Distance { get; set; }
        public int Pull { get; set; }
        public DirectionPullTypes PullType { get; set; }

        public ExitData(long id, string name) : base(id, name) { }

        public int Equals(ExitData exit)
        {
            return vdir < exit.vdir
                ? -1 : vdir > exit.vdir
                ? 1 : 0;
        }

        /*public void SaveFUSS(TextWriterProxy proxy)
        {
            proxy.Write("#EXIT\n");
            proxy.Write("Direction {0}~\n", GameConstants.dir_name[vdir]);
            proxy.Write("ToRoom    {0}\n", Destination.Vnum);
            if (Key > 0)
                proxy.Write("Key       {0}\n", Key);
            if (Distance > 1)
                proxy.Write("Distance  {0}\n", Distance);
            if (Pull > 0)
                proxy.Write("Pull      {0} {1}\n", (int)PullType, Pull);
            if (!Description.IsNullOrEmpty())
                proxy.Write("Desc      {0}~\n", Description);
            if (!Keyword.IsNullOrEmpty())
                proxy.Write("Keywords  {0}~\n", Keyword);
            if (Flags > 0)
                proxy.Write("Flags     {0}~\n", Flags.GetFlagString(BuilderConstants.ex_flags));
            proxy.Write("#ENDEXIT\n\n");
        }*/
    }
}
