﻿using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Interfaces;
using SmaugCS.Loaders.Loaders;
using System.Linq;

namespace SmaugCS.Loaders.Incomplete
{
    public class MixtureLoader : BaseLoader
    {
        #region Overrides of ListLoader

        public MixtureLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        public override string Filename => SystemConstants.GetSystemDirectory(SystemDirectoryTypes.System) + "mixtures.dat";

        public override void Save()
        {
            /*using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(Filename)))
            {
                foreach (MixtureData mixture in db.MIXTURES)
                {
                    proxy.Write("#MIXTURE\n");
                    proxy.Write("Name      {0}~\n", mixture.Name);
                    proxy.Write("Data      {0} {1} {2}\n",
                                mixture.Data[0], mixture.Data[1], mixture.Data[2]);
                    proxy.Write("Object    {0}\n", mixture.Object);
                    proxy.Write("End\n\n");
                }

                proxy.Write("#END\n");
            }*/
        }

        protected override string AppSettingName
        {
            get { throw new System.NotImplementedException(); }
        }

        protected override SystemDirectoryTypes SystemDirectory
        {
            get { throw new System.NotImplementedException(); }
        }

        public override void Load()
        {
            /*using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                string word = string.Empty;

                do
                {
                    word = proxy.ReadNextWord();
                    if (word.StartsWithIgnoreCase("*"))
                        continue;

                    if (word.EqualsIgnoreCase("#mixture"))
                    {
                        MixtureData mix = ReadMixture(proxy);
                        if (mix == null)
                        {
                            // TODO bug, read to end of block
                            continue;
                        }

                        // TODO Verify the vnum isn't already present

                        db.MIXTURES.Add(mix);
                    }
                } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("#end"));
            }*/
        }

        private static MixtureData ReadMixture(TextReaderProxy proxy)
        {
            var word = string.Empty;
            var mix = new MixtureData();

            do
            {
                word = proxy.ReadNextWord();
                if (word.StartsWithIgnoreCase("*"))
                    continue;
                if (word.StartsWithIgnoreCase("#"))
                    break;

                switch (word.ToLower())
                {
                    case "end":
                        break;
                    case "data":
                        mix.Data.ToList()[0] = proxy.ReadNumber();
                        mix.Data.ToList()[1] = proxy.ReadNumber();
                        mix.Data.ToList()[2] = proxy.ReadNumber();
                        break;
                    case "into":
                        mix.Data.ToList()[2] = proxy.ReadNumber();
                        break;
                    case "name":
                        mix.Name = proxy.ReadString().TrimHash();
                        break;
                    case "object":
                        mix.Object = proxy.ReadNumber() > 0;
                        break;
                    case "with":
                        mix.Data.ToList()[0] = proxy.ReadNumber();
                        mix.Data.ToList()[1] = proxy.ReadNumber();
                        break;
                }
            } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("end"));

            return mix;
        }

        #endregion
    }
}
