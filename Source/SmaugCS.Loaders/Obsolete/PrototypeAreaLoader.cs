using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Logging;

namespace SmaugCS.Loaders
{
    public class PrototypeAreaLoader
    {
        public string Directory => SystemConstants.GetSystemDirectory(SystemDirectoryTypes.God);

        public void Load()
        {
            var dirProxy = new DirectoryProxy();
            var files = dirProxy.GetFiles(Directory);

            var validLines = new List<string> { "level", "roomrange", "mobrange", "objrange" };

            foreach (var file in files.Where(x => !x.Equals(".")))
            {
                using (var proxy = new TextReaderProxy(new StreamReader(Directory + file)))
                {
                    IEnumerable<string> lines = proxy.ReadIntoList();

                    int low = 0, hi = 0, rlow = 0, rhi = 0, mlow, mhi, olow, ohi;
                    var badFile = false;

                    foreach (var line in lines)
                    {
                        string[] words;
                        switch (line.ToLower())
                        {
                            case "level":
                                if (low < LevelConstants.ImmortalLevel)
                                {
                                    LogManager.Instance.Bug("God file {0} with level {1} < {2}", file, low,
                                                   LevelConstants.ImmortalLevel);
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
