using System;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Loaders.Loaders
{
    public class LanguageLoader : BaseLoader
    {
        public LanguageLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        public override string Filename
        {
            get
            {
                return SystemConstants.GetSystemDirectory(SystemDirectory) +
                       SystemConstants.GetSystemFile(SystemFileTypes.Tongues);
            }
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        protected override string AppSettingName
        {
            get { return "Languages"; }
        }

        protected override SystemDirectoryTypes SystemDirectory
        {
            get { return SystemDirectoryTypes.Language; }
        }
    }
}
