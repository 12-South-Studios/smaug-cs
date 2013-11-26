using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Managers;

namespace SmaugCS.Loaders
{
    internal class ClassLoader : ListLoader
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
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Class);
            IEnumerable<string> classList = Program.GetAppSetting("Classes").Split(new[] { ',' });

            foreach (string className in classList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + className + ".lua");
                LogManager.BootLog("Loaded Class {0}", className);
            }
        }
    }
}
