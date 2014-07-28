using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Organizations;
using SmaugCS.Logging;
using SmaugCS.Managers;

namespace SmaugCS.Loaders
{
    public class ClanLoader : ListLoader
    {
        public override string Filename
        {
            get
            {
                return SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Clan) +
                       SystemConstants.GetSystemFile(SystemFileTypes.Clans);
            }
        }

        public override void Save()
        {
            throw new System.NotImplementedException();
        }

        public override void Load()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Clan);
            IEnumerable<string> clanList = GameConstants.GetAppSetting("Clans").Split(new[] { ',' });

            foreach (string clanName in clanList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + clanName + ".lua");
            }
        }
    }
}
