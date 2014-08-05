using System;
using System.Data;
using NUnit.Framework;

namespace SmaugCS.Ban.Tests
{
    [TestFixture]
    public class BanDataTests
    {
        private static DataTable BuildDataTable()
        {
            DataTable table = new DataTable("BanData");
            table.Columns.Add("BanId");
            table.Columns.Add("BanTypeName");
            table.Columns.Add("Name");
            table.Columns.Add("Note");
            table.Columns.Add("BannedBy");
            table.Columns.Add("BannedOn");
            table.Columns.Add("Duration");
            table.Columns.Add("Level");
            table.Columns.Add("Warn");
            table.Columns.Add("Prefix");
            table.Columns.Add("Suffix");
            return table;
        }

        [Test]
        public void Translate_NoNulls_Test()
        {
            var table = BuildDataTable();

            var row = table.NewRow();
            row["BanId"] = 1;
            row["Name"] = "TestBan";
            row["BanTypeName"] = BanTypes.Site;
            row["Note"] = "This is a test ban";
            row["BannedBy"] = "Admin";
            row["BannedOn"] = DateTime.Parse("1/1/2014 8:00:00 AM");
            row["Duration"] = 3600;
            row["Level"] = 1;
            row["Warn"] = true;
            row["Prefix"] = true;
            row["Suffix"] = true;

            var ban = BanData.Translate(row);

            Assert.That(ban.Id, Is.EqualTo(1));
            Assert.That(ban.Name, Is.EqualTo("TestBan"));
            Assert.That(ban.Type, Is.EqualTo(BanTypes.Site));
            Assert.That(ban.Note, Is.EqualTo("This is a test ban"));
            Assert.That(ban.BannedBy, Is.EqualTo("Admin"));
            Assert.That(ban.BannedOn, Is.EqualTo(DateTime.Parse("1/1/2014 8:00:00 AM")));
            Assert.That(ban.Duration, Is.EqualTo(3600));
            Assert.That(ban.Level, Is.EqualTo(1));
            Assert.That(ban.Warn, Is.True);
            Assert.That(ban.Prefix, Is.True);
            Assert.That(ban.Suffix, Is.True);
        }

        [Test]
        public void Translate_Nulls_Test()
        {
            var table = BuildDataTable();

            var row = table.NewRow();
            row["BanId"] = 1;
            row["Name"] = "TestBan";
            row["BanTypeName"] = BanTypes.Site;
            row["BannedBy"] = "Admin";
            row["BannedOn"] = DateTime.Parse("1/1/2014 8:00:00 AM");
            row["Duration"] = 3600;

            var ban = BanData.Translate(row);

            Assert.That(ban.Id, Is.EqualTo(1));
            Assert.That(ban.Name, Is.EqualTo("TestBan"));
            Assert.That(ban.Type, Is.EqualTo(BanTypes.Site));
            Assert.That(ban.Note, Is.EqualTo(string.Empty));
            Assert.That(ban.BannedBy, Is.EqualTo("Admin"));
            Assert.That(ban.BannedOn, Is.EqualTo(DateTime.Parse("1/1/2014 8:00:00 AM")));
            Assert.That(ban.Duration, Is.EqualTo(3600));
            Assert.That(ban.Level, Is.EqualTo(0));
            Assert.That(ban.Warn, Is.False);
            Assert.That(ban.Prefix, Is.False);
            Assert.That(ban.Suffix, Is.False);
        }

        [TestCase(172800, false)]
        [TestCase(3600, true)]
        public void IsExpired_Test(int duration, bool expected)
        {
            var ban = new BanData(1, BanTypes.Site);
            ban.BannedOn = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            ban.Duration = duration;  // two days

            Assert.That(ban.IsExpired(), Is.EqualTo(expected));
        }

        [Test]
        public void UnbanDate_Never_Test()
        {
            var ban = new BanData(1, BanTypes.Site);
            ban.BannedOn = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            ban.Duration = 0;

            Assert.That(ban.UnbanDate, Is.EqualTo(DateTime.MaxValue));
            Assert.That(ban.IsExpired(), Is.False);
        }
    }
}
