﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/5</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 05, "A Maze of Twisty Trampolines, All Alike", RequiresData = true)]
public sealed class Day05 : Puzzle
{
    /// <summary>
    /// Gets the number of steps required to exit the input instructions for version 1 of the CPU.
    /// </summary>
    public int StepsToExitV1 { get; private set; }

    /// <summary>
    /// Gets the number of steps required to exit the input instructions for version 2 of the CPU.
    /// </summary>
    public int StepsToExitV2 { get; private set; }

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
        int[] jumps = program.ToArray();

        int counter = 0;
        int index = 0;

        while (index >= 0 && index < jumps.Length)
        {
            int offset = jumps[index];

            if (version == 2 && offset >= 3)
            {
                jumps[index] = offset - 1;
            }
            else
            {
                jumps[index] = offset + 1;
            }

            index += offset;
            counter++;
        }

        return counter;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var program = await ReadResourceAsNumbersAsync<int>(cancellationToken);

        StepsToExitV1 = Execute(program, 1);
        StepsToExitV2 = Execute(program, 2);

        if (Verbose)
        {
            Logger.WriteLine($"It takes {StepsToExitV1:N0} to reach the exit using version 1.");
            Logger.WriteLine($"It takes {StepsToExitV2:N0} to reach the exit using version 2.");
        }

        return PuzzleResult.Create(StepsToExitV1, StepsToExitV2);
    }
}
