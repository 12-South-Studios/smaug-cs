using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Loaders.Loaders;

namespace SmaugCS.Loaders
{
    public class TimeLoader : BaseLoader
    {
        #region Overrides of ListLoader

        public TimeLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        public override string Filename
        {
            get { return SystemConstants.GetSystemDirectory(SystemDirectoryTypes.System) + "time.dat"; }
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        protected override string AppSettingName
        {
            get { throw new NotImplementedException(); }
        }

        protected override SystemDirectoryTypes SystemDirectory
        {
            get { throw new NotImplementedException(); }
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
