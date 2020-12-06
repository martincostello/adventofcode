// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day03"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day03Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day03Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day03Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(5, 10, 25, false)]
        public static void Y2016_Day03_IsTriangleValid_Returns_Correct_Solution(int a, int b, int c, bool expected)
        {
            // Act
            bool actual = Day03.IsValidTriangle(a, b, c);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2016_Day03_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day03>();

            // Assert
            puzzle.PossibleTrianglesByRows.ShouldBe(983);
            puzzle.PossibleTrianglesByColumns.ShouldBe(1836);
        }
    }
}
