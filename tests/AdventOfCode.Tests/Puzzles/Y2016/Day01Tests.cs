// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    /// <summary>
    /// A class containing tests for the <see cref="Day01"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day01Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day01Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day01Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("R2, L3", true, 5)]
        [InlineData("R2, R2, R2", true, 2)]
        [InlineData("R5, L5, R5, R3", true, 12)]
        [InlineData("R8, R4, R4, R8", false, 4)]
        public static void Y2016_Day01_CalculateDistance_Returns_Correct_Solution(
            string instructions,
            bool ignoreDuplicates,
            int expected)
        {
            // Act
            int actual = Day01.CalculateDistance(instructions, ignoreDuplicates);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2016_Day01_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day01>();

            // Assert
            puzzle.BlocksToEasterBunnyHQIgnoringDuplicates.ShouldBe(287);
            puzzle.BlocksToEasterBunnyHQ.ShouldBe(133);
        }
    }
}
