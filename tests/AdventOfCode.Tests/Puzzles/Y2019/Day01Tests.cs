// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using Xunit;
    using Xunit.Abstractions;

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
        [InlineData("12", 2)]
        [InlineData("14", 2)]
        [InlineData("1969", 654)]
        [InlineData("100756", 33583)]
        public static void Y2019_Day01_GetFuelRequirementsForModule(string mass, double expected)
        {
            // Act
            double actual = Day01.GetFuelRequirementsForModule(mass);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Y2019_Day01_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day01>();

            // Assert
            Assert.Equal(3226407, puzzle.TotalFuelRequired);
        }
    }
}
