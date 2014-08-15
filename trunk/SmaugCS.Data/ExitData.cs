using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("Exit")]
    public class ExitData : Entity
    {
        public static ExitData Create(long id, string name)
        {
            return new ExitData(id, name);
        }

        [XmlElement("Reverse")]
        public long Reverse { get; set; }

        [XmlElement("Destination")]
        public long Destination { get; set; }

        [XmlElement]
        public string Keywords { get; set; }

        [XmlElement]
        public string Description { get; set; }

        [XmlElement("ID")]
        public long vnum { get { return ID; } }

        [XmlElement("RoomID")]
        public long Room_vnum { get; set; }

        public int Flags { get; set; }
        public int Key { get; set; }

        [XmlElement("Direction")]
        public DirectionTypes Direction { get; set; }

        public int Distance { get; set; }
        public int Pull { get; set; }
        public DirectionPullTypes PullType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        private ExitData(long id, string name) : base(id, name) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flags"></param>
        public void SetFlags(string flags)
        {
            string[] words = flags.Split(new[] { ' ' });
            foreach (string word in words)
            {
                ExitFlags flag = Realm.Library.Common.EnumerationExtensions.GetEnumIgnoreCase<ExitFlags>(word);
                int flagValue = (int)flag;
                Flags = Flags.SetBit(flagValue);
            }
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
