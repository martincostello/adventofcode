// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using LocationMap = System.Collections.Generic.Dictionary<string, (string Destination, System.Collections.Generic.List<(long Destination, long Source, int Length)> Values)>;

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
    /// Parses the specified almanac and returns the lowest location number
    /// that corresponds to any of the initial seed numbers.
    /// </summary>
    /// <param name="almanac">The almanac to parse.</param>
    /// <returns>
    /// The lowest location number that corresponds to any of the initial seed numbers.
    /// </returns>
    public static long Parse(IList<string> almanac)
    {
        var seeds = almanac[0]["seeds: ".Length..].Split(' ').Select(Parse<long>).ToList();
        var seedLocations = new Dictionary<long, long>();

        LocationMap map = [];
        (string Destination, List<(long Destination, long Source, int Length)> Values) ranges = default;

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
                ranges.Values!.Add((Parse<long>(destination), Parse<long>(source), Parse<int>(length)));
            }
        }

        foreach (long seed in seeds)
        {
            seedLocations[seed] = FindValue("seed", seed, "location", map);
        }

        return seedLocations.Values.Min();

        static long FindValue(string key, long value, string destinationKey, LocationMap map)
        {
            (string nextKey, var ranges) = map[key];

            var (destinationIndex, sourceIndex, length) = ranges.Find((p) => value >= p.Source && value <= p.Source + p.Length);

            long offset = value - sourceIndex;
            long destinationValue = destinationIndex + offset;

            if (nextKey == destinationKey)
            {
                return destinationValue;
            }

            return FindValue(nextKey, destinationValue, destinationKey, map);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var almanac = await ReadResourceAsLinesAsync(cancellationToken);

        LocationMinimum = Parse(almanac);

        if (Verbose)
        {
            Logger.WriteLine("The lowest location number that corresponds to any of the initial seed numbers is {0}.", LocationMinimum);
        }

        return PuzzleResult.Create(LocationMinimum);
    }
}
