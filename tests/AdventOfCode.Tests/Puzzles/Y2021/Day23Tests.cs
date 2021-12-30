// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day23Tests : PuzzleTest
{
    public Day23Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2021_Day23_Organize_Returns_Correct_Value()
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
        int actual = Day23.Organize(diagram);

        // Assert
        actual.ShouldBe(12521);
    }

    [Fact]
    public void Y2021_Day23_Cost_Returns_Correct_Value_1()
    {
        // Arrange
        var x = ("           ", "BA", "CD", "BC", "DA");
        var y = ("   B       ", "BA", "CD", " C", "DA");

        // Act
        int actual = Day23.Cost(x, y);

        // Assert
        actual.ShouldBe(40);
    }

    [Fact]
    public void Y2021_Day23_Cost_Returns_Correct_Value_2()
    {
        // Arrange
        var x = ("   B       ", "BA", "CD", " C", "DA");
        var y = ("   B       ", "BA", " D", "CC", "DA");

        // Act
        int actual = Day23.Cost(x, y);

        // Assert
        actual.ShouldBe(400);
    }

    [Fact]
    public void Y2021_Day23_Cost_Returns_Correct_Value_3()
    {
        // Arrange
        var x = ("   B       ", "BA", " D", "CC", "DA");
        var y = ("   B D     ", "BA", "  ", "CC", "DA");

        // Act
        int actual = Day23.Cost(x, y);

        // Assert
        actual.ShouldBe(3000);
    }

    [Fact]
    public void Y2021_Day23_Cost_Returns_Correct_Value_4()
    {
        // Arrange
        var x = ("   B D     ", "BA", "  ", "CC", "DA");
        var y = ("     D     ", "BA", " B", "CC", "DA");

        // Act
        int actual = Day23.Cost(x, y);

        // Assert
        actual.ShouldBe(30);
    }

    [Fact]
    public void Y2021_Day23_Cost_Returns_Correct_Value_5()
    {
        // Arrange
        var x = ("     D     ", "BA", " B", "CC", "DA");
        var y = ("     D     ", " A", "BB", "CC", "DA");

        // Act
        int actual = Day23.Cost(x, y);

        // Assert
        actual.ShouldBe(40);
    }

    [Fact]
    public void Y2021_Day23_Cost_Returns_Correct_Value_6()
    {
        // Arrange
        var x = ("     D    ", " A", "BB", "CC", "DA");
        var y = ("     D D  ", " A", "BB", "CC", " A");

        // Act
        int actual = Day23.Cost(x, y);

        // Assert
        actual.ShouldBe(2000);
    }

    [Fact]
    public void Y2021_Day23_Cost_Returns_Correct_Value_7()
    {
        // Arrange
        var x = ("     D D   ", " A", "BB", "CC", " A");
        var y = ("     D D A ", " A", "BB", "CC", "  ");

        // Act
        int actual = Day23.Cost(x, y);

        // Assert
        actual.ShouldBe(3);
    }

    [Fact]
    public void Y2021_Day23_Cost_Returns_Correct_Value_8()
    {
        // Arrange
        var x = ("     D D A ", " A", "BB", "CC", "  ");
        var y = ("     D   A ", " A", "BB", "CC", " D");

        // Act
        int actual = Day23.Cost(x, y);

        // Assert
        actual.ShouldBe(3000);
    }

    [Fact]
    public void Y2021_Day23_Cost_Returns_Correct_Value_9()
    {
        // Arrange
        var x = ("     D   A ", " A", "BB", "CC", " D");
        var y = ("         A ", " A", "BB", "CC", "DD");

        // Act
        int actual = Day23.Cost(x, y);

        // Assert
        actual.ShouldBe(4000);
    }

    [Fact]
    public void Y2021_Day23_Cost_Returns_Correct_Value_10()
    {
        // Arrange
        var x = ("         A ", " A", "BB", "CC", "DD");
        var y = ("           ", "AA", "BB", "CC", "DD");

        // Act
        int actual = Day23.Cost(x, y);

        // Assert
        actual.ShouldBe(8);
    }

    [Fact]
    public async Task Y2021_Day23_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day23>();

        // Assert
        puzzle.MinimumEnergy.ShouldBe(-1);
    }
}
