// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day02"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day02Tests
    {
        [Theory]
        [InlineData("2x3x4", 58, 34)]
        [InlineData("1x1x10", 43, 14)]
        public static void Day02_GetTotalWrappingPaperAreaAndRibbonLength(string dimension, int area, int length)
        {
            // Arrange
            string[] dimensions = new[] { dimension };

            // Act
            Tuple<int, int> result = Day02.GetTotalWrappingPaperAreaAndRibbonLength(dimensions);

            // Assert
            Assert.Equal(area, result.Item1);
            Assert.Equal(length, result.Item2);
        }

        [Fact]
        public static void Day02_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new[] { PuzzleTestHelpers.GetInputPath(2) };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day02>(args);

            // Assert
            Assert.Equal(1598415, puzzle.TotalAreaOfPaper);
            Assert.Equal(3812909, puzzle.TotalLengthOfRibbon);
        }
    }
}
