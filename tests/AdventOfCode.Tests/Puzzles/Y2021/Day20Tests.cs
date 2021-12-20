// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

public sealed class Day20Tests : PuzzleTest
{
    public Day20Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 24)]
    [InlineData(2, 35)]
    public void Y2021_Day20_Enhance_Returns_Correct_Value(int enhancements, int expected)
    {
        // Arrange
        string[] algorithm =
        {
            "..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..##",
            "#..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###",
            ".######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#.",
            ".#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#.....",
            ".#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#..",
            "...####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.....",
            "..##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#",
        };

        string[] image =
        {
            string.Join(string.Empty, algorithm),
            string.Empty,
            "#..#.",
            "#....",
            "##..#",
            "..#..",
            "..###",
        };

        // Act
        (int actual, _) = Day20.Enhance(image, enhancements, Logger);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2021_Day20_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day20>();

        // Assert
        puzzle.LitPixelCount.ShouldBe(-1); // 5843 > x > 5424
    }
}
