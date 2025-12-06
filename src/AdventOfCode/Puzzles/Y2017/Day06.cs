// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/6</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 06, "Memory Reallocation", RequiresData = true)]
public sealed class Day06 : Puzzle<int, int>
{
    /// <summary>
    /// Debugs the specified memory to find the number of cycles performed before a distribution is repeated.
    /// </summary>
    /// <param name="memory">The memory to debug.</param>
    /// <returns>
    /// The number of redistribution cycles that must be completed before a configuration is repeated and
    /// the number of loops in the infinite loop cycle caused by the distribution algorithm.
    /// </returns>
    public static (int CycleCount, int LoopSize) Debug(IList<int> memory)
    {
        int[] copy = [.. memory];

        int cycles = GetRepeatCount(copy);
        int loopSize = GetRepeatCount(copy);

        return (cycles, loopSize);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var memory = (await ReadResourceAsStringAsync(cancellationToken)).Trim().AsNumbers<int>('\t');

        (Solution1, Solution2) = Debug(memory);

        if (Verbose)
        {
            Logger.WriteLine($"{Solution1:N0} redistribution cycles must be completed before a configuration is produced that has been seen before.");
            Logger.WriteLine($"{Solution2:N0} cycles are in the infinite loop that arises from the configuration in the input.");
        }

        return Result();
    }

    /// <summary>
    /// Gets the number of cycles before the memory configuration repeats.
    /// </summary>
    /// <param name="memory">The memory to get the number for.</param>
    /// <returns>
    /// The number of times the memory can be redistributed before the configuration repeats.
    /// </returns>
    private static int GetRepeatCount(int[] memory)
    {
        var patterns = new HashSet<string>();
        string pattern = string.Join(',', memory);

        int count = 0;

        do
        {
            patterns.Add(pattern);

            Redistribute(memory);

            count++;
            pattern = string.Join(',', memory);
        }
        while (!patterns.Contains(pattern));

        return count;
    }

    /// <summary>
    /// Redistributes the memory.
    /// </summary>
    /// <param name="memory">The memory to redistribute.</param>
    private static void Redistribute(int[] memory)
    {
        int blocks = memory.Max();
        int index = Array.IndexOf(memory, blocks);

        memory[index] = 0;
        int next = index + 1;

        while (blocks > 0)
        {
            if (next >= memory.Length)
            {
                next = 0;
            }

            memory[next++]++;
            blocks--;
        }
    }
}
