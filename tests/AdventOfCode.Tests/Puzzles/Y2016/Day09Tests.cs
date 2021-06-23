// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    /// <summary>
    /// A class containing tests for the <see cref="Day09"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day09Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day09Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day09Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("ADVENT", 1, 6)]
        [InlineData("A(1x5)BC", 1, 7)]
        [InlineData("(3x3)XYZ", 1, 9)]
        [InlineData("A(2x2)BCD(2x2)EFG", 1, 11)]
        [InlineData("(6x1)(1x3)A", 1, 6)]
        [InlineData("X(8x2)(3x3)ABCY", 1, 18)]
        [InlineData("(3x3)XYZ", 2, 9)]
        [InlineData("X(8x2)(3x3)ABCY", 2, 20)]
        [InlineData("(27x12)(20x12)(13x14)(7x10)(1x12)A", 2, 241920)]
        [InlineData("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN", 2, 445)]
        public static void Y2016_Day09_GetDecompressedLength_Returns_Correct_Solution(string data, int version, long expected)
        {
            // Act
            long actual = Day09.GetDecompressedLength(data, version);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2016_Day09_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day09>();

            // Assert
            puzzle.DecompressedLengthVersion1.ShouldBe(98135);
            puzzle.DecompressedLengthVersion2.ShouldBe(10964557606);
        }
    }
}
