﻿using System;
using System.IO;
using Realm.Library.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Loaders
{
    public class ReservedLoader : ListLoader
    {
        public override string Filename
        {
            get { return SystemConstants.GetSystemFile(SystemFileTypes.Reserved); }
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Load()
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                string reservedName = proxy.ReadString();
                if (reservedName.Equals("$"))
                    return;

                db.ReservedNames.Add(reservedName);
            }

            db.sort_reserved(db.ReservedNames);
        }
    }
}
