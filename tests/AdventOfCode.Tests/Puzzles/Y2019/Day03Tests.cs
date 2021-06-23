// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
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
        [InlineData(new[] { "R8,U5,L5,D3", "U7,R6,D4,L4" }, 6, 30)]
        [InlineData(new[] { "R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83" }, 159, 610)]
        [InlineData(new[] { "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" }, 135, 410)]
        public void Y2019_Day03_GetManhattanDistanceOfClosesIntersection_Computes_Distance(
            string[] wires,
            int expectedDistance,
            int expectedSteps)
        {
            // Act
            (int actualDistance, int actualSteps) = Day03.GetManhattanDistanceOfClosesIntersection(wires);

            // Assert
            actualDistance.ShouldBe(expectedDistance);
            actualSteps.ShouldBe(expectedSteps);
        }

        [Fact]
        public async Task Y2019_Day03_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day03>();

            // Assert
            puzzle.ManhattanDistance.ShouldBe(855);
            puzzle.MinimumSteps.ShouldBe(11238);
        }
    }
}
