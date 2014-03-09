using System;
using System.Collections.Generic;
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
            IEnumerable<string> areaList = GameConstants.GetAppSetting("Areas").Split(new[] { ',' });

            foreach (string areaName in areaList)
            {
                LuaManager.Instance.DoLuaScript(path + "\\" + areaName + ".lua");
                LogManager.Instance.Boot("Loaded Area {0}", areaName);
            }
        }

        /*public override void Load()
        {
            List<AreaLoader> loaders = new List<AreaLoader>();

            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                LogManager.Instance.Log("Reading Area List...");
                List<string> areaNames = proxy.ReadIntoList();

                foreach (string areaName in areaNames.Where(x => !x.Equals("$")))
                {
                    AreaType areaType = ReadAreaFileVersion(areaName);
                    if (areaType == AreaType.Error)
                    {
                        LogManager.Instance.Bug("Unable to determine area type for area file {0}", areaName);
                        continue;
                    }

                    switch (areaType)
                    {
                        case AreaType.FUSS:
                            loaders.Add(new FUSSAreaLoader(areaName, DatabaseManager.BootDb));
                            break;
                        case AreaType.Help:
                            loaders.Add(new HelpAreaLoader(areaName, DatabaseManager.BootDb));
                            break;
                        default:
                            loaders.Add(new SmaugAreaLoader(areaName, DatabaseManager.BootDb, 0));
                            break;
                    }
                }
            }

            ProcessAreaLoaders(loaders);
        }

        private static AreaType ReadAreaFileVersion(string filename)
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Area) + filename;

            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                string word = proxy.ReadNextWord();
                if (!word.StartsWith("#"))
                {
                    LogManager.Instance.Bug("No # found at start of area file: {0}", filename);

                    if (DatabaseManager.BootDb)
                        throw new InitializationException("Invalid Area File {0}", filename);
                    return AreaType.Error;
                }

                switch (word.ToUpper())
                {
                    case "#FUSSAREA":
                        return AreaType.FUSS;
                    case "#HELPS":
                        return AreaType.Help;
                    default:
                        return AreaType.Smaug;
                }
            }
        }

        private static void ProcessAreaLoaders(IEnumerable<AreaLoader> areaLoaders)
        {
            int count = 0;
            foreach (AreaLoader loader in areaLoaders)
            {
                LogManager.Instance.Log("Loading area {0}", loader.AreaName);
                AreaData area = loader.LoadArea(null);
                if (area == null)
                {
                    LogManager.Instance.Bug("Failed to load area {0}", loader.AreaName);
                    continue;
                }

                count++;
            }

            LogManager.Instance.Log("Loaded {0} areas.", count);
        }*/
    }
}
