// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day23Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(false, 12521)]
    [InlineData(true, 44169)]
    public void Y2021_Day23_Organize_Returns_Correct_Value(bool unfoldDiagram, int expected)
    {
        // Arrange
        string[] diagram =
        [
            "#############",
            "#...........#",
            "###B#C#B#D###",
            "  #A#D#C#A#",
            "  #########",
        ];

        // Act
        int actual = Day23.Organize(diagram, unfoldDiagram);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day23_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day23>();

        // Assert
        puzzle.MinimumEnergyFolded.ShouldBe(12240);
        puzzle.MinimumEnergyUnfolded.ShouldBe(44618);
    }
}
