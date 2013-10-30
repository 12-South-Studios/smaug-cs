using System.Xml.Serialization;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace SmaugCS.Objects
// ReSharper restore CheckNamespace
{
    public class ItemShopData : ShopData
    {
        [XmlElement]
        [JsonProperty]
        public int ProfitBuy { get; set; }

        [XmlElement]
        [JsonProperty]
        public int ProfitSell { get; set; }
    }
}
