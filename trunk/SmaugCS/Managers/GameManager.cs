using System.Text.RegularExpressions;
using NCalc;
using Realm.Library.Common.Objects;
using Realm.Library.NCalcExt;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.Managers
{
    public sealed class GameManager : GameSingleton
    {
        private static GameManager _instance;
        private static readonly object Padlock = new object();

        private GameManager()
        {
            ExpParser = new ExpressionParser(ExpressionTableInitializer.GetExpressionTable());
        }

        /// <summary>
        ///
        /// </summary>
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

        public ExpressionParser ExpParser { get; private set; }

        public static CharacterInstance CurrentCharacter { get; set; }

        public void Initialize(bool p)
        {
            throw new System.NotImplementedException();
        }

        public void DoLoop()
        {
            throw new System.NotImplementedException();
        }
    }
}
