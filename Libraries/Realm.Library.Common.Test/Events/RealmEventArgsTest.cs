﻿using System;
using NUnit.Framework;

namespace Realm.Library.Common.Test.Events
{
    [TestFixture]
    public class RealmEventArgsTest
    {
        [Test]
        [Category("Event Tests")]
        public void RealmEventArgsConstructorTest()
        {
            var result = new RealmEventArgs();

            Assert.That(result.Data, Is.Not.Null);
            Assert.IsTrue(typeof(EventTable) == result.Data.GetType());
        }

        [TestCase(false, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(true)]
        [Category("Event Tests")]
        public void RealmEventArgsOverloadConstructor1Test(bool createEventTable)
        {
            var result = new RealmEventArgs(createEventTable ? new EventTable() : null);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.Not.Null);
        }

        [TestCase("", ExpectedException = typeof(ArgumentNullException))]
        [TestCase("test")]
        [Category("Event Tests")]
        public void RealmEventArgsOverloadConstructor2Test(string arg)
        {
            var result = new RealmEventArgs(arg);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Type, Is.EqualTo(arg));
        }

        [Test]
        [Category("Event Tests")]
        public void GetValueEmptyKeyTest()
        {
            var args = new RealmEventArgs();

            Assert.Throws<ArgumentNullException>(() => args.GetValue(string.Empty),
                                                 "Unit test expected an ArgumentNullException to be thrown");
        }

        [Test]
        [Category("Event Tests")]
        public void GetValueNullTableTest()
        {
            var args = new RealmEventArgs("test");

            Assert.That(args.GetValue("key"), Is.Null);
        }

        [Test]
        [Category("Event Tests")]
        public void GetValueNoKeyTest()
        {
            var table = new EventTable { { "key", 25 } };

            var args = new RealmEventArgs(table);

            Assert.That(args.GetValue("key2"), Is.Null);
        }

        [Test]
        [Category("Event Tests")]
        public void GetValueSuccessTest()
        {
            const int value = 25;
            var table = new EventTable { { "key", value } };

            var args = new RealmEventArgs(table);

            Assert.That(args.GetValue("key"), Is.EqualTo(value));
        }

        [Test]
        [Category("Event Tests")]
        public void HasValueNullTableTest()
        {
            var args = new RealmEventArgs("test");

            Assert.That(args.HasValue("key"), Is.False);
        }

        [Test]
        [Category("Event Tests")]
        public void HasValueSuccessTest()
        {
            const int value = 25;
            var table = new EventTable { { "key", value } };

            var args = new RealmEventArgs(table);

            Assert.That(args.HasValue("key"), Is.True);
        }
    }
}
