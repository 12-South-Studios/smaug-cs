using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Moq;
using Ninject;
using NUnit.Framework;
using Realm.Library.Common;
using SmallDBConnectivity;
using SmaugCS.Logging;

namespace SmaugCS.Ban.Tests
{
    [TestFixture]
    public class BanManagerTests
    {
        private static IBanManager _banManager = BanManager.Instance;

        private static BanData GetBan()
        {
            return new BanData(1, BanTypes.Race)
                {
                    Name = "Tester",
                    Note = "Tester was very bad",
                    BannedBy = "BigWig",
                    BannedOn = DateTime.Now,
                    Duration = Int32.MaxValue
                };
        }

        private class FakeTransaction : IDbTransaction
        {
            public void Dispose() {}
            public void Commit() {}
            public void Rollback() {}
            public IDbConnection Connection { get; set; }
            public IsolationLevel IsolationLevel { get; set; }
        }

        [SetUp]
        private void OnSetup()
        {
            var mockLogger = new Mock<ILogManager>();
            mockLogger.Setup(x => x.Error(It.IsAny<Exception>()));

            var mockDb = new Mock<ISmallDb>();
            mockDb.Setup(x => x.ExecuteNonQuery(It.IsAny<IDbConnection>(), It.IsAny<string>(),
                It.IsAny<IEnumerable<IDataParameter>>(), It.IsAny<string>(), It.IsAny<bool>()));

            var mockConnection = new Mock<IDbConnection>();
            mockConnection.Setup(x => x.BeginTransaction()).Returns(new FakeTransaction());

            var mockKernel = new Mock<IKernel>();
            var mockTimer = new Mock<ITimer>();

            _banManager = new BanManager(mockLogger.Object, mockDb.Object, mockConnection.Object, mockKernel.Object,
                mockTimer.Object);
        }

        [TearDown]
        private void OnTeardown()
        {
            _banManager.ClearBans();
            _banManager = null;
        }

        [Test]
        public void CreateSqlParametersTest()
        {
            Assert.That(BanManager.CreateSqlParameters(GetBan()).Count(), Is.EqualTo(6));
        }

        [Test]
        public void AddBan_Empty_Test()
        {
            var ban = GetBan();

            Assert.That(_banManager.AddBan(ban), Is.True);
        }

        [Test]
        public void AddBan_NotEmpty_Test()
        {
            var ban = GetBan();

            _banManager.AddBan(ban);

            Assert.That(_banManager.AddBan(ban), Is.False); 
        }

        [Test]
        public void RemoveBan_Match_Test()
        {
            var ban = GetBan();

            _banManager.AddBan(ban);

            Assert.That(_banManager.RemoveBan(1), Is.True); 
        }

        [Test]
        public void RemoveBan_NoMatch_Test()
        {
            var ban = GetBan();

            _banManager.AddBan(ban);

            Assert.That(_banManager.RemoveBan(2), Is.False);
        }

        [Test]
        public void CheckTotalBans_NoMatch_Test()
        {
            BanManager mgr = (BanManager)_banManager;
            mgr.AddBan(new BanData(1, BanTypes.Site) {Level = 5});
            mgr.AddBan(new BanData(2, BanTypes.Site) {Level = 10});

            Assert.That(mgr.CheckTotalBans("127.0.0.1", 50), Is.False);
        }

        [Test]
        public void CheckTotalBans_PrefixAndSuffixHostMatch_Test()
        {
            BanManager mgr = (BanManager)_banManager;
            mgr.AddBan(new BanData(1, BanTypes.Site)
            {
                Prefix = true,
                Suffix = true,
                Name = "test.whatever.com",
                Level = 50
            });

            Assert.That(mgr.CheckTotalBans("test.whatever.com", 50), Is.True);
        }
    }
}
