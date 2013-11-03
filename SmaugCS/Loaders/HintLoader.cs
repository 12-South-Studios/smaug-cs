using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Objects;

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
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(Filename)))
            {
                foreach (HintData hint in db.HINTS)
                {
                    proxy.Write("Text {0}~\n", hint.Text);
                    proxy.Write("Low  {0}\n", hint.Low);
                    proxy.Write("High {0}\n", hint.High);
                    proxy.Write("End\n");
                }
            }
        }

        public override void Load()
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                List<string> lines = proxy.ReadIntoList();
                HintData newHint = null;

                foreach (string line in lines)
                {
                    if (newHint == null)
                        newHint = new HintData();

                    switch (line.ToLower())
                    {
                        case "text":
                            newHint.Text = line.TrimHash();
                            break;
                        case "end":
                            db.HINTS.Add(newHint);
                            newHint = null;
                            break;
                        case "high":
                            newHint.High = line.ToInt32();
                            break;
                        case "low":
                            newHint.Low = line.ToInt32();
                            break;
                    }
                }
            }
        }
    }
}
