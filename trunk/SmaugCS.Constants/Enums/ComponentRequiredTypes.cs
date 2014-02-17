using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realm.Library.Common;

namespace SmaugCS.Constants.Enums
{
    public enum ComponentRequiredTypes
    {
        [Name("T")]
        ItemType, 

        [Name("V")]
        ItemVnum, 

        [Name("K")]
        ItemKeyword,
 
        [Name("G")]
        PlayerCoin,
 
        [Name("H")]
        PlayerHealth
    }
}
