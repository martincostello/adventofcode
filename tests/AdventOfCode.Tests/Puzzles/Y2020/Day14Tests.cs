// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

public sealed class Day14Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public void Y2020_Day14_RunProgram_Returns_Correct_Value_V1()
    {
        // Arrange
        string[] program =
        [
            "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
            "mem[8] = 11",
            "mem[7] = 101",
            "mem[8] = 0",
        ];

        // Act
        long actual = Day14.RunProgram(program, version: 1);

        // Assert
        actual.ShouldBe(165);
    }

    [Fact]
    public void Y2020_Day14_RunProgram_Returns_Correct_Value_V2()
    {
        // Arrange
        string[] program =
        [
            "mask = 000000000000000000000000000000X1001X",
            "mem[42] = 100",
            "mask = 00000000000000000000000000000000X0XX",
            "mem[26] = 1",
        ];

        // Act
        long actual = Day14.RunProgram(program, version: 2);

        // Assert
        actual.ShouldBe(208);
    }

    [Fact]
    public async Task Y2020_Day14_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day14>();

        // Assert
        puzzle.SumOfRemainingValuesV1.ShouldBe(9967721333886L);
        puzzle.SumOfRemainingValuesV2.ShouldBe(4355897790573L);
    }
}
