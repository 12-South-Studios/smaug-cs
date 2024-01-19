using SmaugCS.Data;
using SmaugCS.Loaders.Obsolete;
using SmaugCS.Logging;
using System;

namespace SmaugCS.Loaders.Incomplete
{
    public class HelpAreaLoader : AreaLoader
    {
        public HelpAreaLoader(string areaName, bool bootDb)
            : base(areaName, bootDb)
        {
        }

        public override AreaData LoadArea(ILogManager logManager, AreaData area)
        {
            throw new NotImplementedException();
        }
    }
}
