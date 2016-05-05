using System.Collections.Generic;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Interfaces;

namespace SmaugCS.Loaders.Loaders
{
    public abstract class BaseLoader
    {
        public abstract string Filename { get; }
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
            var path = SystemConstants.GetSystemDirectory(SystemDirectory);

            var appSetting = GameConstants.GetAppSetting(AppSettingName);
            if (string.IsNullOrEmpty(appSetting))
                throw new EntryNotFoundException("{0} not found in app.config", AppSettingName);

            IEnumerable<string> fileList = appSetting.Split(',');
            foreach (var fileName in fileList)
            {
                _luaManager.DoLuaScript(path + "\\" + fileName + ".lua");
            }
        }
    }
}
