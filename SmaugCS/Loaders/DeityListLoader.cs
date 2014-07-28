using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS.Loaders
{
    public class DeityListLoader : ListLoader
    {
        public override string Filename
        {
            get
            {
                return SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Deity) +
                       SystemConstants.GetSystemFile(SystemFileTypes.Deities);
            }
        }

        public override void Save()
        {
            throw new System.NotImplementedException();
        }

        public override void Load()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Deity);
            IEnumerable<string> deityList = GameConstants.GetAppSetting("Deities").Split(new[] { ',' });

            foreach (string deityName in deityList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + deityName + ".lua");
            }
        }
    }
}
