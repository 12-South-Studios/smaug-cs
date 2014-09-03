﻿using System.Collections.Generic;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Managers;

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
            throw new System.NotImplementedException();
        }

        public override void Load()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Council);

            string councils = GameConstants.GetAppSetting("Councils");
            if (string.IsNullOrEmpty(councils))
                return;

            IEnumerable<string> councilList = councils.Split(new[] { ',' });
            foreach (string councilName in councilList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + councilName + ".lua");
            }
        }
    }
}