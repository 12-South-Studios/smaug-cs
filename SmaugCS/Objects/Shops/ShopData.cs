using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SmaugCS.Enums;

// ReSharper disable CheckNamespace
namespace SmaugCS.Objects
// ReSharper restore CheckNamespace
{
    [XmlRoot("Shop")]
    [JsonObject]
    public abstract class ShopData
    {
        [XmlElement]
        [JsonProperty]
        public int Keeper { get; set; }

        [XmlArray]
        [JsonProperty]
        public List<ItemTypes> ItemTypes { get; set; }

        [XmlElement]
        [JsonProperty]
        public int OpenHour { get; set; }

        [XmlElement]
        [JsonProperty]
        public int CloseHour { get; set; }

        [XmlElement]
        [JsonProperty]
        public int ShopType { get; set; }

        protected ShopData()
        {
            ItemTypes = new List<ItemTypes>();
        }
    }
}
