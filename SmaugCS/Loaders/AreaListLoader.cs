﻿using System;
using System.Collections.Generic;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS.Loaders
{
    public class AreaListLoader : ListLoader
    {
        public override string Filename
        {
            get { return SystemConstants.GetSystemFile(SystemFileTypes.Areas); }
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Area);

            string areas = GameConstants.GetAppSetting("Areas");
            if (string.IsNullOrEmpty(areas))
                throw new EntryNotFoundException("Areas list not found in app.config");

            IEnumerable<string> areaList = areas.Split(new[] { ',' });
            foreach (string areaName in areaList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + areaName + ".lua");
                LogManager.Instance.Boot("Loaded Area {0}", areaName);
            }
        }
    }
}