using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Loaders
{
    public class HintLoader : ListLoader
    {
        public override string Filename
        {
            get { return SystemConstants.GetSystemFile(SystemFileTypes.Hints); }
        }

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
