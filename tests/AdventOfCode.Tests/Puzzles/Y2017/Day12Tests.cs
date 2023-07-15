// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

public sealed class Day12Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Fact]
    public static void Y2017_Day12_GetProgramsInGroup_Returns_Correct_Value()
    {
        // Arrange
        int programId = 0;

        string[] pipes = new[]
        {
            "0 <-> 2",
            "1 <-> 1",
            "2 <-> 0, 3, 4",
            "3 <-> 2, 4",
            "4 <-> 2, 3, 6",
            "5 <-> 6",
            "6 <-> 4, 5",
        };

        // Act
        int actual = Day12.GetProgramsInGroup(programId, pipes);

        // Assert
        actual.ShouldBe(6);
    }

    [Fact]
    public static void Y2017_Day12_GetGroupsInNetwork_Returns_Correct_Value()
    {
        // Arrange
        string[] pipes = new[]
        {
            "0 <-> 2",
            "1 <-> 1",
            "2 <-> 0, 3, 4",
            "3 <-> 2, 4",
            "4 <-> 2, 3, 6",
            "5 <-> 6",
            "6 <-> 4, 5",
        };

        // Act
        int actual = Day12.GetGroupsInNetwork(pipes);

        // Assert
        actual.ShouldBe(2);
    }

    [Fact]
    public async Task Y2017_Day12_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day12>();

        // Assert
        puzzle.ProgramsInGroupOfProgram0.ShouldBe(113);
        puzzle.NumberOfGroups.ShouldBe(202);
    }
}
