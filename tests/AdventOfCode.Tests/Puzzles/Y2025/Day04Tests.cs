// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day04Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2025_Day04_FindAccessible_Returns_Correct_Total()
    {
        // Arrange
        string[] diagram =
        [
            "..@@.@@@@.",
            "@@@.@.@.@@",
            "@@@@@.@.@@",
            "@.@@@@..@.",
            "@@.@@@@.@@",
            ".@@@@@@@.@",
            ".@.@.@.@@@",
            "@.@@@.@@@@",
            ".@@@@@@@@.",
            "@.@.@@@.@.",
        ];

        // Act
        (long actualAccessible, int actualRemoved) = Day04.ArrangeRolls(diagram, TestContext.Current.CancellationToken);

        // Assert
        actualAccessible.ShouldBe(13);
        actualRemoved.ShouldBe(43);
    }

    [Fact]
    public async Task Y2025_Day04_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day04>();

        // Assert
        puzzle.Solution1.ShouldBe(1587);
        puzzle.Solution2.ShouldBe(8946);
    }
}
