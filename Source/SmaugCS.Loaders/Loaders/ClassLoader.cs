using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;
using System;

namespace SmaugCS.Loaders.Loaders
{
    public class ClassLoader : BaseLoader
    {
        public ClassLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        public override string Filename => string.Empty;

        public override void Save()
        {
            throw new NotImplementedException();
        }

        protected override string AppSettingName => "Classes";

        protected override SystemDirectoryTypes SystemDirectory => SystemDirectoryTypes.Class;
    }
}
