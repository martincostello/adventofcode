// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day05Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("abc", false, "18f47a30")]
    [InlineData("abc", true, "05ace8e3")]
    public static void Y2016_Day05_GeneratePassword_Returns_Correct_Solution(string doorId, bool isPositionSpecifiedByHash, string expected)
    {
        // Act
        string actual = Day05.GeneratePassword(doorId, isPositionSpecifiedByHash);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2016_Day05_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day05>("wtnhxymk");

        // Assert
        puzzle.Password.ShouldBe("2414bc77");
        puzzle.PasswordWhenPositionIsIndicated.ShouldBe("437e60fc");
    }
}
