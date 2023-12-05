// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using LocationMap = System.Collections.Generic.Dictionary<string, (string Destination, System.Collections.Generic.List<(long Destination, long Source, long Length)> Values)>;

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/05</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 05, "If You Give A Seed A Fertilizer", RequiresData = true)]
public sealed class Day05 : Puzzle
{
    /// <summary>
    /// Gets the lowest location number that corresponds to any of the initial seed numbers.
    /// </summary>
    public long LocationMinimum { get; private set; }

    /// <summary>
    /// Gets the lowest location number that corresponds to any of the initial seed numbers when using ranges.
    /// </summary>
    public long LocationMinimumWithRanges { get; private set; }

    /// <summary>
    /// Parses the specified almanac and returns the lowest location number
    /// that corresponds to any of the initial seed numbers.
    /// </summary>
    /// <param name="almanac">The almanac to parse.</param>
    /// <param name="useRanges">Whether to parse the seeds as pairs of ranges rather than individual seed numbers.</param>
    /// <param name="cancellationToken">The optional cancellation token to use.</param>
    /// <returns>
    /// The lowest location number that corresponds to any of the initial seed numbers.
    /// </returns>
    public static long Parse(IList<string> almanac, bool useRanges, CancellationToken cancellationToken = default)
    {
        var seeds = almanac[0]["seeds: ".Length..].Split(' ').Select((p) => (Parse<long>(p), 1L)).ToList();

        if (useRanges)
        {
            var lengths = new List<long>(seeds.Count / 2);

            for (int i = 0; i < seeds.Count; i++)
            {
                long seed = seeds[i].Item1;
                long length = seeds[i + 1].Item1;
                seeds.RemoveAt(i + 1);
                seeds[i] = (seed, length);
            }

            seeds.TrimExcess();
        }

        var seedLocations = new Dictionary<long, long>();

        LocationMap map = [];
        (string Destination, List<(long Destination, long Source, long Length)> Values) ranges = default;

        foreach (string value in almanac.Skip(2))
        {
            var line = value.AsSpan();

            if (line.IsEmpty)
            {
                continue;
            }

            const string MapPrefix = " map:";

            if (line.EndsWith(MapPrefix, StringComparison.Ordinal))
            {
                var key = line[..^MapPrefix.Length];
                key.Trifurcate('-', out var source, out _, out var destination);

                map[new(source)] = ranges = (new(destination), []);
            }
            else
            {
                line.Trifurcate(' ', out var destination, out var source, out var length);
                ranges.Values!.Add((Parse<long>(destination), Parse<long>(source), Parse<long>(length)));
            }
        }

        foreach ((long offset, long length) in seeds)
        {
            seedLocations[offset] = FindValue("seed", (offset, length), "location", map, useRanges, cancellationToken);
        }

        return seedLocations.Values.Min();

        static long FindValue(
            string key,
            (long Offset, long Length) range,
            string destinationKey,
            LocationMap map,
            bool useRanges,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            (string nextKey, var ranges) = map[key];

            HashSet<long> valuesOfInterest = [range.Offset];

            if (useRanges && range.Length > 1)
            {
                long maximum = range.Offset + range.Length - 1;

                foreach (var (destination, source, length) in ranges)
                {
                    if (source >= range.Offset && source + length <= maximum)
                    {
                        valuesOfInterest.Add(source);
                    }
                }
            }

            var values = new HashSet<long>();

            foreach (long value in valuesOfInterest)
            {
                var (destinationIndex, sourceIndex, length) = ranges.Find((p) => value >= p.Source && value <= p.Source + p.Length);

                long offset = value - sourceIndex;
                long minimum = destinationIndex + offset;

                if (nextKey != destinationKey)
                {
                    minimum = FindValue(nextKey, (minimum, useRanges ? length : 1), destinationKey, map, useRanges, cancellationToken);
                }

                values.Add(minimum);
            }

            return values.Min();
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var almanac = await ReadResourceAsLinesAsync(cancellationToken);

        LocationMinimum = Parse(almanac, useRanges: false, cancellationToken);
        LocationMinimumWithRanges = Parse(almanac, useRanges: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The lowest location number that corresponds to any of the initial seed numbers is {0}.", LocationMinimum);
            Logger.WriteLine("The lowest location number that corresponds to any of the initial seed numbers as pairs is {0}.", LocationMinimumWithRanges);
        }

        return PuzzleResult.Create(LocationMinimum, LocationMinimumWithRanges);
    }
}
