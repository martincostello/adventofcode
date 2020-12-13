// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

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
        [InlineData("hijkl", "")]
        [InlineData("ihgpwlah", "DDRRRD")]
        [InlineData("kglvqrro", "DDUDRLRRUDRD")]
        [InlineData("ulqzkmiv", "DRURDRUDDLLDLUURRDULRLDUUDDDRR")]
        public static void Y2016_Day17_GetShortestPathToVault_Returns_Correct_Solution(
            string passcode,
            string expected)
        {
            // Act
            string actual = Day17.GetShortestPathToVault(passcode);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2016_Day17_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day17>("pvhmgsws");

            // Assert
            puzzle.ShortestPathToVault.ShouldBe("DRRDRLDURD");
        }
    }
}
