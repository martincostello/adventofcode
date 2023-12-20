// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day20Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    public static TheoryData<string[], int, int> Examples()
    {
        var result = new TheoryData<string[], int, int>();

        string[] configuration =
        [
            "broadcaster -> a, b, c",
            "%a -> b",
            "%b -> c",
            "%c -> inv",
            "&inv -> a",
        ];

        result.Add(configuration, 1, 32);
        result.Add(configuration, 1000, 32000000);

        configuration =
        [
            "broadcaster -> a",
            "%a -> inv, con",
            "&inv -> b",
            "%b -> con",
            "&con -> output",
        ];

        result.Add(configuration, 1000, 11687500);

        return result;
    }

    [Theory]
    [MemberData(nameof(Examples))]
    public void Y2023_Day20_Run_Returns_Correct_Value_Example_1(string[] configuration, int presses, int expected)
    {
        // Act
        int actual = Day20.Run(configuration, presses);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public async Task Y2023_Day20_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day20>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.PulsesProduct.ShouldBeGreaterThan(650330673);
        puzzle.PulsesProduct.ShouldBeLessThan(738980592);
        puzzle.PulsesProduct.ShouldBe(-1);
    }
}
