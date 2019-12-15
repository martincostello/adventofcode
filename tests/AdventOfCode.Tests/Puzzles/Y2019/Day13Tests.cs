// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day13"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day13Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day13Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day13Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void Y2019_Day13_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day13>();

            // Assert
            puzzle.BlockTileCount.ShouldBe(315);
        }
    }
}
