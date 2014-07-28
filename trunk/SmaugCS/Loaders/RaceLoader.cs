﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS.Loaders
{
    public class RaceLoader : ListLoader
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
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Race);
            IEnumerable<string> raceList = GameConstants.GetAppSetting("Races").Split(new[] { ',' });

            foreach (string raceName in raceList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + raceName + ".lua");
            }
        }
    }
}
