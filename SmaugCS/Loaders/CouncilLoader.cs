﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Objects;
using SmaugCS.Organizations;

namespace SmaugCS.Loaders
{
    public class CouncilLoader : ListLoader
    {
        public override string Filename
        {
            get
            {
                return SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Council) +
                       SystemConstants.GetSystemFile(SystemFileTypes.Councils);
            }
        }

        public override void Save()
        {
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(Filename)))
            {
                foreach (CouncilData council in db.COUNCILS)
                    proxy.Write("{0}\n", council.Filename);
                proxy.Write("$\n");
            }
        }

        public override void Load()
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                List<string> lines = proxy.ReadIntoList().Where(x => !x.Equals("$")).ToList();
                foreach (CouncilData council in lines.Select(filename => new CouncilData(filename)))
                {
                    council.Load();
                    db.COUNCILS.Add(council);
                }
            }
        }
    }
}