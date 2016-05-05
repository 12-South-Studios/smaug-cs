using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Logging;

namespace SmaugCS.Board
{
    public class BoardRepository : IBoardRepository
    {
        public IEnumerable<BoardData> Boards { get; }
        private readonly ILogManager _logManager;
        private readonly ISmaugDbContext _dbContext;

        public BoardRepository(ILogManager logManager, ISmaugDbContext dbContext)
        {
            Boards = new List<BoardData>();
            _logManager = logManager;
            _dbContext = dbContext;
        }

        public void Add(BoardData board) => Boards.ToList().Add(board);

        public void Load()
        {
            try
            {
                if (!_dbContext.Boards.Any()) return;

                foreach (DAL.Models.Board board in _dbContext.Boards)
                {
                    var newBoard = new BoardData(board.Id, board.BoardType)
                    {
                        Name = board.Name,
                        ReadGroup = board.ReadGroup,
                        PostGroup = board.PostGroup,
                        ExtraReaders = board.ExtraReaders,
                        ExtraRemovers = board.ExtraRemovers,
                        OTakeMessage = board.OTakeMessage,
                        OPostMessage = board.OTakeMessage,
                        ORemoveMessage = board.ORemoveMessage,
                        OCopyMessage = board.OCopyMessage,
                        OListMessage = board.OListMessage,
                        PostMessage = board.PostMessage,
                        OReadMessage = board.OReadMessage,
                        MinimumReadLevel = board.MinimumReadLevel,
                        MinimumPostLevel = board.MinimumPostLevel,
                        MinimumRemoveLevel = board.MinimumRemoveLevel,
                        MaximumPosts = board.MaximumPosts,
                        BoardObjectId = board.BoardObjectId
                    };
                    Boards.ToList().Add(newBoard);

                    foreach (var newNote in board.Notes.Select(note => new NoteData(note.Id)
                    {
                        Sender = note.Sender,
                        DateSent = note.DateSent,
                        RecipientList = note.RecipientList,
                        Subject = note.Subject,
                        IsPoll = note.IsPoll,
                        Text = note.Text
                    }))
                    {
                        newBoard.Notes.ToList().Add(newNote);
                    }
                }
                _logManager.Boot("Loaded {0} Boards", Boards.Count());
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
                throw;
            }
        }

        public void Save()
        {
            try
            {
                foreach (var board in Boards.Where(x => !x.Saved).ToList())
                {
                    var boardToSave = new DAL.Models.Board
                    {
                        Name = board.Name,
                        ReadGroup = board.ReadGroup,
                        PostGroup = board.PostGroup,
                        ExtraReaders = board.ExtraReaders,
                        ExtraRemovers = board.ExtraRemovers,
                        OTakeMessage = board.OTakeMessage,
                        OPostMessage = board.OPostMessage,
                        ORemoveMessage = board.ORemoveMessage,
                        OCopyMessage = board.OCopyMessage,
                        OListMessage = board.OListMessage,
                        PostMessage = board.PostMessage,
                        OReadMessage = board.ORemoveMessage,
                        MinimumReadLevel = board.MinimumReadLevel,
                        MinimumPostLevel = board.MinimumPostLevel,
                        MinimumRemoveLevel = board.MinimumRemoveLevel,
                        MaximumPosts = board.MaximumPosts,
                        BoardObjectId = board.BoardObjectId
                    };
                    board.Saved = true;
                    _dbContext.Boards.Add(boardToSave);

                    foreach (var note in board.Notes.Where(y => !y.Saved).ToList())
                    {
                        var noteToSave = new DAL.Models.Note
                        {
                            DateSent = note.DateSent,
                            IsPoll = note.IsPoll,
                            RecipientList = note.RecipientList,
                            Sender = note.Sender,
                            Subject = note.Subject,
                            Text = note.Text
                        };
                        note.Saved = true;
                        boardToSave.Notes.Add(noteToSave);
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
                throw;
            }
        }
    }
}
