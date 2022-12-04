// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class containing tests for the <see cref="Day01"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day01Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day01Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day01Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Theory]
    [InlineData(12, false, 2)]
    [InlineData(14, false, 2)]
    [InlineData(1969, false, 654)]
    [InlineData(100756, false, 33583)]
    [InlineData(12, true, 2)]
    [InlineData(1969, true, 966)]
    [InlineData(100756, true, 50346)]
    public static void Y2019_Day01_GetFuelRequirementsForMass_Returns_Correct_Value(int mass, bool includeFuel, int expected)
    {
        // Act
        int actual = Day01.GetFuelRequirementsForMass(mass, includeFuel);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2019_Day01_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day01>();

        // Assert
        puzzle.TotalFuelRequiredForModules.ShouldBe(3226407);
        puzzle.TotalFuelRequiredForRocket.ShouldBe(4836738);
    }
}
