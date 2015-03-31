using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Realm.Library.SmallDb;
using SmaugCS.Logging;

namespace SmaugCS.Auction
{
    public class AuctionRepository : IAuctionRepository
    {
        public IEnumerable<AuctionHistory> History { get; private set; }
        private readonly ILogManager _logManager;
        private readonly ISmallDb _smallDb;
        private readonly IDbConnection _connection;

        public AuctionRepository(ILogManager logManager, ISmallDb smallDb, IDbConnection connection)
        {
            History = new List<AuctionHistory>();
            _logManager = logManager;
            _smallDb = smallDb;
            _connection = connection;
        }

        public void Add(AuctionData auction)
        {
            AuctionHistory history = new AuctionHistory
            {
                BuyerName = auction.Buyer.Name,
                SellerName = auction.Seller.Name,
                ItemForSale = auction.ItemForSale.ObjectIndex.ID,
                SoldFor = auction.BidAmount,
                SoldOn = DateTime.UtcNow,
                Saved = false
            };
            History.ToList().Add(history);
        }

        public void Load()
        {
            try
            {
                List<AuctionHistory> auctions = _smallDb.ExecuteQuery(_connection, SqlProcedureStatics.AuctionGetAll, TranslateAuctionHistory);

                auctions.ForEach(x => History.ToList().Add(x));
                _logManager.Boot("Loaded {0} Auctions", History.Count());
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
            }
        }

        [ExcludeFromCodeCoverage]
        private static List<AuctionHistory> TranslateAuctionHistory(IDataReader reader)
        {
            List<AuctionHistory> auctions = new List<AuctionHistory>();
            using (DataTable dt = new DataTable())
            {
                dt.Load(reader);
                auctions.AddRange(from DataRow row in dt.Rows select AuctionHistory.Translate(row));
            }

            return auctions;
        }

        [ExcludeFromCodeCoverage]
        public void Save()
        {
            IDbTransaction transaction = null;
            try
            {
                transaction = _connection.BeginTransaction();
                foreach (AuctionHistory history in History.Where(x => !x.Saved).ToList())
                {
                    _smallDb.ExecuteNonQuery(_connection, SqlProcedureStatics.AuctionSave, CreateSqlParameters(history));
                    history.Saved = true;
                }
                transaction.Commit();
            }
            catch (DbException ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                _logManager.Error(ex);
            }
        }

        internal static IEnumerable<SqlParameter> CreateSqlParameters(AuctionHistory auction)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@buyerName", auction.BuyerName),
                    new SqlParameter("@sellerName", auction.SellerName),
                    new SqlParameter("@soldOn", auction.SoldOn),
                    new SqlParameter("@soldFor", auction.SoldFor),
                    new SqlParameter("@itemSoldID", auction.ItemForSale)
                };

            return parameters;
        }
    }
}
