// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day23Tests : PuzzleTest
{
    public Day23Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    public static IEnumerable<object[]> Costs()
    {
        yield return new object[] { ("           ", "BA", "CD", "BC", "DA"), ("   B       ", "BA", "CD", " C", "DA"), 40 };
        yield return new object[] { ("   B       ", "BA", "CD", " C", "DA"), ("   B       ", "BA", " D", "CC", "DA"), 400 };
        yield return new object[] { ("   B       ", "BA", " D", "CC", "DA"), ("   B D     ", "BA", "  ", "CC", "DA"), 3000 };
        yield return new object[] { ("   B D     ", "BA", "  ", "CC", "DA"), ("     D     ", "BA", " B", "CC", "DA"), 30 };
        yield return new object[] { ("     D     ", "BA", " B", "CC", "DA"), ("     D     ", " A", "BB", "CC", "DA"), 40 };
        yield return new object[] { ("     D     ", " A", "BB", "CC", "DA"), ("     D D   ", " A", "BB", "CC", " A"), 2000 };
        yield return new object[] { ("     D D   ", " A", "BB", "CC", " A"), ("     D D A ", " A", "BB", "CC", "  "), 3 };
        yield return new object[] { ("     D D A ", " A", "BB", "CC", "  "), ("     D   A ", " A", "BB", "CC", " D"), 3000 };
        yield return new object[] { ("     D   A ", " A", "BB", "CC", " D"), ("         A ", " A", "BB", "CC", "DD"), 4000 };
        yield return new object[] { ("         A ", " A", "BB", "CC", "DD"), ("           ", "AA", "BB", "CC", "DD"), 8 };
    }

    [Theory]
    [MemberData(nameof(Costs))]
    public void Y2021_Day23_Cost_Returns_Correct_Value(
    (string Hallway, string Amber, string Bronze, string Copper, string Desert) x,
    (string Hallway, string Amber, string Bronze, string Copper, string Desert) y,
    int expected)
    {
        // Act
        int actual = Day23.Cost(x, y);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData(false, 12521)]
    [InlineData(true, 44169)]
    public void Y2021_Day23_Organize_Returns_Correct_Value(bool unfoldDiagram, int expected)
    {
        // Arrange
        string[] diagram =
        {
            "#############",
            "#...........#",
            "###B#C#B#D###",
            "  #A#D#C#A#",
            "  #########",
        };

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
