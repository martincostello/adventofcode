// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day08"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day08Tests
    {
        [Theory]
        [InlineData("\"\"", 0)]
        [InlineData("\"abc\"", 3)]
        [InlineData("\"aaa\\\"aaa\"", 7)]
        [InlineData("\"\\x27\"", 1)]
        [InlineData("\"v\\xfb\"lgs\"kvjfywmut\\x9cr\"", 18)]
        [InlineData("\"d\\\\gkbqo\\\\fwukyxab\"u\"", 18)]
        public static void Day8_GetLiteralCharacterCount(string value, int expected)
        {
            // Act
            int actual = Day08.GetLiteralCharacterCount(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Day8_GetLiteralCharacterCountForCollection()
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
            int actual = Day08.GetLiteralCharacterCount(collection);

            // Assert
            Assert.Equal(11, actual);
        }

        [Theory]
        [InlineData("\"\"", 6)]
        [InlineData("\"abc\"", 9)]
        [InlineData("\"aaa\\\"aaa\"", 16)]
        [InlineData("\"\\x27\"", 11)]
        public static void Day8_GetEncodedCharacterCount(string value, int expected)
        {
            // Act
            int actual = Day08.GetEncodedCharacterCount(value);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public static void Day8_GetEncodedCharacterCountForCollection()
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
            int actual = Day08.GetEncodedCharacterCount(collection);

            // Assert
            Assert.Equal(42, actual);
        }
    }
}
