﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day16Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    public static TheoryData<string[], int> Examples()
    {
        return new()
        {
            {
                new[]
                {
                    "###############",
                    "#.......#....E#",
                    "#.#.###.#.###.#",
                    "#.....#.#...#.#",
                    "#.###.#####.#.#",
                    "#.#.#.......#.#",
                    "#.#.#####.###.#",
                    "#...........#.#",
                    "###.#.#####.#.#",
                    "#...#.....#.#.#",
                    "#.#.#.###.#.#.#",
                    "#.....#...#.#.#",
                    "#.###.#.#.#.#.#",
                    "#S..#.....#...#",
                    "###############",
                },
                7036
            },
            {
                new[]
                {
                    "#################",
                    "#...#...#...#..E#",
                    "#.#.#.#.#.#.#.#.#",
                    "#.#.#.#...#...#.#",
                    "#.#.#.#.###.#.#.#",
                    "#...#.#.#.....#.#",
                    "#.#.#.#.#.#####.#",
                    "#.#...#.#.#.....#",
                    "#.#.#####.#.###.#",
                    "#.#.#.......#...#",
                    "#.#.###.#####.###",
                    "#.#.#...#.....#.#",
                    "#.#.#.#####.###.#",
                    "#.#.#.........#.#",
                    "#.#.#.#########.#",
                    "#S#.............#",
                    "#################",
                },
                11048
            },
        };
    }

    [Theory]
    [MemberData(nameof(Examples))]
    public void Y2024_Day16_Solve_Returns_Correct_Value(string[] values, int expected)
    {
        // Arrange
        using var cts = Timeout();

        // Act
        int actual = Day16.Race(values, cts.Token);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2024_Day16_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day16>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.WinningScore.ShouldBe(90440);
    }
}
