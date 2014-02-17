using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Realm.Library.Common;

namespace SmaugCS.Constants.Enums
{
    public enum ComponentOperatorTypes
    {
        [Name("!")]
        FailIfPresent,

        [Name("+")]
        DoNotConsume,
 
        [Name("@")]
        DecreaseValue0,

        [Name("#")]
        DecreaseValue1,
 
        [Name("$")]
        DecreaseValue2,

        [Name("%")]
        DecreaseValue3,

        [Name("^")]
        DecreaseValue4,

        [Name("&")]
        DecreaesValue5
    }
}
