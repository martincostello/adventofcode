// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

public sealed class Day12Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2016_Day12_Process_Returns_Correct_Solution()
    {
        // Arrange
        string[] instructions = new[]
        {
            "cpy 41 a",
            "inc a",
            "inc a",
            "dec a",
            "jnz a 2",
            "dec a",
        };

        int[] binsOfInterest = new[] { 0, 1, 2 };

        // Act
        var actual = Day12.Process(instructions, initialValueOfC: 0);

        // Assert
        actual.ShouldNotBeNull();
        actual.Count.ShouldBe(4);
        actual.ShouldContainKeyAndValue('a', 42);
        actual.ShouldContainKeyAndValue('b', 0);
        actual.ShouldContainKeyAndValue('c', 0);
        actual.ShouldContainKeyAndValue('d', 0);
    }

    [Fact]
    public static void Y2016_Day12_Process_With_Toggle_Returns_Correct_Solution()
    {
        // Arrange
        string[] instructions = new[]
        {
            "cpy 2 a",
            "tgl a",
            "tgl a",
            "tgl a",
            "cpy 1 a",
            "dec a",
            "dec a",
        };

        int[] binsOfInterest = new[] { 0, 1, 2 };

        // Act
        var actual = Day12.Process(instructions, initialValueOfC: 0);

        // Assert
        actual.ShouldNotBeNull();
        actual.Count.ShouldBe(4);
        actual.ShouldContainKeyAndValue('a', 3);
        actual.ShouldContainKeyAndValue('b', 0);
        actual.ShouldContainKeyAndValue('c', 0);
        actual.ShouldContainKeyAndValue('d', 0);
    }

    [Fact]
    public async Task Y2016_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.ValueInRegisterA.ShouldBe(318020);
        puzzle.ValueInRegisterAWhenInitializedWithIgnitionKey.ShouldBe(9227674);
    }
}
