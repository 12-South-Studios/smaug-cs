using System.Collections.Generic;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
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
            var path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Clan);

            var clans = GameConstants.GetAppSetting("Clans");
            if (string.IsNullOrEmpty(clans))
                return;

            IEnumerable<string> clanList = clans.Split(new[] { ',' });
            foreach (var clanName in clanList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + clanName + ".lua");
            }
        }
    }
}
