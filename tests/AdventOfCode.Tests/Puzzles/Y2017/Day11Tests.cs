// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day11"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day11Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day11Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day11Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData("ne,ne,ne", 3)]
        [InlineData("ne,ne,sw,sw", 0)]
        [InlineData("ne,ne,s,s", 2)]
        [InlineData("se,sw,se,sw,sw", 3)]
        public static void Y2017_Day11_FindMinimumSteps_Returns_Correct_Solution(string path, int expected)
        {
            // Act
            (int actual, int _) = Day11.FindStepRange(path);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void Y2017_Day11_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day11>();

            // Assert
            puzzle.MinimumSteps.ShouldBe(796);
            puzzle.MaximumDistance.ShouldBe(1585);
        }
    }
}
