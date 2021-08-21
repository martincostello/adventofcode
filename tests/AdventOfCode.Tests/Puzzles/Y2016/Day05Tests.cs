// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class containing tests for the <see cref="Day05"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day05Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day05Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day05Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

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
