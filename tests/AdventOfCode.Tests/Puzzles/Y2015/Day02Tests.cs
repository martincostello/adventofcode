// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day02Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(new[] { "2x3x4" }, 58, 34)]
    [InlineData(new[] { "1x1x10" }, 43, 14)]
    public static void Y2015_Day02_GetTotalWrappingPaperAreaAndRibbonLength(string[] dimensions, int expectedArea, int expectedLength)
    {
        // Act
        (int actualArea, int actualLength) = Day02.GetTotalWrappingPaperAreaAndRibbonLength(dimensions);

        // Assert
        actualArea.ShouldBe(expectedArea);
        actualLength.ShouldBe(expectedLength);
    }

    [Fact]
    public async Task Y2015_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.TotalAreaOfPaper.ShouldBe(1598415);
        puzzle.TotalLengthOfRibbon.ShouldBe(3812909);
    }
}
