using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Logging;

namespace SmaugCS.Board
{
    public class BoardRepository : IBoardRepository
    {
        public IEnumerable<BoardData> Boards { get; private set; }
        private readonly ILogManager _logManager;
        private readonly ISmaugDbContext _dbContext;

        public BoardRepository(ILogManager logManager, ISmaugDbContext dbContext)
        {
            Boards = new List<BoardData>();
            _logManager = logManager;
            _dbContext = dbContext;
        }

        public void Add(BoardData board)
        {
            Boards.ToList().Add(board);
        }

        public void Load()
        {
            try
            {
                foreach (var b in _dbContext.Boards)
                {
                    var newBoard = new BoardData(b.Id, b.BoardType)
                    {
                        Name = b.Name,
                        ReadGroup = b.ReadGroup,
                        PostGroup = b.PostGroup,
                        ExtraReaders = b.ExtraReaders,
                        ExtraRemovers = b.ExtraRemovers,
                        OTakeMessage = b.OTakeMessage,
                        OPostMessage = b.OTakeMessage,
                        ORemoveMessage = b.ORemoveMessage,
                        OCopyMessage = b.OCopyMessage,
                        OListMessage = b.OListMessage,
                        PostMessage = b.PostMessage,
                        OReadMessage = b.OReadMessage,
                        MinimumReadLevel = b.MinimumReadLevel,
                        MinimumPostLevel = b.MinimumPostLevel,
                        MinimumRemoveLevel = b.MinimumRemoveLevel,
                        MaximumPosts = b.MaximumPosts,
                        BoardObjectId = b.BoardObjectId
                    };
                    Boards.ToList().Add(newBoard);

                    foreach (var n in b.Notes)
                    {
                        var newNote = new NoteData(n.Id)
                        {
                            Sender = n.Sender,
                            DateSent = n.DateSent,
                            RecipientList = n.RecipientList,
                            Subject = n.Subject,
                            IsPoll = n.IsPoll,
                            Text = n.Text
                        };
                        newBoard.Notes.ToList().Add(newNote);
                    }
                }
                _logManager.Boot("Loaded {0} Boards", Boards.Count());
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
            }
        }

        public void Save()
        {
            try
            {
                foreach (var board in Boards.Where(x => !x.Saved).ToList())
                {
                    var boardToSave = _dbContext.Boards.Create();
                    boardToSave.Name = board.Name;
                    boardToSave.ReadGroup = board.ReadGroup;
                    boardToSave.PostGroup = board.PostGroup;
                    boardToSave.ExtraReaders = board.ExtraReaders;
                    boardToSave.ExtraRemovers = board.ExtraRemovers;
                    boardToSave.OTakeMessage = board.OTakeMessage;
                    boardToSave.OPostMessage = board.OPostMessage;
                    boardToSave.ORemoveMessage = board.ORemoveMessage;
                    boardToSave.OCopyMessage = board.OCopyMessage;
                    boardToSave.OListMessage = board.OListMessage;
                    boardToSave.PostMessage = board.PostMessage;
                    boardToSave.OReadMessage = board.ORemoveMessage;
                    boardToSave.MinimumReadLevel = board.MinimumReadLevel;
                    boardToSave.MinimumPostLevel = board.MinimumPostLevel;
                    boardToSave.MinimumRemoveLevel = board.MinimumRemoveLevel;
                    boardToSave.MaximumPosts = board.MaximumPosts;
                    boardToSave.BoardObjectId = board.BoardObjectId;
                    board.Saved = true;

                    foreach (var note in board.Notes.Where(y => !y.Saved).ToList())
                    {
                        var noteToSave = _dbContext.Notes.Create();
                        noteToSave.BoardId = boardToSave.Id;
                        noteToSave.DateSent = note.DateSent;
                        noteToSave.IsPoll = note.IsPoll;
                        noteToSave.RecipientList = note.RecipientList;
                        noteToSave.Sender = note.Sender;
                        noteToSave.Subject = note.Subject;
                        noteToSave.Text = note.Text;
                        note.Saved = true;
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
            }
        }
    }
}
