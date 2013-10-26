using System.Xml.Serialization;

// ReSharper disable CheckNamespace
namespace SmaugCS.Objects
// ReSharper restore CheckNamespace
{
    public class RepairShopData : ShopData
    {
        [XmlElement]
        public int ProfitFix { get; set; }
    }
}
