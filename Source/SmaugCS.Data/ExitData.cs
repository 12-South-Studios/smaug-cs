using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Common;
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

        public ExitData(long id, string name) : base(id, name) { }

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
    }
}
