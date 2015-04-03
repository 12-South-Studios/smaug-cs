using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Objects;
using SmaugCS.Repository;

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
            using (var proxy = new TextWriterProxy(new StreamWriter(Filename)))
            {
                foreach (var watch in db.WATCHES)
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
            using (var proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                var number = proxy.ReadNumber();
                if (number == -1)
                    return;

                var watch = new WatchData
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
                foreach (var command in RepositoryManager.Instance.COMMANDS.Values
                    .Where(command => command.Name.EqualsIgnoreCase(watch.TargetName)))
                {
                    command.Flags.SetBit((int)CommandFlags.Watch);
                    break;
                }

                db.WATCHES.Add(watch);
            }
        }

        #endregion
    }
}
