// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2020/day/13</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2020, 13, RequiresData = true)]
public sealed class Day13 : Puzzle
{
    /// <summary>
    /// Gets the product of the ID of the earliest bus and the number of minutes to wait for it.
    /// </summary>
    public int BusWaitProduct { get; private set; }

    /// <summary>
    /// Gets the earliest timestamp where all bus IDs depart at offsets matching their positions in the input.
    /// </summary>
    public long EarliestTimestamp { get; private set; }

    /// <summary>
    /// Gets the product of the earliest busand the number of minutes to wait for it.
    /// </summary>
    /// <param name="notes">The notes for the bus schedules.</param>
    /// <returns>
    /// The product of the ID of the earliest bus and the number of minutes to wait for it.
    /// </returns>
    public static int GetEarliestBusWaitProduct(IList<string> notes)
    {
        int timestamp = ParseInt32(notes[0]);

        int[] buses = notes[1]
            .Split(',')
            .Where((p) => !string.Equals(p, "x", StringComparison.Ordinal))
            .Select(ParseInt32)
            .ToArray();

        var nextBusesInMinutes = new Dictionary<int, int>(buses.Length);

        foreach (int id in buses)
        {
            int busesSoFar = timestamp / id;
            int lastBusAt = busesSoFar * id;
            int nextBusAt = lastBusAt + id;

            nextBusesInMinutes[id] = nextBusAt - timestamp;
        }

        var nextBus = nextBusesInMinutes
            .OrderBy((p) => p.Value)
            .Select((p) => new { Id = p.Key, WaitInMinutes = p.Value })
            .First();

        return nextBus.Id * nextBus.WaitInMinutes;
    }

    /// <summary>
    /// Gets the earliest timestamp where all bus IDs depart at offsets
    /// matching their positions in the specified notes.
    /// </summary>
    /// <param name="notes">The notes for the bus schedules.</param>
    /// <returns>
    /// The product of the ID of the earliest bus and the number of minutes to wait for it.
    /// </returns>
    public static long GetEarliestTimestamp(IList<string> notes)
    {
        // Adapted from https://github.com/RaczeQ/AdventOfCode2020/blob/master/Day13/SecondSolver.cs
        // Uses the Chinese Remainder Theorem: https://en.wikipedia.org/wiki/Chinese_remainder_theorem
        var buses = notes[1]
            .Split(',')
            .Select((id, offset) => (id, offset))
            .Where((p) => !string.Equals(p.id, "x", StringComparison.Ordinal))
            .Select((p) => (id: ParseInt64(p.id), p.offset))
            .OrderByDescending((p) => p.id)
            .ToList();

        var largestBus = buses[0];

        long timestamp = largestBus.id - largestBus.offset;
        long period = largestBus.id;

        for (int i = 1; i <= buses.Count; i++)
        {
            while (buses.Take(i).Any((p) => (timestamp + p.offset) % p.id != 0))
            {
                timestamp += period;
            }

            period = buses
                .Take(i)
                .Select((p) => p.id)
                .Aggregate(Maths.LowestCommonMultiple);
        }

        return timestamp;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> notes = await ReadResourceAsLinesAsync();

        BusWaitProduct = GetEarliestBusWaitProduct(notes);
        EarliestTimestamp = GetEarliestTimestamp(notes);

        if (Verbose)
        {
            Logger.WriteLine("The product of the ID of the earliest and the number of minutes to wait is {0}.", BusWaitProduct);
            Logger.WriteLine("The earliest timestamp where all bus IDs depart at the specified offsets is {0}.", EarliestTimestamp);
        }

        return PuzzleResult.Create(BusWaitProduct, EarliestTimestamp);
    }
}
