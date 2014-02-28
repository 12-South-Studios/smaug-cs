using System;
using System.IO;
using NUnit.Framework;

namespace Realm.Library.Common.Test
{
    [TestFixture]
    public class TextWriterProxyTests
    {
        [Test]
        public void WriteTest()
        {
            TextWriterProxy proxy = new TextWriterProxy(new StringWriter());

            proxy.Write("test");
        }

        [Test]
        public void Write_1Arg_Test()
        {
            TextWriterProxy proxy = new TextWriterProxy(new StringWriter());

            proxy.Write("{0} test", 1);
        }

        [Test]
        public void Write_2Args_Test()
        {
            TextWriterProxy proxy = new TextWriterProxy(new StringWriter());

            proxy.Write("{0} {1} tests", 2, "big");
        }

        [Test]
        public void Write_3Args_Test()
        {
            TextWriterProxy proxy = new TextWriterProxy(new StringWriter());

            proxy.Write("{0} {1} {2} tests", 3, "super", "huge");
        }

        [Test]
        public void Write_ArrayArgs_Test()
        {
            TextWriterProxy proxy = new TextWriterProxy(new StringWriter());

            proxy.Write("{0} {1} {2} tests of {3} {4}", new object[] {4, "super", "huge", "an", "array"});
        }
    }
}
