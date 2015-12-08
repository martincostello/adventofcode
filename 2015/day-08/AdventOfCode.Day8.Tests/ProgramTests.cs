// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgramTests.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   ProgramTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day8
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Program"/> class. This class cannot be inherited.
    /// </summary>
    public static class ProgramTests
    {
        [Theory]
        [InlineData("\"\"", 0)]
        [InlineData("\"abc\"", 3)]
        [InlineData("\"aaa\\\"aaa\"", 7)]
        [InlineData("\"\\x27\"", 1)]
        [InlineData("\"v\\xfb\"lgs\"kvjfywmut\\x9cr\"", 18)]
        [InlineData("\"d\\\\gkbqo\\\\fwukyxab\"u\"", 18)]
        public static void GetLiteralCharacterCount(string value, int expected)
        {
            // Act
            int actual = Program.GetLiteralCharacterCount(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void GetLiteralCharacterCountForCollection()
        {
            // Arrange
            string[] collection = new[]
            {
                "\"\"",
                "\"abc\"",
                "\"aaa\\\"aaa\"",
                "\"\\x27\"",
            };

            // Act
            int actual = Program.GetLiteralCharacterCount(collection);

            // Assert
            Assert.Equal(11, actual);
        }
    }
}
