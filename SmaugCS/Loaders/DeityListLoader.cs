using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
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
                LogManager.Instance.Boot("Loaded Deity {0}", deityName);
            }
        }

        /*      public override void Save()
        {

            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(Filename)))
            {
                foreach (DeityData deity in db.DEITIES)
                    proxy.Write("{0}\n", deity.Filename);
                proxy.Write("$\n");
            }
        }

        public override void Load()
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                List<string> lines = proxy.ReadIntoList().Where(x => !x.Equals("$")).ToList();
                foreach (DeityData deity in lines.Select(filename => new DeityData(filename)))
                {
                    deity.Load();
                    db.DEITIES.Add(deity);
                }
            }
        }*/
    }
}
