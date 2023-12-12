// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day12Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("???.### 1,1,3", 1)]
    [InlineData(".??..??...?##. 1,1,3", 4)]
    [InlineData("?#?#?#?#?#?#?#? 1,3,1,6", 1)]
    [InlineData("????.#...#... 4,1,1", 1)]
    [InlineData("????.######..#####. 1,6,5", 4)]
    [InlineData("?###???????? 3,2,1", 10)]
    public void Y2023_Day12_Analyze_Returns_Correct_Value(string record, int expected)
    {
        // Act
        int actual = Day12.Analyze(record, CancellationToken.None);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void Y2023_Day12_Analyze_Returns_Correct_Value_For_Records()
    {
        // Arrange
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
        int actual = Day12.Analyze(records, CancellationToken.None);

        // Assert
        actual.ShouldBe(21);
    }

    [Fact]
    public async Task Y2023_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.SumOfCounts.ShouldBe(-1);
    }
}
