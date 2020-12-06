// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System.Threading.Tasks;
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day09"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day09Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day09Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day09Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public static void Y2015_Day09_Shortest_Distance_To_Visit_All_Points_Once_Is_Correct()
        {
            // Arrange
            string[] distances = new[]
            {
                "London to Dublin = 464",
                "London to Belfast = 518",
                "Dublin to Belfast = 141",
            };

            // Act
            int actual = Day09.GetShortestDistanceBetweenPoints(distances);

            // Assert
            actual.ShouldBe(605);
        }

        [Fact]
        public static void Y2015_Day09_Shortest_Distance_To_Visit_All_Points_Once_Is_Correct_If_Only_One_Point()
        {
            // Arrange
            string[] distances = new[]
            {
                "London to Dublin = 464",
            };

            // Act
            int actual = Day09.GetShortestDistanceBetweenPoints(distances);

            // Assert
            actual.ShouldBe(464);
        }

        [Theory]
        [InlineData(new string[0], 207)]
        [InlineData(new[] { "true" }, 804)]
        public async Task Y2015_Day09_Solve_Returns_Correct_Solution(string[] args, int expected)
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day09>(args);

            // Assert
            puzzle.Solution.ShouldBe(expected);
        }
    }
}
