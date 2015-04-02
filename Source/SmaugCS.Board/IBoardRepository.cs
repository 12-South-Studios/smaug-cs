using System.Collections.Generic;

namespace SmaugCS.Board
{
    public interface IBoardRepository
    {
        void Add(BoardData board);
        void Load();
        void Save();

        IEnumerable<BoardData> Boards { get; }
    }
}
