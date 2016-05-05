using Realm.Library.NCalcExt;

namespace SmaugCS.Data.Interfaces
{
    public interface IGameManager
    {
        TimeInfoData GameTime { get; }
        SystemData SystemData { get; }
        void SetGameTime(TimeInfoData gameTime);
        ExpressionParser ExpParser { get; }
        void StartMainGameLoop();
    }
}
