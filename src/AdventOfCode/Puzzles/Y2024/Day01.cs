// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/1</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 01, "Historian Hysteria", RequiresData = true)]
public sealed class Day01 : Puzzle
{
    /// <summary>
    /// Gets the total distance between the values in the list.
    /// </summary>
    public int TotalDistance { get; private set; }

    /// <summary>
    /// Gets the total distance between the values in the list.
    /// </summary>
    /// <param name="values">The list of values to get the total distance for.</param>
    /// <returns>
    /// The total distance between the values in the list.
    /// </returns>
    public static int GetTotalDistance(IList<string> values)
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

        for (int i = 0; i < first.Count; i++)
        {
            total += Math.Abs(first[i] - second[i]);
        }

        return total;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        TotalDistance = GetTotalDistance(values);

        if (Verbose)
        {
            Logger.WriteLine("{0}", TotalDistance);
        }

        return PuzzleResult.Create(TotalDistance);
    }
}
