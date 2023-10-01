// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day24Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2015_Day24_GetQuantumEntanglementOfIdealConfiguration()
    {
        // Arrange
        int compartments = 3;
        int[] weights = [1, 2, 3, 4, 5, 7, 8, 9, 10, 11];

        // Act
        long actual = Day24.GetQuantumEntanglementOfIdealConfiguration(compartments, weights);

        // Assert
        actual.ShouldBe(99);

        // Arrange
        compartments = 4;

        // Act
        actual = Day24.GetQuantumEntanglementOfIdealConfiguration(compartments, weights);

        // Assert
        actual.ShouldBe(44);
    }

    [Theory(Skip = "Too slow.")]
    [InlineData(new string[0], 11266889531)]
    [InlineData(new string[] { "4" }, 77387711)]
    public async Task Y2015_Day24_Solve_Returns_Correct_Solution(string[] args, long expected)
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day24>(args);

        // Assert
        puzzle.QuantumEntanglementOfFirstGroup.ShouldBe(expected);
    }
}
