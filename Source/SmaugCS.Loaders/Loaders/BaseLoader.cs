using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Interfaces;
using System.Collections.Generic;

namespace SmaugCS.Loaders.Loaders
{
    public abstract class BaseLoader : IBaseLoader
    {
        //public abstract string Filename { get; }
        public abstract void Save();

        protected abstract string AppSettingName { get; }
        protected abstract SystemDirectoryTypes SystemDirectory { get; }

        private readonly ILuaManager _luaManager;

        protected BaseLoader(ILuaManager luaManager)
        {
            _luaManager = luaManager;
        }

        public virtual void Load()
        {
            //var path = SystemConstants.GetSystemDirectory(SystemDirectory);

            //var appSetting = GameConstants.GetAppSetting(AppSettingName);
            //if (string.IsNullOrEmpty(appSetting))
            //    throw new EntryNotFoundException($"{AppSettingName} not found in app.config");

            //IEnumerable<string> fileList = appSetting.Split(',');
            //foreach (var fileName in fileList)
            //{
            //    _luaManager.DoLuaScript($"{path}\\{fileName}.lua");
            //}
        }
    }
}
