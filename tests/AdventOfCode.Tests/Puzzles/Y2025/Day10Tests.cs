// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

public sealed class Day10Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2025_Day10_GetMinimumButtonPresses_Returns_Correct_Value()
    {
        // Arrange
        string[] values =
        [
            "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}",
            "[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}",
            "[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}",
        ];

        // Act
        (int actualIndicator, int actualJoltage) = Day10.GetMinimumButtonPresses(values, TestContext.Current.CancellationToken);

        // Assert
        actualIndicator.ShouldBe(7);
        actualJoltage.ShouldBe(33);
    }

    [Fact(Skip = "Too slow.")]
    public async Task Y2025_Day10_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day10>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.Solution1.ShouldBe(524);
        puzzle.Solution2.ShouldBe(Puzzle.Unsolved);
    }
}
