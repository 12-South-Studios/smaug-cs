using SmaugCS.Interfaces;

namespace SmaugCS.Extensions
{
    public static class GameManagerExtensions
    {
        public static int GetSaveFlags(this IGameManager gameManager)
        {
            return gameManager.SystemData.SaveFlags;
        }   
    }
}
