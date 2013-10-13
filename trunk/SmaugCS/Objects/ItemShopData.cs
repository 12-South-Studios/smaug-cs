using System.Xml.Serialization;

namespace SmaugCS.Objects
{
    public class ItemShopData : ShopData
    {
        [XmlElement]
        public int ProfitBuy { get; set; }

        [XmlElement]
        public int ProfitSell { get; set; }
    }
}
