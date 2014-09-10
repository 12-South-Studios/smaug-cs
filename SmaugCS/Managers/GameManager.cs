using System;
using Ninject;
using Realm.Library.NCalcExt;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Interfaces;

namespace SmaugCS.Managers
{
    public sealed class GameManager : IGameManager
    {
        private static IDatabaseManager _dbManager;

        public GameManager(IDatabaseManager databaseManager)
        {
            _dbManager = databaseManager;

            ExpParser = new ExpressionParser(ExpressionTableInitializer.GetExpressionTable());
            SystemData = new SystemData();
        }

        public static IGameManager Instance
        {
            get { return Program.Kernel.Get<IGameManager>(); }
        }

        public SystemData SystemData { get; private set; }

        public TimeInfoData GameTime { get; private set; }
        public void SetGameTime(TimeInfoData gameTime)
        {
            if (GameTime == null)
                GameTime = gameTime;
        }

        public ExpressionParser ExpParser { get; private set; }

        public static CharacterInstance CurrentCharacter { get; set; }

        public void DoLoop()
        {
            InitializeResets();

            // TODO Loop here
        }

        private void InitializeResets()
        {
            foreach (AreaData area in _dbManager.AREAS.Values)
                area.OnStartup(this, new EventArgs());
        }
    }
}
