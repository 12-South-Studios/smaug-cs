using System.Xml.Serialization;

namespace SmaugCS.Objects
{
    public class RepairShopData : ShopData
    {
        [XmlElement]
        public int ProfitFix { get; set; }
    }
}
