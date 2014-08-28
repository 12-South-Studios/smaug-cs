using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Timers;
using Ninject;
using Realm.Library.Common;
using SmallDBConnectivity;
using SmaugCS.Data;
using SmaugCS.Logging;

namespace SmaugCS.Auction
{
    public sealed class AuctionManager : IAuctionManager
    {
        private readonly AuctionData _auction;
        private readonly AuctionHistoryRepository _repository;
        private readonly ILogManager _logManager;
        private readonly ISmallDb _smallDb;
        private readonly IDbConnection _connection;
        private static IKernel _kernel;
        private static ITimer _timer;

        public AuctionData Auction { get; set; }

        public AuctionManager(ILogManager logManager, ISmallDb smallDb, IDbConnection connection, IKernel kernel,
            ITimer timer)
        {
            _logManager = logManager;
            _smallDb = smallDb;
            _connection = connection;
            _kernel = kernel;

            _timer = timer;

            if (_timer.Interval <= 0)
                _timer.Interval = 30000;

            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();

            _repository = new AuctionHistoryRepository(_logManager, _smallDb, _connection);
        }

        ~AuctionManager()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            if (_connection != null)
                _connection.Dispose();
        }

        public static IAuctionManager Instance
        {
            get { return _kernel.Get<IAuctionManager>(); }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            _repository.Load();
        }

        public void Save()
        {
            _repository.Save();
        }
    }
}
