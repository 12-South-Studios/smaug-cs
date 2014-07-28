using System.IO;
using Realm.Library.Common;
using SmaugCS.Data;
using SmaugCS.Data;

namespace SmaugCS.Loaders
{
    public class HelpAreaLoader : AreaLoader
    {
        public HelpAreaLoader(string areaName, bool bootDb)
            : base(areaName, bootDb)
        {
        }

        public override AreaData LoadArea(AreaData area)
        {
            throw new System.NotImplementedException();
        }
    }
}
