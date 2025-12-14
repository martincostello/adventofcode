// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/14</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 14, "Extended Polymerization", RequiresData = true)]
public sealed class Day14 : Puzzle<long, long>
{
    /// <summary>
    /// Expands the polymer template as specified by the instructions.
    /// </summary>
    /// <param name="instructions">The instructions to expand a polymer from.</param>
    /// <param name="steps">The number of steps to expand the polymer by.</param>
    /// <returns>
    /// The polymer's score after expansion.
    /// </returns>
    public static long Expand(IList<string> instructions, int steps)
    {
        string template = instructions[0];

        var insertions = new Dictionary<string, char>(instructions.Count - 2);
        var alternate = insertions.GetAlternateLookup();

        foreach (string instruction in instructions.Skip(2))
        {
            instruction.AsSpan().Bifurcate(" -> ", out var pair, out var element);
            alternate[pair] = element[0];
        }

        var pairCounts = new Dictionary<string, long>();

        for (int i = 0; i < template.Length - 1; i++)
        {
            char first = template[i];
            char second = template[i + 1];

            string pair = first + string.Empty + second;

            pairCounts.AddOrIncrement(pair, 1);
        }

        for (int i = 1; i <= steps; i++)
        {
            foreach ((string pair, long count) in pairCounts.ToArray())
            {
                if (count > 0 && insertions.TryGetValue(pair, out char element))
                {
                    pairCounts[pair] -= count;

                    string pair1 = $"{pair[0]}{element}";
                    string pair2 = $"{element}{pair[1]}";

                    pairCounts.AddOrIncrement(pair1, count, count);
                    pairCounts.AddOrIncrement(pair2, count, count);
                }
            }
        }

        var frequencies = new Dictionary<char, long>();

        foreach ((string pair, long count) in pairCounts)
        {
            frequencies.AddOrIncrement(pair[0], count, count);
            frequencies.AddOrIncrement(pair[1], count, count);
        }

        long max = frequencies.Values.Max();
        long min = frequencies.Values.Min();

        return (max - min + 1) / 2;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (instructions, logger, _) =>
            {
                long score10 = Expand(instructions, steps: 10);
                long score40 = Expand(instructions, steps: 40);

                if (logger is { })
                {
                    logger.WriteLine("The \"score\" of the polymer after 10 steps of expansion is {0:N0}.", score10);
                    logger.WriteLine("The \"score\" of the polymer after 40 steps of expansion is {0:N0}.", score40);
                }

                return (score10, score40);
            },
            cancellationToken);
    }
}
