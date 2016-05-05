using System;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;

namespace SmaugCS.Loaders.Loaders
{
    public class ClanLoader : BaseLoader
    {
        public ClanLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        public override string Filename => SystemConstants.GetSystemDirectory(SystemDirectory) +
                                           SystemConstants.GetSystemFile(SystemFileTypes.Clans);

        public override void Save()
        {
            throw new NotImplementedException();
        }

        protected override string AppSettingName => "Clans";

        protected override SystemDirectoryTypes SystemDirectory => SystemDirectoryTypes.Clan;
    }
}
