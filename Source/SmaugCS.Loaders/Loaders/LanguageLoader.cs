using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;
using System;

namespace SmaugCS.Loaders.Loaders
{
    public class LanguageLoader : BaseLoader
    {
        public LanguageLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        public override string Filename => SystemConstants.GetSystemDirectory(SystemDirectory) +
                                           SystemConstants.GetSystemFile(SystemFileTypes.Tongues);

        public override void Save()
        {
            throw new NotImplementedException();
        }

        protected override string AppSettingName => "Languages";

        protected override SystemDirectoryTypes SystemDirectory => SystemDirectoryTypes.Language;
    }
}
