// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/5</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 05, "A Maze of Twisty Trampolines, All Alike", RequiresData = true)]
public sealed class Day05 : Puzzle<int, int>
{
    /// <summary>
    /// Executes the specified program and CPU version.
    /// </summary>
    /// <param name="program">The program to execute represented as CPU jump offsets.</param>
    /// <param name="version">The version of the CPU to use.</param>
    /// <returns>
    /// The value of the program counter when the program terminates.
    /// </returns>
    public static int Execute(IEnumerable<int> program, int version)
    {
        int[] jumps = [.. program];

        int counter = 0;
        int index = 0;

        while (index >= 0 && index < jumps.Length)
        {
            int offset = jumps[index];

            jumps[index] = version == 2 && offset >= 3 ? offset - 1 : offset + 1;

            index += offset;
            counter++;
        }

        return counter;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var program = await ReadResourceAsNumbersAsync<int>(cancellationToken);

        Solution1 = Execute(program, version: 1);
        Solution2 = Execute(program, version: 2);

        if (Verbose)
        {
            Logger.WriteLine($"It takes {Solution1:N0} to reach the exit using version 1.");
            Logger.WriteLine($"It takes {Solution2:N0} to reach the exit using version 2.");
        }

        return Result();
    }
}
