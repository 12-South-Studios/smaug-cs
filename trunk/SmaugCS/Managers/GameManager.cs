using System.Text.RegularExpressions;
using NCalc;
using Realm.Library.Common.Objects;
using Realm.Library.NCalcExt;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Managers
{
    public sealed class GameManager : GameSingleton, IGameManager
    {
        private static GameManager _instance;
        private static readonly object Padlock = new object();

        private GameManager()
        {
            ExpParser = new ExpressionParser(ExpressionTableInitializer.GetExpressionTable());
            SystemData = new SystemData();
        }

        public static GameManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new GameManager());
                }
            }
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

        public void Initialize(bool p)
        {

        }

        public void DoLoop()
        {

        }
    }
}
