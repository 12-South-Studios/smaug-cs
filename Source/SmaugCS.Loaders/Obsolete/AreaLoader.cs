using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Logging;

namespace SmaugCS.Loaders.Obsolete
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
           // FilePath = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Area) + AreaName;
        }

        public abstract AreaData LoadArea(ILogManager logManager, AreaData area);

        protected AreaData CreateArea()
        {
            /*AreaData newArea = new AreaData(RepositoryManager.Instance.AREAS.Count + db.BUILD_AREAS.Count + 1, AreaName)
            {
                Age = 15,
                HighSoftRange = LevelConstants.MaxLevel,
                HighHardRange = LevelConstants.MaxLevel,
                Version = 1,
                Filename = AreaName
            };

            RepositoryManager.Instance.AREAS.Add(newArea.ID, newArea);
            ++db.TopArea;
            return newArea;*/
            return null;
        }
    }
}
