// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    /// <summary>
    /// A class containing tests for the <see cref="Day19"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day19Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day19Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day19Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(5, 1, 3)]
        [InlineData(5, 2, 2)]
        public static void Y2016_Day19_FindElfThatGetsAllPresents_Returns_Correct_Solution(int count, int version, int expected)
        {
            // Act
            int actual = Day19.FindElfThatGetsAllPresents(count, version);

            // Assert
            actual.ShouldBe(expected);
        }

        [Theory]
        [InlineData("5", "1", 3)]
        [InlineData("5", "2", 2)]
        [InlineData("3014387", "1", 1834471)]
        [InlineData("3014387", "2", 1420064)]
        public async Task Y2016_Day19_Solve_Returns_Correct_Solution(string elves, string version, int expected)
        {
            // Arrange
            string[] args = new[] { elves, version };

            // Act
            var puzzle = await SolvePuzzleAsync<Day19>(args);

            // Assert
            puzzle.ElfWithAllPresents.ShouldBe(expected);
        }
    }
}
