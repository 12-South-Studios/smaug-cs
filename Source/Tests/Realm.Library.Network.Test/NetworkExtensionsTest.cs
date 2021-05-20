using System;
using System.Net;
using FluentAssertions;
using Realm.Library.Network.Mxp;
using Xunit;

namespace Realm.Library.Network.Test
{
    public class NetworkExtensionsTest
    {
        [Fact]
        public void MxpTag_ConvertsStringProperly()
        {
            const string test = "Test";

            test.MxpTag().Should().Be("\x03Test\x04");
        }

        [Fact]
        public void MxpAmp_ReturnsAmpersandValue()
        {
            MxpExtensions.MxpAmp().Should().Be("\x06");
        }

        [Fact]
        public void MxpMode_ReturnsMxpModeValue()
        {
            const int value = 6;
            value.MxpMode().Should().Be("\x1B[6z");
        }

        [Theory]
        [InlineData("100.101.102.103", 1734763876)]
        [InlineData("localhost", 16777343)]
        [InlineData("this_is_not_an_ip_address", 16777343)]
        public void ConvertToIpAddress(string ipAddress, int bigEndianValue)
        {
            ipAddress.ConvertToIpAddress().Should().Be(new IPAddress(bigEndianValue));
        }

        [Fact]
        public void ConvertToIpAddress_GetsNullIpAddress_ThrowsException()
        {
            const string ipAddress = null;

            Action act = () => ipAddress.ConvertToIpAddress();
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
