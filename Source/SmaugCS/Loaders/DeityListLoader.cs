using System.Collections.Generic;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
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
            var path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Deity);

            var deities = GameConstants.GetAppSetting("Deities");
            if (string.IsNullOrEmpty(deities))
                return;

            IEnumerable<string> deityList = deities.Split(new[] { ',' });
            foreach (var deityName in deityList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + deityName + ".lua");
            }
        }
    }
}
