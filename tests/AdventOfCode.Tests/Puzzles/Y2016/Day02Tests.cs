// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day02"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day02Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day02Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day02Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(new[] { "UDLR" }, "5")]
        [InlineData(new[] { "ULL", "RRDDD", "LURDL", "UUUUD" }, "1985")]
        public static void Y2016_Day02_GetBathroomCode_Returns_Correct_Solution_Digits(string[] instructions, string expected)
        {
            // Act
            string actual = Day02.GetBathroomCode(instructions, Day02.DigitGrid);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData(new[] { "UDLR" }, "6")]
        [InlineData(new[] { "ULL", "RRDDD", "LURDL", "UUUUD" }, "5DB3")]
        public static void Y2016_Day02_GetBathroomCode_Returns_Correct_Solution_Alphanumeric(string[] instructions, string expected)
        {
            // Act
            string actual = Day02.GetBathroomCode(instructions, Day02.AlphanumericGrid);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2016_Day02_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day02>();

            // Assert
            puzzle.BathroomCodeDigitKeypad.ShouldBe("14894");
            puzzle.BathroomCodeAlphanumericKeypad.ShouldBe("26B96");
        }
    }
}
