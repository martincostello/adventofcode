// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class containing tests for the <see cref="Day18"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day18Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day18Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day18Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public static void Y2015_Day18_GetGridConfigurationAfterSteps()
    {
        // Arrange
        string[] initialState = new string[]
        {
            ".#.#.#",
            "...##.",
            "#....#",
            "..#...",
            "#.#..#",
            "####..",
        };

        int steps = 0;
        bool areCornerLightsBroken = false;

        // Act
        IList<string> actual = Day18.GetGridConfigurationAfterSteps(initialState, steps, areCornerLightsBroken);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldBe(
            new[]
            {
                ".#.#.#",
                "...##.",
                "#....#",
                "..#...",
                "#.#..#",
                "####..",
            });

        // Arrange
        steps = 4;

        // Act
        actual = Day18.GetGridConfigurationAfterSteps(initialState, steps, areCornerLightsBroken);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldBe(
            new[]
            {
                "......",
                "......",
                "..##..",
                "..##..",
                "......",
                "......",
            });

        // Arrange
        steps = 5;
        areCornerLightsBroken = true;

        // Act
        actual = Day18.GetGridConfigurationAfterSteps(initialState, steps, areCornerLightsBroken);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldBe(
            new[]
            {
                "##.###",
                ".##..#",
                ".##...",
                ".##...",
                "#.#...",
                "##...#",
            });
    }

    [Fact]
    public async Task Y2015_Day18_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day18>();

        // Assert
        puzzle.LightsIlluminated.ShouldBe(814);
        puzzle.LightsIlluminatedWithStuckLights.ShouldBe(924);
    }
}
