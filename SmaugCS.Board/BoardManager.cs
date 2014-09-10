using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Ninject;
using SmallDBConnectivity;
using SmaugCS.Logging;

namespace SmaugCS.Board
{
    public sealed class BoardManager : IBoardManager
    {
        private readonly List<BoardData> _boards;
        private readonly ILogManager _logManager;
        private readonly ISmallDb _smallDb;
        private readonly IDbConnection _connection;
        private static IKernel _kernel;

        public BoardManager(ILogManager logManager, ISmallDb smallDb, IDbConnection connection, IKernel kernel)
        {
            _logManager = logManager;
            _smallDb = smallDb;
            _connection = connection;
            _kernel = kernel;

            _boards = new List<BoardData>();
        }

        public static IBoardManager Instance
        {
            get { return _kernel.Get<IBoardManager>(); }
        }

        public void Initialize()
        {
            try
            {
                List<BoardData> boards = _smallDb.ExecuteQuery(_connection, SqlProcedureStatics.BoardGetAll, TranslateBoardData);

                foreach (BoardData board in boards)
                {
                    _boards.Add(board);
                    LoadNotesForBoard(board);
                }
                _logManager.Boot("Loaded {0} Boards", _boards.Count);
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
            }
        }

        [ExcludeFromCodeCoverage]
        private void LoadNotesForBoard(BoardData board)
        {
            List<NoteData> notes = _smallDb.ExecuteQuery(_connection, SqlProcedureStatics.BoardGetNotes,
                                                         TranslateNoteData, new List<SqlParameter>
                                                             {
                                                                 new SqlParameter("@boardId", board.Id)
                                                             });
            board.AddNotes(notes);
        }

        [ExcludeFromCodeCoverage]
        private static List<NoteData> TranslateNoteData(IDataReader reader)
        {
            List<NoteData> notes = new List<NoteData>();
            using (DataTable dt = new DataTable())
            {
                dt.Load(reader);
                notes.AddRange(from DataRow row in dt.Rows select NoteData.Translate(row));
            }

            return notes;
        }
            
        [ExcludeFromCodeCoverage]
        private static List<BoardData> TranslateBoardData(IDataReader reader)
        {
            List<BoardData> boards = new List<BoardData>();
            using (DataTable dt = new DataTable())
            {
                dt.Load(reader);
                boards.AddRange(from DataRow row in dt.Rows select BoardData.Translate(row));
            }

            return boards;
        }

    }
}
