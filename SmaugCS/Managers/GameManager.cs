﻿using Ninject;
using Realm.Library.NCalcExt;
using SmaugCS.Data;
using SmaugCS.Interfaces;

namespace SmaugCS.Managers
{
    public sealed class GameManager : IGameManager
    {
        public GameManager()
        {
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

        }
    }
}
