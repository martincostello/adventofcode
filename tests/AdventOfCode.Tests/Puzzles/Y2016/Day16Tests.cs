// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day16Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("110010110100", 12, "100")]
    [InlineData("10000", 20, "01100")]
    public static void Y2016_Day16_GetDiskChecksum_Returns_Correct_Solution(string initial, int size, string expected)
    {
        // Act
        string actual = Day16.GetDiskChecksum(initial, size);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("272", "10010110010011110")]
    [InlineData("35651584", "01101011101100011")]
    public async Task Y2016_Day16_Solve_Returns_Correct_Solution(string size, string expected)
    {
        // Arrange
        string[] args = new[] { "10010000000110000", size };

        // Act
        var puzzle = await SolvePuzzleAsync<Day16>(args);

        // Assert
        puzzle.Checksum.ShouldBe(expected);
    }
}
