// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day10Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2023_Day10_Walk_Returns_Correct_Value_Simple()
    {
        // Arrange
        string[] sketch =
        [
            ".....",
            ".S-7.",
            ".|.|.",
            ".L-J.",
            ".....",
        ];

        // Act
        (int actualSteps, int actualTiles) = Day10.Walk(sketch);

        // Assert
        actualSteps.ShouldBe(4);
        actualTiles.ShouldBe(1);
    }

    [Fact]
    public void Y2023_Day10_Walk_Returns_Correct_Value_Complex()
    {
        // Arrange
        string[] sketch =
        [
            "7-F7-",
            ".FJ|7",
            "SJLL7",
            "|F--J",
            "LJ.LJ",
        ];

        // Act
        (int actualSteps, int actualTiles) = Day10.Walk(sketch);

        // Assert
        actualSteps.ShouldBe(8);
        actualTiles.ShouldBe(1);
    }

    [Fact]
    public void Y2023_Day10_Walk_Returns_Correct_Value_For_Tiles_1()
    {
        // Arrange
        string[] sketch =
        [
            "...........",
            ".S-------7.",
            ".|F-----7|.",
            ".||.....||.",
            ".||.....||.",
            ".|L-7.F-J|.",
            ".|..|.|..|.",
            ".L--J.L--J.",
            "...........",
        ];

        // Act
        (_, int actual) = Day10.Walk(sketch);

        // Assert
        actual.ShouldBe(4);
    }

    [Fact]
    public void Y2023_Day10_Walk_Returns_Correct_Value_For_Tiles_2()
    {
        // Arrange
        string[] sketch =
        [
            "..........",
            ".S------7.",
            ".|F----7|.",
            ".||....||.",
            ".||....||.",
            ".|L-7F-J|.",
            ".|..||..|.",
            ".L--JL--J.",
            "..........",
        ];

        // Act
        (_, int actual) = Day10.Walk(sketch);

        // Assert
        actual.ShouldBe(4);
    }

    [Fact]
    public void Y2023_Day10_Walk_Returns_Correct_Value_For_Tiles_3()
    {
        // Arrange
        string[] sketch =
        [
            ".F----7F7F7F7F-7....",
            ".|F--7||||||||FJ....",
            ".||.FJ||||||||L7....",
            "FJL7L7LJLJ||LJ.L-7..",
            "L--J.L7...LJS7F-7L7.",
            "....F-J..F7FJ|L7L7L7",
            "....L7.F7||L7|.L7L7|",
            ".....|FJLJ|FJ|F7|.LJ",
            "....FJL-7.||.||||...",
            "....L---J.LJ.LJLJ...",
        ];

        // Act
        (_, int actual) = Day10.Walk(sketch);

        // Assert
        actual.ShouldBe(8);
    }

    [Fact]
    public void Y2023_Day10_Walk_Returns_Correct_Value_For_Tiles_4()
    {
        // Arrange
        string[] sketch =
        [
            "FF7FSF7F7F7F7F7F---7",
            "L|LJ||||||||||||F--J",
            "FL-7LJLJ||||||LJL-77",
            "F--JF--7||LJLJ7F7FJ-",
            "L---JF-JLJ.||-FJLJJ7",
            "|F|F-JF---7F7-L7L|7|",
            "|FFJF7L7F-JF7|JL---7",
            "7-L-JL7||F7|L7F-7F7|",
            "L.L7LFJ|||||FJL7||LJ",
            "L7JLJL-JLJLJL--JLJ.L",
        ];

        // Act
        (_, int actual) = Day10.Walk(sketch);

        // Assert
        actual.ShouldBe(10);
    }

    [Fact]
    public async Task Y2023_Day10_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day10>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Steps.ShouldBe(6870);
        puzzle.Tiles.ShouldBe(287);
    }
}
