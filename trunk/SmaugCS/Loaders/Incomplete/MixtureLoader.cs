using System.IO;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Loaders
{
    public class MixtureLoader : ListLoader
    {
        #region Overrides of ListLoader

        public override string Filename
        {
            get { return SystemConstants.GetSystemDirectory(SystemDirectoryTypes.System) + "mixtures.dat"; }
        }

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
            string word = string.Empty;
            MixtureData mix = new MixtureData();

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
                        mix.Data[0] = proxy.ReadNumber();
                        mix.Data[1] = proxy.ReadNumber();
                        mix.Data[2] = proxy.ReadNumber();
                        break;
                    case "into":
                        mix.Data[2] = proxy.ReadNumber();
                        break;
                    case "name":
                        mix.Name = proxy.ReadString().TrimHash();
                        break;
                    case "object":
                        mix.Object = proxy.ReadNumber() > 0;
                        break;
                    case "with":
                        mix.Data[0] = proxy.ReadNumber();
                        mix.Data[1] = proxy.ReadNumber();
                        break;
                }
            } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("end"));

            return mix;
        }

        #endregion
    }
}
