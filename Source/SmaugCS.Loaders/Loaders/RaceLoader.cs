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

        public override string Filename
        {
            get { return string.Empty; }
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        protected override string AppSettingName
        {
            get { return "Races"; }
        }

        protected override SystemDirectoryTypes SystemDirectory
        {
            get { return SystemDirectoryTypes.Race; }
        }
    }
}
