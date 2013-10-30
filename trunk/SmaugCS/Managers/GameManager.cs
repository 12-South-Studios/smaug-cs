using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Realm.Library.Common.Objects;
using Realm.Library.NCalcExt;

namespace SmaugCS.Managers
{
    public sealed class GameManager : GameSingleton 
    {
        private static GameManager _instance;
        private static readonly object Padlock = new object();

        private GameManager()
        {
            ExpParser = new ExpressionParser(GetExpressionTable());
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

        private ExpressionTable GetExpressionTable()
        {
            return new ExpressionTable();
        }
    }
}
