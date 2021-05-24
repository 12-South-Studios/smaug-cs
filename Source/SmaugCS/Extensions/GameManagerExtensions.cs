using SmaugCS.Data.Interfaces;

namespace SmaugCS
{
    public static class GameManagerExtensions
    {
        public static int GetSaveFlags(this IGameManager gameManager) => gameManager.SystemData.SaveFlags;
    }
}
