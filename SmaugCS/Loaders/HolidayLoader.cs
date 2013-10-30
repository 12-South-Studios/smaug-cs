using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Managers;
using SmaugCS.Objects;

namespace SmaugCS.Loaders
{
    public class HolidayLoader : ListLoader
    {
        #region Overrides of ListLoader

        public override string Filename
        {
            get { return SystemConstants.GetSystemFile(SystemFileTypes.Holiday); }
        }

        public override void Save()
        {
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(Filename)))
            {
                foreach (HolidayData day in db.HOLIDAYS)
                {
                    proxy.Write("#HOLIDAY\n");
                    proxy.Write("Name     {0}~\n", day.Name);
                    proxy.Write("Announce {0}~\n", day.Announce);
                    proxy.Write("Month    {0}\n", day.Month);
                    proxy.Write("Day      {0}\n", day.Day);
                    proxy.Write("End\n\n");
                }
                proxy.Write("#END\n");
            }
        }

        public override void Load()
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                int dayCount = 0;
                List<TextSection> sections = proxy.ReadSections(new[] { "#HOLIDAY" }, new[] { "*" }, null, "END");
                foreach (TextSection section in sections)
                {
                    if (dayCount >= db.SystemData.MaxHolidays)
                    {
                        LogManager.Bug("Exceeded maximum holidays {0}", dayCount);
                        return;
                    }

                    HolidayData newHoliday = new HolidayData();
                    newHoliday.Load(section);
                    dayCount++;
                    db.HOLIDAYS.Add(newHoliday);
                }
            }
        }

        #endregion
    }
}
