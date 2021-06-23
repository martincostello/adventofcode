// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
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
        [InlineData(10, new[] { 5, 8, 3, 7, 4, 1, 9, 2, 6 })]
        [InlineData(100, new[] { 2, 9, 1, 6, 7, 3, 8, 4, 5, })]
        public void Y2020_Day23_Play_Returns_Correct_Value_Part_1(int moves, int[] expected)
        {
            // Arrange
            int[] arrangement = { 3, 8, 9, 1, 2, 5, 4, 6, 7 };

            // Act
            LinkedList<int> actual = Day23.Play(arrangement, moves);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public void Y2020_Day23_Play_Returns_Correct_Value_Part_2()
        {
            // Arrange
            IEnumerable<int> arrangement = new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 }.Concat(Enumerable.Range(10, 999_991));

            // Act
            LinkedList<int> actual = Day23.Play(arrangement, moves: 10_000_000);

            LinkedListNode<int>? item1 = actual.Find(1);

            // Assert
            item1.ShouldNotBeNull();

            var next = item1.Next ?? actual.First;
            next.ShouldNotBeNull();
            next.Value.ShouldBe(934001);

            next = next.Next ?? actual.First;

            next.ShouldNotBeNull();
            next.Value.ShouldBe(159792);
        }

        [Fact]
        public async Task Y2020_Day23_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day23>("583976241");

            // Assert
            puzzle.LabelsAfterCup1.ShouldBe("24987653");
            puzzle.ProductOfLabelsAfterCup1.ShouldBe(442938711161L);
        }
    }
}
