// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class containing tests for the <see cref="Day19"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day19Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day19Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day19Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

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
