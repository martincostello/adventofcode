// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day06Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(new[] { "eedadn", "drvtee", "eandsr", "raavrd", "atevrs", "tsrnev", "sdttsa", "rasrtv", "nssdts", "ntnada", "svetve", "tesnvt", "vntsnd", "vrdear", "dvrsen", "enarar" }, false, "easter")]
    [InlineData(new[] { "eedadn", "drvtee", "eandsr", "raavrd", "atevrs", "tsrnev", "sdttsa", "rasrtv", "nssdts", "ntnada", "svetve", "tesnvt", "vntsnd", "vrdear", "dvrsen", "enarar" }, true, "advent")]
    public static void Y2016_Day06_DecryptMessage_Returns_Correct_Solution(string[] messages, bool leastLikely, string expected)
    {
        // Act
        string actual = Day06.DecryptMessage(messages, leastLikely);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2016_Day06_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day06>();

        // Assert
        puzzle.ErrorCorrectedMessage.ShouldBe("qzedlxso");
        puzzle.ModifiedErrorCorrectedMessage.ShouldBe("ucmifjae");
    }
}
