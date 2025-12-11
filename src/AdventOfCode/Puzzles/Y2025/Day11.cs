// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/11</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 11, "Reactor", RequiresData = true)]
public sealed class Day11 : Puzzle<int>
{
    /// <summary>
    /// Counts the number of different paths that lead from <c>you</c> to <c>out</c>.
    /// </summary>
    /// <param name="devices">The devices to solve the puzzle from.</param>
    /// <returns>
    /// The solution.
    /// </returns>
    public static int CountPaths(IReadOnlyList<string> devices)
    {
        var connections = new Dictionary<string, Device>();

        foreach (string line in devices)
        {
            int index = line.IndexOf(':', StringComparison.Ordinal);
            string name = line[..index];
            string rest = line[(index + 1)..];

            var outputs = new List<string>();

            foreach (var range in rest.AsSpan().Split(' '))
            {
                outputs.Add(rest[range]);
            }

            connections[name] = new(name, outputs);
        }

        var alternate = connections.GetAlternateLookup();

        return CountPaths("you", "out", alternate);

        static int CountPaths(
            ReadOnlySpan<char> origin,
            ReadOnlySpan<char> destination,
            Dictionary<string, Device>.AlternateLookup<ReadOnlySpan<char>> connections)
        {
            if (origin.SequenceEqual(destination))
            {
                return 1;
            }

            int total = 0;

            if (connections.TryGetValue(origin, out var device))
            {
                foreach (string to in device.Connections)
                {
                    total += CountPaths(to, destination, connections);
                }
            }

            return total;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, _) =>
            {
                int solution = CountPaths(values);

                if (logger is { })
                {
                    logger.WriteLine("{0} different paths lead from you to out.", solution);
                }

                return solution;
            },
            cancellationToken);
    }

    private sealed record Device(string Name, IReadOnlyList<string> Connections);
}
