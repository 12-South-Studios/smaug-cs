using System;
using Moq;
using Ninject;
using NUnit.Framework;
using Realm.Library.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Logging;

namespace SmaugCS.Ban.Tests
{
    [TestFixture]
    public class BanManagerTests
    {
        private static IBanManager _banManager = BanManager.Instance;

        private static BanData GetBan()
        {
            return new BanData
                {
                    Id = 1,
                    Type = BanTypes.Race,
                    Name = "Tester",
                    Note = "Tester was very bad",
                    BannedBy = "BigWig",
                    BannedOn = DateTime.Now,
                    Duration = int.MaxValue
                };
        }

        [SetUp]
        private void OnSetup()
        {
            var mockLogger = new Mock<ILogManager>();
            mockLogger.Setup(x => x.Error(It.IsAny<Exception>()));

            var mockKernel = new Mock<IKernel>();
            var mockTimer = new Mock<ITimer>();
            var mockRepo = new Mock<IBanRepository>();

            _banManager = new BanManager(mockKernel.Object, mockTimer.Object, mockLogger.Object, mockRepo.Object);
        }

        [TearDown]
        private void OnTeardown()
        {
            _banManager.ClearBans();
            _banManager = null;
        }

        //[Test]
        //public void AddBan_Empty_Test()
        //{
        //    var ban = GetBan();

        //    Assert.That(_banManager.AddBan(ban), Is.True);
        //}

        //[Test]
        //public void AddBan_NotEmpty_Test()
        //{
        //    var ban = GetBan();

        //    _banManager.AddBan(ban);

        //    Assert.That(_banManager.AddBan(ban), Is.False); 
        //}

        //[Test]
        //public void RemoveBan_Match_Test()
        //{
        //    var ban = GetBan();

        //    _banManager.AddBan(ban);

        //    Assert.That(_banManager.RemoveBan(1), Is.True); 
        //}

        //[Test]
        //public void RemoveBan_NoMatch_Test()
        //{
        //    var ban = GetBan();

        //    _banManager.AddBan(ban);

        //    Assert.That(_banManager.RemoveBan(2), Is.False);
        //}

        //[Test]
        //public void CheckTotalBans_NoMatch_Test()
        //{
        //    BanManager mgr = (BanManager)_banManager;
        //    mgr.Repository.Add(new BanData(1, BanTypes.Site) {Level = 5});
        //    mgr.AddBan(new BanData(2, BanTypes.Site) {Level = 10});

        //    Assert.That(mgr.CheckTotalBans("127.0.0.1", 50), Is.False);
        //}

        //[Test]
        //public void CheckTotalBans_PrefixAndSuffixHostMatch_Test()
        //{
        //    BanManager mgr = (BanManager)_banManager;
        //    mgr.AddBan(new BanData(1, BanTypes.Site)
        //    {
        //        Prefix = true,
        //        Suffix = true,
        //        Name = "test.whatever.com",
        //        Level = 50
        //    });

        //    Assert.That(mgr.CheckTotalBans("test.whatever.com", 50), Is.True);
        //}
    }
}
