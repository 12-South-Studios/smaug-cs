using System;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Loaders.Loaders
{
    public class ClassLoader : BaseLoader
    {
        public ClassLoader(ILuaManager luaManager) : base(luaManager)
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
            get { return "Classes"; }
        }

        protected override SystemDirectoryTypes SystemDirectory
        {
            get { return SystemDirectoryTypes.Class; }
        }
    }
}
