﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day09"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day09Tests
    {
        [Fact]
        public static void Day09_Shortest_Distance_To_Visit_All_Points_Once_Is_Correct()
        {
            // Arrange
            var distances = new[]
            {
                "London to Dublin = 464",
                "London to Belfast = 518",
                "Dublin to Belfast = 141",
            };

            // Act
            int actual = Day09.GetShortestDistanceBetweenPoints(distances);

            // Assert
            Assert.Equal(605, actual);
        }

        [Fact]
        public static void Day09_Shortest_Distance_To_Visit_All_Points_Once_Is_Correct_If_Only_One_Point()
        {
            // Arrange
            var distances = new[]
            {
                "London to Dublin = 464",
            };

            // Act
            int actual = Day09.GetShortestDistanceBetweenPoints(distances);

            // Assert
            Assert.Equal(464, actual);
        }

        [Fact]
        public static void Day09_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string inputPath = PuzzleTestHelpers.GetInputPath(9);
            string[] args = new[] { inputPath };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day09>(args);

            // Assert
            Assert.Equal(207, puzzle.Solution);

            // Arrange
            args = new[] { inputPath, "true" };

            // Act
            puzzle = PuzzleTestHelpers.SolvePuzzle<Day09>(args);

            // Assert
            Assert.Equal(804, puzzle.Solution);
        }
    }
}
