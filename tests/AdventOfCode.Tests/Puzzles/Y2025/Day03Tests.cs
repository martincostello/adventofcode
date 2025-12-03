// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day03Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("987654321111111", 2, 98)]
    [InlineData("987654321111111", 12, 987654321111)]
    [InlineData("811111111111119", 2, 89)]
    [InlineData("811111111111119", 12, 811111111119)]
    [InlineData("234234234234278", 2, 78)]
    [InlineData("234234234234278", 12, 434234234278)]
    [InlineData("818181911112111", 2, 92)]
    [InlineData("818181911112111", 12, 888911112111)]
    public void Y2025_Day03_GetJoltage_Returns_Correct_Value(string value, int batteries, long expected)
    {
        // Act
        long actual = Day03.GetJoltage([value], batteries);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(2, 357)]
    [InlineData(12, 3121910778619)]
    public void Y2025_Day03_GetJoltage_Returns_Correct_Total(int batteries, long expected)
    {
        // Arrange
        string[] values =
        [
            "987654321111111",
            "811111111111119",
            "234234234234278",
            "818181911112111",
        ];

        // Act
        long actual = Day03.GetJoltage(values, batteries);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2025_Day03_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day03>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalOutputJoltageFor2.ShouldBe(17524);
        puzzle.TotalOutputJoltageFor12.ShouldBe(173848577117276);
    }
}
