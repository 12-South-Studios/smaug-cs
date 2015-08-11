using Realm.Library.NCalcExt;
using SmaugCS.Data;

namespace SmaugCS.Interfaces
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
