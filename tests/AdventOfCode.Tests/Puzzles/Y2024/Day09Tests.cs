// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

public sealed class Day09Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact(Skip = "Not implemented.")]
    public void Y2024_Day09_Defragment_Returns_Correct_Value()
    {
        // Arrange
        string map = "2333133121414131402";

        // Act
        int actual = Day09.Defragment(map);

        // Assert
        actual.ShouldBe(1928);
    }

    [Fact(Skip = "Not implemented.")]
    public async Task Y2024_Day09_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day09>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Checksum.ShouldBe(-1);
    }
}
