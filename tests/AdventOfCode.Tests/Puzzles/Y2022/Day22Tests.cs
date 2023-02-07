// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day22"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day22Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day22Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day22Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2022_Day22_GetFinalPassword_Returns_Correct_Value()
    {
        // Arrange
        string[] map = new[]
        {
            "        ...#",
            "        .#..",
            "        #...",
            "        ....",
            "...#.......#",
            "........#...",
            "..#....#....",
            "..........#.",
            "        ...#....",
            "        .....#..",
            "        .#......",
            "        ......#.",
            string.Empty,
            "10R5L5R10L4R5L5",
        };

        // Act
        int actual = Day22.GetFinalPassword(map);

        // Assert
        actual.ShouldBe(6032);
    }

    [Fact]
    public async Task Y2022_Day22_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day22>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.FinalPassword.ShouldBe(-1);
    }
}
