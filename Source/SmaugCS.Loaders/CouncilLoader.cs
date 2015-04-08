using System;
using System.Collections.Generic;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Lua;

namespace SmaugCS.Loaders
{
    public class CouncilLoader : ListLoader
    {
        public override string Filename
        {
            get
            {
                return SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Council) +
                       SystemConstants.GetSystemFile(SystemFileTypes.Councils);
            }
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
            var path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Council);

            var councils = GameConstants.GetAppSetting("Councils");
            if (string.IsNullOrEmpty(councils))
                return;

            IEnumerable<string> councilList = councils.Split(',');
            foreach (var councilName in councilList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + councilName + ".lua");
            }
        }
    }
}
