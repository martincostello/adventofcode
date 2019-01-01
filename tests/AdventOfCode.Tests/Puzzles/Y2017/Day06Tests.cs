// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day06"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day06Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day06Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day06Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public static void Y2017_Day06_Debug_Returns_Correct_Value()
        {
            // Arrange
            int[] memory = new[] { 0, 2, 7, 0 };

            // Act
            (int cycleCount, int loopSize) = Day06.Debug(memory);

            // Assert
            Assert.Equal(5, cycleCount);
            Assert.Equal(4, loopSize);
        }

        [Fact]
        public void Y2017_Day06_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day06>();

            // Assert
            Assert.Equal(3156, puzzle.CycleCount);
            Assert.Equal(1610, puzzle.LoopSize);
        }
    }
}
