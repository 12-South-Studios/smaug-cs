using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;

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
            //using (var proxy = new TextWriterProxy(new StreamWriter(Filename)))
            //{
            //    foreach (var day in db.HOLIDAYS)
            //    {
            //        proxy.Write("#HOLIDAY\n");
            //        proxy.Write("Name     {0}~\n", day.Name);
            //        proxy.Write("Announce {0}~\n", day.Announce);
            //        proxy.Write("Month    {0}\n", day.Month);
            //        proxy.Write("Day      {0}\n", day.Day);
            //        proxy.Write("End\n\n");
            //    }
            //    proxy.Write("#END\n");
            //}
        }

        public override void Load()
        {
            //using (var proxy = new TextReaderProxy(new StreamReader(Filename)))
            //{
            //    var dayCount = 0;
            //    IEnumerable<TextSection> sections = proxy.ReadSections(new[] { "#HOLIDAY" }, new[] { "*" }, null, "END");
            //    foreach (var section in sections)
            //    {
            //        if (dayCount >= GameManager.Instance.SystemData.MaxHolidays)
            //        {
            //            LogManager.Instance.Bug("Exceeded maximum holidays {0}", dayCount);
            //            return;
            //        }

            //        var newHoliday = new HolidayData();
            //        newHoliday.Load(section);
            //        dayCount++;
            //        db.HOLIDAYS.Add(newHoliday);
            //    }
            //}
        }

        #endregion
    }
}
