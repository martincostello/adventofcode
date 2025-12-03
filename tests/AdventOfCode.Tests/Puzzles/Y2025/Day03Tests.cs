// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day03Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("987654321111111", 98)]
    [InlineData("811111111111119", 89)]
    [InlineData("234234234234278", 78)]
    [InlineData("818181911112111", 92)]
    public void Y2025_Day03_GetJoltage_Returns_Correct_Value(string value, int expected)
    {
        // Act
        int actual = Day03.GetJoltage([value]);

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
        puzzle.TotalOutputJoltage.ShouldBe(17524);
    }
}
