// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    /// <summary>
    /// A class containing tests for the <see cref="Day17"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day17Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day17Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day17Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("hijkl", "", 0)]
        [InlineData("ihgpwlah", "DDRRRD", 370)]
        [InlineData("kglvqrro", "DDUDRLRRUDRD", 492)]
        [InlineData("ulqzkmiv", "DRURDRUDDLLDLUURRDULRLDUUDDDRR", 830)]
        public static void Y2016_Day17_GetPathsToVault_Returns_Correct_Solution(
            string passcode,
            string expectedShortestPath,
            int expectedLongestPath)
        {
            // Act
            (string actualShortestPath, int actualLongestPath) = Day17.GetPathsToVault(passcode);

            // Assert
            actualShortestPath.ShouldBe(expectedShortestPath);
            actualLongestPath.ShouldBe(expectedLongestPath);
        }

        [Fact]
        public async Task Y2016_Day17_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day17>("pvhmgsws");

            // Assert
            puzzle.ShortestPathToVault.ShouldBe("DRRDRLDURD");
            puzzle.LongestPathToVault.ShouldBe(618);
        }
    }
}
