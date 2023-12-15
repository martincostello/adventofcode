// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

public sealed class Day15Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("HASH", 52)]
    [InlineData("rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7", 1320)]
    public void Y2023_Day15_Hash_Returns_Correct_Value(string initializationSequence, int expected)
    {
        // Act
        int actual = Day15.HashSequence(initializationSequence);

        // Assert
        actual.ShouldBe(expected);
    }

    [Fact]
    public void Y2023_Day15_Initialize_Returns_Correct_Value()
    {
        // Arrange
        string initializationSequence = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";

        // Act
        int actual = Day15.Initialize(initializationSequence);

        // Assert
        actual.ShouldBe(145);
    }

    [Fact]
    public async Task Y2023_Day15_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day15>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.HashSum.ShouldBe(511215);
        puzzle.FocusingPower.ShouldBe(236057);
    }
}
