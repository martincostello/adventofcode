// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day24Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData(3, 99)]
    [InlineData(4, 44)]
    public static void Y2015_Day24_GetQuantumEntanglementOfIdealConfiguration(
        int compartments,
        int expected)
    {
        // Arrange
        List<long> weights = [1, 2, 3, 4, 5, 7, 8, 9, 10, 11];

        // Act
        long actual = Day24.GetQuantumEntanglementOfIdealConfiguration(compartments, weights);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact(Skip = "Too slow.")]
    public async Task Y2015_Day24_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day24>();

        // Assert
        puzzle.Solution1.ShouldBe(11266889531);
        puzzle.Solution2.ShouldBe(77387711);
    }
}
