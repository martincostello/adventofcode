// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day09Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2025_Day09_FindLargestRectangle_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "7,1",
            "11,1",
            "11,7",
            "9,7",
            "9,5",
            "2,5",
            "2,3",
            "7,3",
        ];

        // Act
        long actual = Day09.FindLargestRectangle(values);

        // Assert
        actual.ShouldBe(50);
    }

    [Fact]
    public async Task Y2025_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Solution.ShouldBe(4759420470);
    }
}
