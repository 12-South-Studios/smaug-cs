using System;
using System.Net;
using NUnit.Framework;
using Realm.Library.Network.Mxp;

namespace Realm.Library.Network.Test
{
    [TestFixture]
    public class NetworkExtensionsTest
    {
        [Test]
        public void NetworkExtensions_MxpTag_Test()
        {
            const string test = "Test";
            Assert.That(test.MxpTag(), Is.EqualTo("\x03Test\x04"));
        }

        [Test]
        public void NetworkExtensions_MxpAmp_Test()
        {
            Assert.That(MxpExtensions.MxpAmp(), Is.EqualTo("\x06"));
        }

        [Test]
        public void NetworkExtensions_MxpMode_Test()
        {
            const int value = 6;
            Assert.That(value.MxpMode(), Is.EqualTo("\x1B[6z"));
        }

        [TestCase("100.101.102.103", 1734763876)]
        [TestCase("localhost", 16777343)]
        [TestCase("this_is_not_an_ip_address", 16777343)]
        [TestCase(null, 16777343, ExpectedException = typeof(ArgumentNullException))]
        public void ConvertToIpAddress_Test(string ipAddress, int bigEndianValue)
        {
            Assert.That(ipAddress.ConvertToIpAddress(), Is.EqualTo(new IPAddress(bigEndianValue)));
        }
    }
}
