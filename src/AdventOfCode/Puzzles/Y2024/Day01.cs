// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 01, "Historian Hysteria", RequiresData = true)]
public sealed class Day01 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the total distance between the values in the list.
    /// </summary>
    /// <param name="values">The list of values to get the total distance for.</param>
    /// <returns>
    /// The total distance between the values and the
    /// similarity score for the values in the list.
    /// </returns>
    public static (int TotalDistance, int SimilarityScore) ParseList(IList<string> values)
    {
        List<int> first = new(values.Count);
        List<int> second = new(values.Count);

        foreach (string pair in values)
        {
            string[] split = pair.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            first.Add(Parse<int>(split[0]));
            second.Add(Parse<int>(split[1]));
        }

        first.Sort();
        second.Sort();

        int total = 0;
        int similarity = 0;

        for (int i = 0; i < first.Count; i++)
        {
            int left = first[i];
            int right = second[i];

            if (second.IndexOf(left) is int index && index > -1)
            {
                int count = 1;

                if (index < second.Count - 1)
                {
                    while (second[++index] == left)
                    {
                        count++;
                    }
                }

                similarity += left * count;
            }

            total += Math.Abs(left - right);
        }

        return (total, similarity);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        (Solution1, Solution2) = ParseList(values);

        if (Verbose)
        {
            Logger.WriteLine("The total distance between the lists is {0}", Solution1);
            Logger.WriteLine("The similarity score for the lists is {0}", Solution2);
        }

        return Result();
    }
}
