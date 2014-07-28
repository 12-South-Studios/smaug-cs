using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Managers;

namespace SmaugCS.Loaders
{
    public abstract class AreaLoader
    {
        public string AreaName { get; private set; }
        public bool BootDb { get; private set; }
        public string FilePath { get; private set; }

        protected AreaLoader(string areaName, bool bootDb)
        {
            AreaName = areaName;
            BootDb = bootDb;
            FilePath = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Area) + AreaName;
        }

        public abstract AreaData LoadArea(AreaData area);

        protected AreaData CreateArea()
        {
            AreaData newArea = new AreaData(DatabaseManager.Instance.AREAS.Count + db.BUILD_AREAS.Count + 1, AreaName)
            {
                Age = 15,
                HighSoftRange = LevelConstants.MaxLevel,
                HighHardRange = LevelConstants.MaxLevel,
                Version = 1,
                Filename = AreaName
            };

            DatabaseManager.Instance.AREAS.Add(newArea.ID, newArea);
            ++db.TopArea;
            return newArea;
        }
    }
}
