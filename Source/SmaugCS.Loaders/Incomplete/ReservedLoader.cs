using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;
using SmaugCS.Loaders.Loaders;
using System;

namespace SmaugCS.Loaders.Incomplete
{
    public class ReservedLoader : BaseLoader
    {
        public ReservedLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        public override string Filename => SystemConstants.GetSystemFile(SystemFileTypes.Reserved);

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
            //using (var proxy = new TextReaderProxy(new StreamReader(Filename)))
            //{
            //    var reservedName = proxy.ReadString();
            //    if (reservedName.Equals("$"))
            //        return;

            //    db.ReservedNames.Add(reservedName);
            //}

            //db.sort_reserved(db.ReservedNames);
        }
    }
}
