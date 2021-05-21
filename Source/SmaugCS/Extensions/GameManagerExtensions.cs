using SmaugCS.Data.Interfaces;

namespace SmaugCS.Extensions
{
    public static class GameManagerExtensions
    {
        public static int GetSaveFlags(this IGameManager gameManager) => gameManager.SystemData.SaveFlags;
    }
}
