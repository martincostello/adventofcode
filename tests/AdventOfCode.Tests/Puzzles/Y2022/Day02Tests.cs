// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day02"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day02Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day02Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day02Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(false, 15)]
    [InlineData(true, 12)]
    public void Y2022_Day02_GetTotalScore_Returns_Correct_Values(bool containsDesiredOutcome, int expected)
    {
        // Arrange
        string[] moves = new[]
        {
            "A Y",
            "B X",
            "C Z",
        };

        // Act
        int actual = Day02.GetTotalScore(moves, containsDesiredOutcome);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2022_Day02_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day02>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalScoreForMoves.ShouldBe(13675);
        puzzle.TotalScoreForOutcomes.ShouldBe(14184);
    }
}
