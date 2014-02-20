using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Moq;
using NUnit.Framework;
using SmallDBConnectivity;
using SmaugCS.Logging;

namespace SmaugCS.Ban.Tests
{
    [TestFixture]
    public class BanManagerTests
    {
        private BanData GetBan()
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

        private IBanManager SetupBanManager()
        {
            var mockLogger = new Mock<ILogManager>();
            mockLogger.Setup(x => x.Error(It.IsAny<Exception>()));

            var mockDb = new Mock<ISmallDb>();
            mockDb.Setup(x => x.ExecuteNonQuery(It.IsAny<IDbConnection>(), It.IsAny<string>(),
                It.IsAny<IEnumerable<IDataParameter>>(), It.IsAny<string>(), It.IsAny<bool>()));

            var mockConnection = new Mock<IDbConnection>();

            var mgr = BanManager.Instance;
            mgr.Initialize(mockLogger.Object, mockDb.Object, mockConnection.Object);

            return mgr;
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

            IBanManager mgr = SetupBanManager();

            Assert.That(mgr.AddBan(ban), Is.True);
        }

        [Test]
        public void AddBan_NotEmpty_Test()
        {
            var ban = GetBan();

            IBanManager mgr = SetupBanManager();

            mgr.AddBan(ban);

            Assert.That(mgr.AddBan(ban), Is.False); 
        }

        [Test]
        public void RemoveBan_Match_Test()
        {
            var ban = GetBan();

            IBanManager mgr = SetupBanManager();

            mgr.AddBan(ban);

            Assert.That(mgr.RemoveBan(1), Is.True); 
        }

        [Test]
        public void RemoveBan_NoMatch_Test()
        {
            var ban = GetBan();

            IBanManager mgr = SetupBanManager();

            mgr.AddBan(ban);

            Assert.That(mgr.RemoveBan(2), Is.False);
        }
    }
}
