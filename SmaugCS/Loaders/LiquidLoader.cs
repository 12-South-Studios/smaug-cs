using System.IO;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Loaders
{
    public class LiquidLoader : ListLoader
    {
        #region Overrides of ListLoader

        public override string Filename
        {
            get { return SystemConstants.GetSystemDirectory(SystemDirectoryTypes.System) + "liquids.dat"; }
        }

        public override void Save()
        {
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(Filename)))
            {
                foreach (LiquidData liquid in db.LIQUIDS)
                {
                    proxy.Write("#LIQUID\n");
                    proxy.Write("Name      {0}~\n", liquid.Name);
                    proxy.Write("Shortdesc {0}~\n", liquid.ShortDescription);
                    proxy.Write("Color     {0}~\n", liquid.Color);
                    proxy.Write("Type      {0}\n", liquid.Type);
                    proxy.Write("Mod       {0} {1} {2} {3}\n",
                        liquid.Mods[ConditionTypes.Drunk],
                        liquid.Mods[ConditionTypes.Full],
                        liquid.Mods[ConditionTypes.Thirsty],
                        liquid.Mods[ConditionTypes.Bloodthirsty]);
                    proxy.Write("End\n\n");
                }

                proxy.Write("#END\n");
            }
        }

        public override void Load()
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                string word = string.Empty;

                do
                {
                    word = proxy.ReadNextWord();
                    if (word.StartsWithIgnoreCase("*"))
                        continue;

                    if (word.EqualsIgnoreCase("#liquid"))
                    {
                        LiquidData liquid = ReadLiquid(proxy);
                        if (liquid == null)
                        {
                            // TODO bug, read to end of block
                            continue;
                        }

                        // TODO Verify the vnum isn't already present

                        db.LIQUIDS.Add(liquid);
                    }
                } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("#end"));
            }
        }

        private static LiquidData ReadLiquid(TextReaderProxy proxy)
        {
            string word = string.Empty;
            LiquidData liquid = new LiquidData();

            do
            {
                word = proxy.ReadNextWord();
                if (word.StartsWithIgnoreCase("*"))
                    continue;
                if (word.StartsWithIgnoreCase("#"))
                    break;

                switch (word.ToLower())
                {
                    case "color":
                        liquid.Color = proxy.ReadString().TrimHash();
                        break;
                    case "end":
                        break;
                    case "name":
                        liquid.Name = proxy.ReadString().TrimHash();
                        break;
                    case "mod":
                        string[] words = proxy.ReadString().Split(new[] { ' ' });
                        liquid.Mods[ConditionTypes.Drunk] = words[0].ToInt32();
                        liquid.Mods[ConditionTypes.Full] = words[1].ToInt32();
                        liquid.Mods[ConditionTypes.Thirsty] = words[2].ToInt32();
                        liquid.Mods[ConditionTypes.Bloodthirsty] = words[3].ToInt32();
                        break;
                    case "shortdesc":
                        liquid.ShortDescription = proxy.ReadString().TrimHash();
                        break;
                    case "type":
                        liquid.Type = EnumerationExtensions.GetEnum<LiquidTypes>(proxy.ReadNumber());
                        break;
                    case "vnum":
                        liquid.Vnum = proxy.ReadNumber();
                        break;
                }
            } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("end"));

            return liquid;
        }

        #endregion
    }
}
