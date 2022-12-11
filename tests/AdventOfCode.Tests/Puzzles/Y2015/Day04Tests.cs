// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class containing tests for the <see cref="Day04"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day04Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day04Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day04Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData("abcdef", 5, 609043)]
    [InlineData("pqrstuv", 5, 1048970)]
    public static async Task Y2015_Day04_GetLowestPositiveNumberWithStartingZeroesAsync(string secretKey, int zeroes, int expected)
    {
        // Act
        int actual = await Day04.GetLowestPositiveNumberWithStartingZeroesAsync(secretKey, zeroes);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2015_Day04_Solve_Returns_Correct_Solution()
    {
        // Arrange
        string secretKey = "iwrupvqb";

        // Act
        var puzzle = await SolvePuzzleAsync<Day04>(secretKey);

        // Assert
        puzzle.LowestZeroHash5.ShouldBe(346386);
        puzzle.LowestZeroHash6.ShouldBe(9958218);
    }
}
