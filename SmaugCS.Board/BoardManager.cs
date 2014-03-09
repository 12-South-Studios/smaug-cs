using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Realm.Library.Common;
using SmallDBConnectivity;
using SmaugCS.Logging;

namespace SmaugCS.Board
{
    public sealed class BoardManager : GameSingleton
    {
        private static BoardManager _instance;
        private static readonly object Padlock = new object();

        private readonly List<BoardData> _boards;
        private readonly List<ProjectData> _projects;
        private ILogManager _logManager;
        private ISmallDb _smallDb;
        private IDbConnection _connection;

        [ExcludeFromCodeCoverage]
        private BoardManager()
        {
            _boards = new List<BoardData>();
            _projects = new List<ProjectData>();
        }

        /// <summary>
        ///
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static BoardManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new BoardManager());
                }
            }
        }

        [ExcludeFromCodeCoverage]
        public void Initialize(ILogManager logManager, ISmallDb smallDb, IDbConnection connection)
        {
            _logManager = logManager;
            _smallDb = smallDb;
            _connection = connection;
        }

        [ExcludeFromCodeCoverage]
        public void LoadBoards()
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
            catch (Exception ex)
            {
                _logManager.Error(ex);
            }
        }

        [ExcludeFromCodeCoverage]
        private void LoadNotesForBoard(BoardData board)
        {
            List<NoteData> notes = _smallDb.ExecuteQuery(_connection, SqlProcedureStatics.BoardGetNotes,
                                                         TranslateNoteData, new List<SqlParameter>()
                                                             {
                                                                 new SqlParameter("@BoardId", board.Id)
                                                             });
            board.NoteList.AddRange(notes);
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
