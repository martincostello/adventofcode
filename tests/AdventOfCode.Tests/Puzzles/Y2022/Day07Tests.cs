// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class containing tests for the <see cref="Day07"/> class. This class cannot be inherited.
/// </summary>
public sealed class Day07Tests : PuzzleTest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Day07Tests"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    public Day07Tests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void Y2022_Day07_GetTotalSize_Returns_Correct_Value()
    {
        // Arrange
        string[] terminalOutput = new[]
        {
            "$ cd /",
            "$ ls",
            "dir a",
            "14848514 b.txt",
            "8504156 c.dat",
            "dir d",
            "$ cd a",
            "$ ls",
            "dir e",
            "29116 f",
            "2557 g",
            "62596 h.lst",
            "$ cd e",
            "$ ls",
            "584 i",
            "$ cd ..",
            "$ cd ..",
            "$ cd d",
            "$ ls",
            "4060174 j",
            "8033020 d.log",
            "5626152 d.ext",
            "7214296 k",
        };

        // Act
        long actual = Day07.GetTotalSize(terminalOutput, 100000);

        // Assert
        actual.ShouldBe(95437);
    }

    [Fact]
    public async Task Y2022_Day07_Solve_Returns_Correct_Solution()
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day07>();

        // Assert
        puzzle.ShouldNotBeNull();
        puzzle.TotalSizeOfDirectoriesLargerThan100000.ShouldBe(1642503);
    }
}
