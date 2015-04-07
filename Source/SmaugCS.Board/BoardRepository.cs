using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Infrastructure.Data;
using SmaugCS.Logging;

namespace SmaugCS.Board
{
    public class BoardRepository : IBoardRepository
    {
        public IEnumerable<BoardData> Boards { get; private set; }
        private readonly ILogManager _logManager;
        private readonly IRepository _repository;

        public BoardRepository(ILogManager logManager, IRepository repository)
        {
            Boards = new List<BoardData>();
            _logManager = logManager;
            _repository = repository;
        }

        public void Add(BoardData board)
        {
            Boards.ToList().Add(board);
        }

        public void Load()
        {
            try
            {
                foreach (var b in _repository.GetQuery<DAL.Models.Board>())
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

                    foreach (var note in _repository.GetQuery<DAL.Models.Note>().Where(x => x.BoardId == newBoard.Id))
                    {
                        var newNote = new NoteData(note.Id)
                        {
                            Sender = note.Sender,
                            DateSent = note.DateSent,
                            RecipientList = note.RecipientList,
                            Subject = note.Subject,
                            IsPoll = note.IsPoll,
                            Text = note.Text
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
                _repository.UnitOfWork.BeginTransaction();
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
                    _repository.Attach(boardToSave);

                    foreach (var note in board.Notes.Where(y => !y.Saved).ToList())
                    {
                        var noteToSave = new DAL.Models.Note
                        {
                            BoardId = boardToSave.Id,
                            DateSent = note.DateSent,
                            IsPoll = note.IsPoll,
                            RecipientList = note.RecipientList,
                            Sender = note.Sender,
                            Subject = note.Subject,
                            Text = note.Text
                        };
                        note.Saved = true;
                        _repository.Attach(noteToSave);
                    }
                }
                _repository.UnitOfWork.CommitTransaction();
            }
            catch (DbException ex)
            {
                _repository.UnitOfWork.RollBackTransaction();
                _logManager.Error(ex);
            }
        }
    }
}
