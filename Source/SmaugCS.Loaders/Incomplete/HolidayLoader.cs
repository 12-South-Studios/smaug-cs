using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Loaders.Loaders;

namespace SmaugCS.Loaders
{
    public class HolidayLoader : BaseLoader
    {
        #region Overrides of ListLoader

        public HolidayLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

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
