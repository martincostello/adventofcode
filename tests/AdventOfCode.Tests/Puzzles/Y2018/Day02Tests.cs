// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

public sealed class Day02Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("abcdef", 0, 0)]
    [InlineData("bababc", 1, 1)]
    [InlineData("abbcde", 1, 0)]
    [InlineData("abcccd", 0, 1)]
    [InlineData("aabcdd", 1, 0)]
    [InlineData("abcdee", 1, 0)]
    [InlineData("ababab", 0, 1)]
    public static void Y2018_Day02_GetBoxScore_Returns_Correct_Solution(
        string id,
        int expectedCount2,
        int expectedCount3)
    {
        // Act
        (int actualCount2, int actualCount3) = Day02.GetBoxScore(id);

        // Assert
        actualCount2.ShouldBe(expectedCount2);
        actualCount3.ShouldBe(expectedCount3);
    }

    [Theory]
    [InlineData(new[] { "abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab" }, 12)]
    public static void Y2018_Day02_CalculateChecksum_Returns_Correct_Solution(
        string[] boxIds,
        int expected)
    {
        // Act
        int actual = Day02.CalculateChecksum(boxIds);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2018_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.Checksum.ShouldBe(5880);
        puzzle.CommonLettersForBoxes.ShouldBe("tiwcdpbseqhxryfmgkvjujvza");
    }
}
