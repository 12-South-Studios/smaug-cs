using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Objects;

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
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                TimeInfoData timeInfo = new TimeInfoData();

                List<string> lines = proxy.ReadIntoList();
                foreach (string line in lines.Where(x => !x.StartsWith("*")))
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
