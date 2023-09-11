// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

public sealed class Day03Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(new[] { "#1 @ 1,3: 4x4", "#2 @ 3,1: 4x4", "#3 @ 5,5: 2x2" }, 4)]
    public static void Y2018_Day03_GetAreaWithTwoOrMoreOverlappingClaims_Returns_Correct_Solution(
        string[] sequence,
        int expected)
    {
        // Act
        int actual = Day03.GetAreaWithTwoOrMoreOverlappingClaims(sequence);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(new[] { "#1 @ 1,3: 4x4", "#2 @ 3,1: 4x4", "#3 @ 5,5: 2x2" }, "3")]
    public static void Y2018_Day03_GetClaimWithNoOverlappingClaims_Returns_Correct_Solution(
        string[] sequence,
        string expected)
    {
        // Act
        string? actual = Day03.GetClaimWithNoOverlappingClaims(sequence);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2018_Day03_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day03>();

        // Assert
        puzzle.Area.ShouldBe(100595);
        puzzle.IdOfUniqueClaim.ShouldBe("415");
    }
}
