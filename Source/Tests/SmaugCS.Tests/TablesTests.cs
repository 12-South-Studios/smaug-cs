using FluentAssertions;
using Xunit;

namespace SmaugCS.Tests
{

    public class TablesTests
    {
        [Fact]
        public void ConvertStringSyllablesTest()
        {
            const string inputString = "Protect Evil";
            const string outputString = "sfainfra zzur";

            tables.ConvertStringSyllables(inputString).Should().Be(outputString);
        }
    }
}
