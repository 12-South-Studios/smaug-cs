using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realm.Library.Common.Objects;
using SmallDBConnectivity;
using SmaugCS.Common.Database;
using SmaugCS.Logging;

namespace SmaugCS.Board
{
    public sealed class BoardManager : GameSingleton
    {
        private static BoardManager _instance;
        private static readonly object Padlock = new object();

        private readonly List<BoardData> _boards;
        private readonly List<ProjectData> _projects;
        private readonly SqlRepository _repository;
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
            _repository.AddSql("BoardGetAll", SqlProcedureStatics.BoardGetAll);

            _logManager = logManager;
            _smallDb = smallDb;
            _connection = connection;
        }

        [ExcludeFromCodeCoverage]
        public void LoadBoards()
        {
            try
            {
                List<BoardData> boards = _smallDb.ExecuteQuery(_connection,
                                                            _repository.GetSql(SqlProcedureStatics.BoardGetAll),
                                                            TranslateBoardData);

                boards.ForEach(board => _boards.Add(board));
                _logManager.BootLog("Loaded {0} Boards", _boards.Count);
            }
            catch (Exception ex)
            {
                _logManager.Error(ex);
            }
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
