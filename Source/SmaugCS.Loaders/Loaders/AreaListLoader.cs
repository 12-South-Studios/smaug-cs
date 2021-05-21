using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;
using System;

namespace SmaugCS.Loaders.Loaders
{
    public class AreaListLoader : BaseLoader
    {
        public AreaListLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        public override string Filename => SystemConstants.GetSystemFile(SystemFileTypes.Areas);

        public override void Save()
        {
            throw new NotImplementedException();
        }

        protected override string AppSettingName => "Areas";

        protected override SystemDirectoryTypes SystemDirectory => SystemDirectoryTypes.Area;
    }
}
