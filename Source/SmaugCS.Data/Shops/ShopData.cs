using System.Collections.Generic;
using System.Xml.Serialization;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data.Shops
{
    [XmlRoot("Shop")]
    public abstract class ShopData
    {
        private List<ItemTypes> _itemTypes;
 
        public int Keeper { get; set; }

        public IEnumerable<ItemTypes> ItemTypes { get { return _itemTypes; } }

        public int OpenHour { get; set; }

        public int CloseHour { get; set; }

        public ShopTypes ShopType { get; set; }

        protected ShopData()
        {
            _itemTypes = new List<ItemTypes>();
        }

        public void AddItemType(string type)
        {
            ItemTypes itemType = EnumerationExtensions.GetEnumIgnoreCase<ItemTypes>(type);
            if (!_itemTypes.Contains(itemType))
                _itemTypes.Add(itemType);
        }

        public void AddItemType(ItemTypes type)
        {
            if (!_itemTypes.Contains(type))
                _itemTypes.Add(type);
        }
    }
}
