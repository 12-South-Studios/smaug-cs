using System.Collections.Generic;
using System.Xml.Serialization;
using SmaugCS.Enums;


namespace SmaugCS.Objects
{
    [XmlRoot("Shop")]
    public class ShopData
    {
        [XmlElement]
        public int Keeper { get; set; }

        [XmlArray]
        public List<ItemTypes> ItemTypes { get; set; }

        [XmlElement]
        public int OpenHour { get; set; }

        [XmlElement]
        public int CloseHour { get; set; }

        [XmlElement]
        public int ShopType { get; set; }

        protected ShopData()
        {
            ItemTypes = new List<ItemTypes>();
        }
    }
}
