// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

public sealed class Day05Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 10)]
    public static void Y2017_Day05_Execute_Returns_Correct_Value(int version, int expected)
    {
        // Arrange
        int[] program = [0, 3, 0, 1, -3];

        // Act
        int actual = Day05.Execute(program, version);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2017_Day05_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day05>();

        // Assert
        puzzle.StepsToExitV1.ShouldBe(373543);
        puzzle.StepsToExitV2.ShouldBe(27502966);
    }
}
