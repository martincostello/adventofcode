// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using Shouldly;
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

        [Fact]
        public void Y2020_Day01_Get2020Product_Returns_Correct_Value()
        {
            string[] values = new[]
            {
                "1721",
                "979",
                "366",
                "299",
                "675",
                "1456",
            };

            // Act
            int actual = Day01.Get2020Product(values);

            // Assert
            actual.ShouldBe(514579);
        }

        [Fact]
        public void Y2020_Day01_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = SolvePuzzle<Day01>();

            // Assert
            puzzle.ProductOf2020Sum.ShouldBe(63616);
        }
    }
}
