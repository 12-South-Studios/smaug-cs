using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Loaders
{
    public class TimeLoader : ListLoader
    {
        #region Overrides of ListLoader

        public override string Filename
        {
            get { return SystemConstants.GetSystemDirectory(SystemDirectoryTypes.System) + "time.dat"; }
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public TimeInfoData LoadTimeInfo()
        {
            using (var proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                var timeInfo = new TimeInfoData();

                IEnumerable<string> lines = proxy.ReadIntoList();
                foreach (var line in lines.Where(x => !x.StartsWith("*")))
                {
                    if (line.StartsWithIgnoreCase("#time"))
                    {
                        timeInfo.Load(lines);
                        break;
                    }
                    if (line.EqualsIgnoreCase("end"))
                        break;
                }

                return timeInfo;
            }
        }

        #endregion
    }
}
