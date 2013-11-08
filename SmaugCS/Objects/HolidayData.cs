
using System;
using System.Linq;
using SmaugCS.Common;
using Realm.Library.Common.Extensions;

namespace SmaugCS.Objects
{
    public class HolidayData
    {
        public int Month { get; set; }
        public int Day { get; set; }
        public string Name { get; set; }
        public string Announce { get; set; }

        public void Load(Realm.Library.Common.TextSection section)
        {
            foreach (string line in section.Lines.Where(x => !x.StartsWith("*")))
            {
                Tuple<string, string> tuple = line.FirstArgument();
                switch (tuple.Item1.ToLower())
                {
                    case "announce":
                        Announce = tuple.Item2.TrimHash();
                        break;
                    case "day":
                        Day = tuple.Item2.ToInt32();
                        break;
                    case "end":
                        if (!string.IsNullOrEmpty(Announce))
                            Announce = "Today is a holiday, but who the hell knows which one.";
                        break;
                    case "month":
                        Month = tuple.Item2.ToInt32();
                        break;
                    case "name":
                        Name = tuple.Item2.TrimHash();
                        break;
                }
            }
        }
    }
}
