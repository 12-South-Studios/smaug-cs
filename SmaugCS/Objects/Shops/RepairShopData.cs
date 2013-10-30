using System.Xml.Serialization;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace SmaugCS.Objects
// ReSharper restore CheckNamespace
{
    public class RepairShopData : ShopData
    {
        [XmlElement]
        [JsonProperty]
        public int ProfitFix { get; set; }
    }
}
