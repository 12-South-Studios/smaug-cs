using System.IO;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Objects;

namespace SmaugCS.Loaders
{
    public class WatchListLoader : ListLoader
    {
        #region Overrides of ListLoader

        public override string Filename
        {
            get { return SystemConstants.GetSystemFile(SystemFileTypes.Watches); }
        }

        public override void Save()
        {
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(Filename)))
            {
                foreach (WatchData watch in db.WATCHES)
                {
                    proxy.Write(string.Format("{0} {1}~{2}~{3}~\n", watch.ImmortalLevel, watch.ImmortalName,
                                              string.IsNullOrEmpty(watch.TargetName) ? " " : watch.TargetName,
                                              string.IsNullOrEmpty(watch.PlayerSite) ? " " : watch.PlayerSite));
                }

                proxy.Write("-1\n");
            }
        }

        public override void Load()
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                int number = proxy.ReadNumber();
                if (number == -1)
                    return;

                WatchData watch = new WatchData
                                      {
                                          ImmortalLevel = number,
                                          ImmortalName = proxy.ReadString().TrimHash(),
                                          TargetName = proxy.ReadString().TrimHash(),
                                          PlayerSite = proxy.ReadString().TrimHash()
                                      };
                if (watch.TargetName.Length < 2)
                    throw new InvalidDataException(string.Format("Watch TargetName {0} is too short", watch.TargetName));

                if (watch.PlayerSite.Length < 2)
                    throw new InvalidDataException(string.Format("Watch PlayerSite {0} is too short", watch.PlayerSite));

                //// Check for command watches
                foreach (CommandData command in db.COMMANDS
                    .Where(command => command.Name.EqualsIgnoreCase(watch.TargetName)))
                {
                    command.flags.SetBit((int)CommandFlags.Watch);
                    break;
                }

                db.WATCHES.Add(watch);
            }
        }

        #endregion
    }
}
