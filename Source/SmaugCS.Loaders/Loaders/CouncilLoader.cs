using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;
using System;

namespace SmaugCS.Loaders.Loaders
{
    public class CouncilLoader : BaseLoader
    {
        public CouncilLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        //public override string Filename => SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Council) +
        //                                   SystemConstants.GetSystemFile(SystemFileTypes.Councils);

        public override void Save()
        {
            throw new NotImplementedException();
        }

        protected override string AppSettingName => "Councils";

        protected override SystemDirectoryTypes SystemDirectory => SystemDirectoryTypes.Council;
    }
}
