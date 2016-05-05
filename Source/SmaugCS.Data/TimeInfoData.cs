using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    public class TimeInfoData
    {
        public int Hour { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public SunPositionTypes Sunlight { get; set; }
        public SeasonTypes Season { get; set; }

        public void SetTimeOfDay(int hourSunrise, int hourDayBegin, int hourSunset, int hourNightBegin)
        {
            if (Hour < hourSunrise)
                Sunlight = SunPositionTypes.Dark;
            else if (Hour < hourDayBegin)
                Sunlight = SunPositionTypes.Sunrise;
            else if (Hour < hourSunset)
                Sunlight = SunPositionTypes.Light;
            else if (Hour < hourNightBegin)
                Sunlight = SunPositionTypes.Sunset;
            else
                Sunlight = SunPositionTypes.Dark;
        }

        public void Load(IEnumerable<string> lines)
        {
            foreach (Tuple<string, string> tuple in lines.Where(x => !x.StartsWith("*"))
                .TakeWhile(line => !line.Equals("END")).Select(line => line.FirstArgument()))
            {
                switch (tuple.Item1.ToLower())
                {
                    case "mhour":
                        Hour = tuple.Item2.ToInt32();
                        break;
                    case "mday":
                        Day = tuple.Item2.ToInt32();
                        break;
                    case "mmonth":
                        Month = tuple.Item2.ToInt32();
                        break;
                    case "myear":
                        Year = tuple.Item2.ToInt32();
                        break;
                }
            }
        }

        public void Save()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.System) + "time.dat";
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                proxy.Write("#TIME\n");
                proxy.Write("Mhour  {0}\n", Hour);
                proxy.Write("Mday   {0}\n", Day);
                proxy.Write("Mmonth {0}\n", Month);
                proxy.Write("Myear  {0}\n", Year);
                proxy.Write("End\n\n");
                proxy.Write("#END\n");
            }
        }
    }
}
