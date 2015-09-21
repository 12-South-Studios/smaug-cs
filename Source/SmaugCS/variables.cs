using System;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Logging;
using EnumerationExtensions = Realm.Library.Common.EnumerationExtensions;

namespace SmaugCS
{
    public static class variables
    {
        public static VariableData make_variable(VariableTypes type, int vnum, string tag)
        {
            var var = new VariableData
                                   {
                                       Type = type,
                                       Flags = 0,
                                       vnum = vnum,
                                       Tag = tag,
                                       CTime = DateTime.Now,
                                       MTime = DateTime.Now,
                                       RTime = DateTime.MinValue,
                                       Timer = 0
                                   };

            if (type == VariableTypes.ExtendedBit)
                var.Data = new ExtendedBitvector();

            return var;
        }

        public static void delete_variable(VariableData var)
        {
            if (var.Type == VariableTypes.ExtendedBit
                || var.Type == VariableTypes.String
                && var.Data != null)
                var.Data = null;
            var.Tag = string.Empty;
        }

        public static VariableData get_tag(CharacterInstance ch, string tag, int vnum)
        {
            return ch.Variables.FirstOrDefault(v => (vnum == 0
                                                     || vnum == v.vnum) && tag.Equals(v.Tag));
        }

        public static bool remove_tag(CharacterInstance ch, string tag, int vnum)
        {
            if (ch.Variables == null || !ch.Variables.Any())
                return false;

            // TODO

            return false;
        }

        public static int tag_char(CharacterInstance ch, VariableData var, int replace)
        {
            // TODO
            return 0;
        }

        public static bool is_valid_tag(string tagname)
        {
            return tagname.ToCharArray().ToList().All(char.IsLetter)
                && tagname.ToCharArray().All(c => char.IsLetterOrDigit(c) || c == '_');
        }

        public static void do_mptag(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mprmtag(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mpflag(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void do_mprmflag(CharacterInstance ch, string argument)
        {
            // TODO
        }

        public static void fwrite_variables(CharacterInstance ch, TextWriterProxy proxy)
        {
            foreach (var vd in ch.Variables)
            {
                proxy.Write("#VARIABLE\n");
                proxy.Write("Type   {0}\n", vd.Type);
                proxy.Write("Flags  {0}\n", vd.Flags);
                proxy.Write("Vnum   {0}\n", vd.vnum);
                proxy.Write("Ctime  {0}\n", vd.CTime.ToFileTimeUtc());
                proxy.Write("Mtime  {0}\n", vd.MTime.ToFileTimeUtc());
                proxy.Write("Rtime  {0}\n", vd.RTime.ToFileTimeUtc());
                proxy.Write("Tag    {0}~\n", vd.Tag);
                switch (vd.Type)
                {
                    case VariableTypes.String:
                        proxy.Write("Str     {0}~\n", vd.Data.ToString());
                        break;
                    case VariableTypes.ExtendedBit:
                        proxy.Write("Xbit    {9}\n", vd.Data.ToString());
                        break;
                    case VariableTypes.Integer:
                        proxy.Write("Int     {0}\n", (long)vd.Data);
                        break;
                }
                proxy.Write("End\n\n");
            }
        }

        public static void fread_variable(CharacterInstance ch, TextReaderProxy proxy)
        {
            var vd = new VariableData();

            for (; ; )
            {
                var word = proxy.EndOfStream ? "End" : proxy.ReadNextWord();

                switch (char.ToUpper(word.ToCharArray()[0]))
                {
                    case '*':
                        proxy.ReadToEndOfLine();
                        break;
                    case 'C':
                        vd.CTime = DateTime.FromFileTimeUtc(proxy.ReadNumber());
                        break;
                    case 'E':
                        if (word.Equals("End", StringComparison.OrdinalIgnoreCase))
                        {
                            if (vd.Type == VariableTypes.Integer)
                                tag_char(ch, vd, 1);
                            else
                                LogManager.Instance.Bug("%s: invalid/incomplete variable: %s",
                                    "fread_variable", vd.Tag);
                        }
                        break;
                    case 'F':
                        vd.Flags = proxy.ReadNumber();
                        break;
                    case 'I':
                        if (word.Equals("Int", StringComparison.OrdinalIgnoreCase))
                        {
                            if (vd.Type != VariableTypes.Integer)
                                LogManager.Instance.Bug("%s: Type mismatch -- type(%d) != vtInt",
                                       "fread_variable", vd.Type);
                            else
                                vd.Data = proxy.ReadNumber();
                        }
                        break;
                    case 'M':
                        vd.MTime = DateTime.FromFileTimeUtc(proxy.ReadNumber());
                        break;
                    case 'R':
                        vd.RTime = DateTime.FromFileTimeUtc(proxy.ReadNumber());
                        break;
                    case 'S':
                        if (word.Equals("Str", StringComparison.OrdinalIgnoreCase))
                        {
                            if (vd.Type != VariableTypes.String)
                                LogManager.Instance.Bug("%s: Type mismatch -- type(%d) != String",
                                       "fread_variable", vd.Type);
                            else
                                vd.Data = proxy.ReadString();
                        }
                        break;
                    case 'T':
                        vd.Tag = proxy.ReadString("~");
                        vd.Timer = proxy.ReadNumber();
                        vd.Type = EnumerationExtensions.GetEnum<VariableTypes>(proxy.ReadNumber());
                        break;
                    case 'V':
                        vd.vnum = proxy.ReadNumber();
                        break;
                    case 'X':
                        if (word.Equals("Xbit", StringComparison.OrdinalIgnoreCase))
                        {
                            if (vd.Type != VariableTypes.ExtendedBit)
                                LogManager.Instance.Bug("%s: Type mismatch -- type(%d) != ExtendedBit",
                                       "fread_variable", vd.Type);
                            else
                                vd.Data = proxy.ReadBitvector();
                        }
                        break;
                }
            }
        }
    }
}
