// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day23"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day23Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day23Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day23Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Theory]
        [InlineData(10, "92658374")]
        [InlineData(100, "67384529")]
        public void Y2020_Day23_Play_Returns_Correct_Value(int moves, string expected)
        {
            // Arrange
            int[] arrangement = { 3, 8, 9, 1, 2, 5, 4, 6, 7 };

            // Act
            string actual = Day23.Play(arrangement, moves);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task Y2020_Day23_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day23>("583976241");

            // Assert
            puzzle.LabelsAfterCup1.ShouldBe("24987653");
        }
    }
}
