// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day19Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("HOH", 4)]
    [InlineData("HOHOHO", 7)]
    public static void Y2015_Day19_GetPossibleMolecules(string molecule, int expected)
    {
        // Arrange
        List<(string Source, string Target)> replacements =
        [
            ("H", "HO"),
            ("H", "OH"),
            ("O", "HH"),
        ];

        using var cts = Timeout();

        // Act
        int actual = Day19.GetPossibleMolecules(molecule, replacements, cts.Token);

        // Assert
        actual.ShouldBe(expected);
    }

    [Theory]
    [InlineData("HOH", 3)]
    [InlineData("HOHOHO", 6)]
    public void Y2015_Day19_GetMinimumSteps(string molecule, int expected)
    {
        // Arrange
        List<(string Source, string Target)> replacements =
        [
            ("e", "H"),
            ("e", "O"),
            ("H", "HO"),
            ("H", "OH"),
            ("O", "HH"),
        ];

        using var cts = Timeout();

        // Act
        int actual = Day19.GetMinimumSteps(molecule, replacements, cts.Token);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact(Skip = "Too slow.")]
    public async Task Y2015_Day19_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day19>();

        // Assert
        puzzle.CalibrationSolution.ShouldBe(576);
        puzzle.FabricationSolution.ShouldBe(207);
    }
}
