using System;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Loaders.Loaders
{
    public class RaceLoader : BaseLoader
    {
        public RaceLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        public override string Filename => string.Empty;

        public override void Save()
        {
            throw new NotImplementedException();
        }

        protected override string AppSettingName => "Races";

        protected override SystemDirectoryTypes SystemDirectory => SystemDirectoryTypes.Race;
    }
}
