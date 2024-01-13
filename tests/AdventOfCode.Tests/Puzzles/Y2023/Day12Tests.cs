// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day12Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("???.### 1,1,3", false, 1)]
    [InlineData(".??..??...?##. 1,1,3", false, 4)]
    [InlineData("?#?#?#?#?#?#?#? 1,3,1,6", false, 1)]
    [InlineData("????.#...#... 4,1,1", false, 1)]
    [InlineData("????.######..#####. 1,6,5", false, 4)]
    [InlineData("?###???????? 3,2,1", false, 10)]
    [InlineData("???.### 1,1,3", true, 1)]
    [InlineData(".??..??...?##. 1,1,3", true, 16384, Skip = "Unsolved.")]
    [InlineData("?#?#?#?#?#?#?#? 1,3,1,6", true, 1, Skip = "Unsolved.")]
    [InlineData("????.#...#... 4,1,1", true, 16, Skip = "Unsolved.")]
    [InlineData("????.######..#####. 1,6,5", true, 2500)]
    [InlineData("?###???????? 3,2,1", true, 506250, Skip = "Unsolved.")]
    public void Y2023_Day12_Analyze_Returns_Correct_Value(string record, bool unfold, int expected)
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // Act
        int actual = Day12.Analyze(record, unfold, cts.Token);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(false, 21)]
    [InlineData(true, 525152, Skip = "Unsolved.")]
    public void Y2023_Day12_Analyze_Returns_Correct_Value_For_Records(bool unfold, int expected)
    {
        // Arrange
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        string[] records =
        [
            "???.### 1,1,3",
            ".??..??...?##. 1,1,3",
            "?#?#?#?#?#?#?#? 1,3,1,6",
            "????.#...#... 4,1,1",
            "????.######..#####. 1,6,5",
            "?###???????? 3,2,1",
        ];

        // Act
        int actual = Day12.Analyze(records, unfold, cts.Token);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact(Skip = "Unsolved.")]
    public async Task Y2023_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SumOfCountsFolded.ShouldBe(7047);
        puzzle.SumOfCountsUnfolded.ShouldBe(-1);
    }
}
