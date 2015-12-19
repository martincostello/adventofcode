// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day17"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day17Tests
    {
        [Fact]
        public static void Day17_GetContainerCombinationCount()
        {
            // Arrange
            int volume = 25;
            int[] containerVolumes = new[] { 20, 15, 10, 5, 5 };

            // Act
            int actual = Day17.GetContainerCombinationCount(volume, containerVolumes);

            // Assert
            Assert.Equal(4, actual);
        }

        [Fact(Skip = "Not working correctly yet.")]
        public static void Day17_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { @".\Input\Day17\input.txt", "150" };
            var target = new Day17();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.NotEqual(0, target.Combinations);
        }
    }
}
