// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day08Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact(Skip = "Not implemented.")]
    public void Y2025_Day08_Connect_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "162,817,812",
            "57,618,57",
            "906,360,560",
            "592,479,940",
            "352,342,300",
            "466,668,158",
            "542,29,236",
            "431,825,988",
            "739,650,466",
            "52,470,668",
            "216,146,977",
            "819,987,18",
            "117,168,530",
            "805,96,715",
            "346,949,466",
            "970,615,88",
            "941,993,340",
            "862,61,35",
            "984,92,344",
            "425,690,689",
        ];

        // Act
        int actual = Day08.Connect(values);

        // Assert
        actual.ShouldBe(40);
    }

    [Fact(Skip = "Not implemented.")]
    public async Task Y2025_Day08_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day08>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Solution.ShouldBe(Puzzle.Unsolved);
    }
}
