using FakeItEasy;
using Ninject;
using Realm.Library.Common;
using SmaugCS.Common.Enumerations;
using SmaugCS.Logging;
using System;
using Xunit;

namespace SmaugCS.Ban.Tests
{

    public class BanManagerTests
    {
        private static IBanManager _banManager;

        public BanManagerTests()
        {
            var mockLogger = A.Fake<ILogManager>();
            A.CallTo(() => mockLogger.Error(A<Exception>.Ignored));

            var mockKernel = A.Fake<IKernel>();
            var mockTimer = A.Fake<ITimer>();
            var mockRepo = A.Fake<IBanRepository>();

            _banManager = new BanManager(mockKernel, mockTimer, mockLogger, mockRepo);
        }

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


        ~BanManagerTests()
        {
            _banManager.ClearBans();
            _banManager = null;
        }

        [Fact]
        public void AddBan_Empty_Test()
        {
            var ban = GetBan();

            try
            {
                _banManager.Repository.Add(ban);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message + " was thrown!");
            }
        }

        //[Fact]
        //public void AddBan_NotEmpty_Test()
        //{
        //    var ban = GetBan();

        //    _banManager.AddBan(ban);

        //    _banManager.AddBan(ban).Should().BeFalse(); 
        //}

        //[Fact]
        //public void RemoveBan_Match_Test()
        //{
        //    var ban = GetBan();

        //    _banManager.AddBan(ban);

        //    _banManager.RemoveBan(1).Should().BeTrue(); 
        //}

        //[Fact]
        //public void RemoveBan_NoMatch_Test()
        //{
        //    var ban = GetBan();

        //    _banManager.AddBan(ban);

        //    _banManager.RemoveBan(2).Should().BeFalse();
        //}

        //[Fact]
        //public void CheckTotalBans_NoMatch_Test()
        //{
        //    BanManager mgr = (BanManager)_banManager;
        //    mgr.Repository.Add(new BanData(1, BanTypes.Site) {Level = 5});
        //    mgr.AddBan(new BanData(2, BanTypes.Site) {Level = 10});

        //    mgr.CheckTotalBans("127.0.0.1", 50).Should().BeFalse();
        //}

        //[Fact]
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

        //    mgr.CheckTotalBans("test.whatever.com", 50).Should().BeTrue();
        //}
    }
}
