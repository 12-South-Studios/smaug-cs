using System.Collections.Generic;
using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data.Shops
{
    [XmlRoot("Shop")]
    public abstract class ShopData
    {
        public int Keeper { get; set; }

        public List<ItemTypes> ItemTypes { get; set; }

        public int OpenHour { get; set; }

        public int CloseHour { get; set; }

        public ShopTypes ShopType { get; set; }

        protected ShopData()
        {
            ItemTypes = new List<ItemTypes>();
        }

        public void AddItemType(string type)
        {
            ItemTypes itemType = EnumerationExtensions.GetEnumIgnoreCase<ItemTypes>(type);
            if (!ItemTypes.Contains(itemType))
                ItemTypes.Add(itemType);
        }
    }
}
