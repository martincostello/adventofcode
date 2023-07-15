// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day19Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2015_Day19_GetPossibleMolecules()
    {
        // Arrange
        string molecule = "HOH";
        List<string> replacements = new() { "H => HO", "H => OH", "O => HH" };

        // Act
        var actual = Day19.GetPossibleMolecules(molecule, replacements, CancellationToken.None);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldBe(new[] { "HHHH", "HOHO", "HOOH", "OHOH" });
    }

    [Fact]
    public void Y2015_Day19_GetMinimumSteps()
    {
        // Arrange
        string molecule = "HOH";
        List<string> replacements = new() { "e => H", "e => O", "H => HO", "H => OH", "O => HH" };

        // Act
        int actual = Day19.GetMinimumSteps(molecule, replacements, Logger, CancellationToken.None);

        // Assert
        actual.ShouldBe(3);
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
