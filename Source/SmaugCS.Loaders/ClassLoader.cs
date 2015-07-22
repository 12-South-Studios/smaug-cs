using System;
using System.Collections.Generic;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Exceptions;
using SmaugCS.Lua;

namespace SmaugCS.Loaders
{
    public class ClassLoader : ListLoader
    {
        public override string Filename
        {
            get { return string.Empty; }
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
            var path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Class);

            var classes = GameConstants.GetAppSetting("Classes");
            if (string.IsNullOrEmpty(classes))
                throw new EntryNotFoundException("Classes not found in app.config");

            IEnumerable<string> classList = classes.Split(',');
            foreach (var className in classList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + className + ".lua");

            }
        }
    }
}
