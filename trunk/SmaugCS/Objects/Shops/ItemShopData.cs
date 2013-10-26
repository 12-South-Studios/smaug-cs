using System.Xml.Serialization;

// ReSharper disable CheckNamespace
namespace SmaugCS.Objects
// ReSharper restore CheckNamespace
{
    public class ItemShopData : ShopData
    {
        [XmlElement]
        public int ProfitBuy { get; set; }

        [XmlElement]
        public int ProfitSell { get; set; }
    }
}
