﻿using System;
using System.Collections.Generic;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Exceptions;
using SmaugCS.Lua;

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
            var path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Race);

            var races = GameConstants.GetAppSetting("Races");
            if (string.IsNullOrEmpty(races))
                throw new EntryNotFoundException("Races not found in app.config");

            IEnumerable<string> raceList = races.Split(',');
            foreach (var raceName in raceList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + raceName + ".lua");
            }
        }
    }
}
