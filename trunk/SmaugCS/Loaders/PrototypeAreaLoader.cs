using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Managers;

namespace SmaugCS.Loaders
{
    public class PrototypeAreaLoader
    {
        public string Directory { get { return SystemConstants.GetSystemDirectory(SystemDirectoryTypes.God); } }

        public void Load()
        {
            DirectoryProxy dirProxy = new DirectoryProxy();
            IEnumerable<string> files = dirProxy.GetFiles(Directory);

            List<string> validLines = new List<string>() { "level", "roomrange", "mobrange", "objrange" };

            foreach (string file in files.Where(x => !x.Equals(".")))
            {
                using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(Directory + file)))
                {
                    IEnumerable<string> lines = proxy.ReadIntoList();

                    int low = 0, hi = 0, rlow = 0, rhi = 0, mlow, mhi, olow, ohi;
                    bool badFile = false;
                    string[] words;

                    foreach (string line in lines)
                    {
                        switch (line.ToLower())
                        {
                            case "level":
                                if (low < Program.GetLevel("immortal"))
                                {
                                    LogManager.Instance.Bug("God file {0} with level {1} < {2}", file, low,
                                                   Program.GetLevel("immortal"));
                                    badFile = true;
                                }
                                break;
                            case "roomrange":
                                words = line.Split();
                                rlow = low = words[1].ToInt32();
                                rhi = hi = words[2].ToInt32();
                                break;
                            case "mobrange":
                                words = line.Split();
                                rlow = low = words[1].ToInt32();
                                rhi = hi = words[2].ToInt32();
                                break;
                            case "objrange":
                                words = line.Split();
                                rlow = low = words[1].ToInt32();
                                rhi = hi = words[2].ToInt32();
                                break;
                        }
                    }

                    if (rlow > 0 && rhi > 0 && !badFile)
                    {

                    }

                }
            }
        }
    }
}
