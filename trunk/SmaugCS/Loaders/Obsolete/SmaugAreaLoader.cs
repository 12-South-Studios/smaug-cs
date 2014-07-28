using System.IO;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Data;
using SmaugCS.Logging;

namespace SmaugCS.Loaders
{
    public class SmaugAreaLoader : AreaLoader
    {
        public int Version { get; private set; }

        public SmaugAreaLoader(string areaName, bool bootDb, int version)
            : base(areaName, bootDb)
        {
            Version = version;
        }

        #region Overrides of AreaLoader

        public override AreaData LoadArea(AreaData area)
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(FilePath)))
            {
                string word = string.Empty;
                AreaData newArea = area;
                newArea.Age = 15;
                newArea.Author = "unknown";
                newArea.Version = 0;

                do
                {
                    char c = proxy.ReadNextLetter();
                    if (c != '#')
                    {
                        LogManager.Instance.Bug("LoadArea: # not found in area file %s", AreaName);
                        throw new InitializationException("# not found in area file");
                    }

                    word = proxy.ReadNextWord();
                    if (word.StartsWith("$"))
                        break;

                    switch (word.ToUpper())
                    {
                        case "AREA":
                            // area =load_area(proxy, areaVersion);
                            break;
                        case "AUTHOR":
                            area.Author = proxy.ReadToEndOfLine();
                            break;
                        case "FLAGS":
                            area.Flags = proxy.ReadNumber();
                            area.ResetFrequency = proxy.ReadNumber();
                            area.Age = area.ResetFrequency;
                            break;
                        case "RANGES":
                            area.HighSoftRange = LevelConstants.MaxLevel;
                            area.HighHardRange = LevelConstants.MaxLevel;
                            break;
                        case "ECONOMY":
                            area.HighEconomy = proxy.ReadNumber();
                            area.LowEconomy = proxy.ReadNumber();
                            break;
                        case "RESETMSG":
                            area.ResetMessage = proxy.ReadToEndOfLine();
                            break;
                        case "HELPS":
                            // load_helps(proxy);
                            break;
                        case "MOBILES":
                            // load_mobiles(area, proxy);
                            break;
                        case "OBJECTS":
                            // load_objects(area, proxy);
                            break;
                        case "RESETS":
                            // load_resets(area, proxy);
                            break;
                        case "ROOMS":
                            // load_rooms(area, proxy);
                            break;
                        case "SHOPS":
                            // load_shops(proxy);
                            break;
                        case "REPAIRS":
                            // load_repairs(proxy);
                            break;
                        case "SPECIALS":
                            // load_specials(proxy);
                            break;
                        case "CLIMATE":
                            // load_climate(area, proxy);
                            break;
                        case "NEIGHBOR":
                            // load_neighbor(area, proxy);
                            break;
                        case "VERSION":
                            area.Version = proxy.ReadNumber();
                            break;
                        case "SPELLLIMIT":
                            area.SpellLimit = proxy.ReadNumber();
                            break;
                        default:
                            LogManager.Instance.Bug("LoadArea: Area %s: bad section name %s", AreaName, word);
                            if (BootDb)
                                throw new InitializationException("Area {0} had a bad section name {1}", AreaName, word);
                            return null;
                    }
                } while (!proxy.EndOfStream && !word.StartsWith("$"));

                return newArea;
            }
        }

        #endregion
    }
}
