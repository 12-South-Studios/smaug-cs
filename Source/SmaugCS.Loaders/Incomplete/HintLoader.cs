using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;
using SmaugCS.Loaders.Loaders;

namespace SmaugCS.Loaders.Incomplete
{
    public class HintLoader : BaseLoader
    {
        public HintLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        public override string Filename => SystemConstants.GetSystemFile(SystemFileTypes.Hints);

        public override void Save()
        {
            //using (var proxy = new TextWriterProxy(new StreamWriter(Filename)))
            //{
            //    foreach (var hint in db.HINTS)
            //    {
            //        proxy.Write("Text {0}~\n", hint.Text);
            //        proxy.Write("Low  {0}\n", hint.Low);
            //        proxy.Write("High {0}\n", hint.High);
            //        proxy.Write("End\n");
            //    }
            //}
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
            //using (var proxy = new TextReaderProxy(new StreamReader(Filename)))
            //{
            //    IEnumerable<string> lines = proxy.ReadIntoList();
            //    HintData newHint = null;

            //    foreach (var line in lines)
            //    {
            //        if (newHint == null)
            //            newHint = new HintData();

            //        switch (line.ToLower())
            //        {
            //            case "text":
            //                newHint.Text = line.TrimHash();
            //                break;
            //            case "end":
            //                db.HINTS.Add(newHint);
            //                newHint = null;
            //                break;
            //            case "high":
            //                newHint.High = line.ToInt32();
            //                break;
            //            case "low":
            //                newHint.Low = line.ToInt32();
            //                break;
            //        }
            //    }
            //}
        }
    }
}
