// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day08"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day08Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day08Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day08Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("\"\"", 0)]
        [InlineData("\"abc\"", 3)]
        [InlineData("\"aaa\\\"aaa\"", 7)]
        [InlineData("\"\\x27\"", 1)]
        [InlineData("\"v\\xfb\"lgs\"kvjfywmut\\x9cr\"", 18)]
        [InlineData("\"d\\\\gkbqo\\\\fwukyxab\"u\"", 18)]
        public static void Y2015_Day08_GetLiteralCharacterCount(string value, int expected)
        {
            // Act
            int actual = Day08.GetLiteralCharacterCount(value);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public static void Y2015_Day08_GetLiteralCharacterCountForCollection()
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
            actual.ShouldBe(11);
        }

        [Theory]
        [InlineData("\"\"", 6)]
        [InlineData("\"abc\"", 9)]
        [InlineData("\"aaa\\\"aaa\"", 16)]
        [InlineData("\"\\x27\"", 11)]
        public static void Y2015_Day08_GetEncodedCharacterCount(string value, int expected)
        {
            // Act
            int actual = Day08.GetEncodedCharacterCount(value);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public static void Y2015_Day08_GetEncodedCharacterCountForCollection()
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
            actual.ShouldBe(42);
        }

        [Fact]
        public async Task Y2015_Day08_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day08>();

            // Assert
            puzzle.FirstSolution.ShouldBe(1342);
            puzzle.SecondSolution.ShouldBe(2074);
        }
    }
}
