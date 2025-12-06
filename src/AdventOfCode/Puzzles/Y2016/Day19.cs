// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/19</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 19, "An Elephant Named Joseph", MinimumArguments = 1, IsSlow = true)]
public sealed class Day19 : Puzzle<int, int>
{
    /// <summary>
    /// Finds the elf that receives all of the presents.
    /// </summary>
    /// <param name="count">The number of elves participating.</param>
    /// <param name="version">The version of the rules to use.</param>
    /// <returns>
    /// The number of the elf that receives all the presents.
    /// </returns>
    internal static int FindElfThatGetsAllPresents(int count, int version) =>
        version == 2 ?
        FindElfThatGetsAllPresentsV2(count) :
        FindElfThatGetsAllPresentsV1(count);

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        int count = Parse<int>(args[0]);

        Solution1 = FindElfThatGetsAllPresents(count, version: 1);
        Solution2 = FindElfThatGetsAllPresents(count, version: 2);

        if (Verbose)
        {
            Logger.WriteLine($"The elf that gets all the presents using version 1 of the rules is {Solution1:N0}.");
            Logger.WriteLine($"The elf that gets all the presents using version 2 of the rules is {Solution2:N0}.");
        }

        return Result();
    }

    /// <summary>
    /// Finds the elf that receives all of the presents using version 1 of the rules.
    /// </summary>
    /// <param name="count">The number of elves participating.</param>
    /// <returns>
    /// The number of the elf that receives all the presents.
    /// </returns>
    private static int FindElfThatGetsAllPresentsV1(int count)
    {
        var circle = new LinkedList<int>(Enumerable.Range(1, count));
        var current = circle.First!;

        while (circle.Count > 1)
        {
            circle.Remove(circle.Clockwise(current));
            current = circle.Clockwise(current);
        }

        return circle.First!.Value;
    }

    /// <summary>
    /// Finds the elf that receives all of the presents using version 2 of the rules.
    /// </summary>
    /// <param name="count">The number of elves participating.</param>
    /// <returns>
    /// The number of the elf that receives all the presents.
    /// </returns>
    private static int FindElfThatGetsAllPresentsV2(int count)
    {
        var circle = new LinkedList<int>(Enumerable.Range(1, count));
        var current = circle.First!;
        var opposite = current;

        int steps = circle.Count / 2;

        for (int i = 0; i < steps; i++)
        {
            opposite = circle.Clockwise(opposite);
        }

        while (circle.Count > 1)
        {
            var next = circle.Clockwise(opposite);

            if (circle.Count % 2 == 1)
            {
                next = circle.Clockwise(next);
            }

            circle.Remove(opposite);

            opposite = next;

            current = circle.Clockwise(current);
        }

        return circle.First!.Value;
    }
}
