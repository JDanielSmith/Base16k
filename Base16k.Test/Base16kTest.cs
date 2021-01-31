using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JDanielSmith
{
    public class ConvertTest
    {
        [Theory]
        [MemberData(nameof(TripsBytesData))]
        public void RoundTripsBytes(int length, int seed)
        {
            // Arrange
            var expected = new byte[length];
            var random = new Random(seed);
            random.NextBytes(expected);

            // Act
            var encoded = Convert.ToBase16KString(expected);
            var actual = Convert.FromBase16KString(encoded);

            // Assert
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> TripsBytesData => Enumerable
            .Range(0, 17)
            .Select(i => (int)Math.Pow(2, i))
            .SelectMany(l =>
            {
                var random = new Random(l);
                return Enumerable
                    .Range(0, 2)
                    .Select(i => new object[] { l, random.Next() });
            });

        public class TheToBase16KStringMethod
        {
            [Fact]
            public void ConvertsKnownInput()
            {
                var input = System.Convert.FromBase64String(
                    "q3PbbFnRWhU9XMTfcnXqZdLqJP1WzEDUW7m7cDX6F5EcGoo1zcvNWa2nxSXVw2+BMRWjJeEnJd8zBhCfFerfQkQyw1jqtJgHHcpQrgYa/5hKOT8gV/Kw9PSqAsa6LUPDx4Lhog==");

                var actual = Convert.ToBase16KString(input);

                Assert.Equal(
                    "100竜趶腧慚問旌捽艵誙洮碓赖茐嵅绦議嵾煹呰檊嵳沼蕦綧腉浜嶾儱啨艞咜痟峁焉豗竟悑匬嵣窴瘁臜祂縆嚿覄磤輠旼笏插稂膮狔式垂表瀀",
                    actual);
            }

            [Fact]
            public void ConvertsEmptyBytes()
            {
                Assert.Equal("0", Convert.ToBase16KString(new byte[0]));
            }

            [Fact]
            public void RejectNullInput()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Convert.ToBase16KString(null));
                Assert.Equal("inArray", ex.ParamName);
            }
        }

        public class TheFromBase16KStringMethod
        {
            [Fact]
            public void ConvertsKnownInput()
            {
                var input = "100竜趶腧慚問旌捽艵誙洮碓赖茐嵅绦議嵾煹呰檊嵳沼蕦綧腉浜嶾儱啨艞咜痟峁焉豗竟悑匬嵣窴瘁臜祂縆嚿覄磤輠旼笏插稂膮狔式垂表瀀";
                var expected = System.Convert.FromBase64String(
                    "q3PbbFnRWhU9XMTfcnXqZdLqJP1WzEDUW7m7cDX6F5EcGoo1zcvNWa2nxSXVw2+BMRWjJeEnJd8zBhCfFerfQkQyw1jqtJgHHcpQrgYa/5hKOT8gV/Kw9PSqAsa6LUPDx4Lhog==");

                var actual = Convert.FromBase16KString(input);

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ConvertsEmptyBytes()
            {
                Assert.Equal(Array.Empty<byte>(), Convert.FromBase16KString("0"));
            }

            [Fact]
            public void RejectTooShortInput()
            {
                var ex = Assert.Throws<FormatException>(() => Convert.FromBase16KString("100竜"));
                Assert.Equal("Too few Han characters representing binary data.", ex.Message);
            }

            [Fact]
            public void RejectsInputNotStartingWithLength()
            {
                var ex = Assert.Throws<FormatException>(() => Convert.FromBase16KString("A竜趶腧慚"));
                Assert.Equal("Unable to find a length value.", ex.Message);
            }

            [Fact]
            public void RejectUnparsableLength()
            {
                var ex = Assert.Throws<FormatException>(() => Convert.FromBase16KString("9999999999999999999999竜趶腧慚"));
                Assert.Equal("Unable to parse the length string.", ex.Message);
            }

            [Fact]
            public void RejectNullInput()
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Convert.FromBase16KString(null));
                Assert.Equal("input", ex.ParamName);
            }
        }
    }
}
