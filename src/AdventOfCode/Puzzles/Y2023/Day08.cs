// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2023;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2023/day/08</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2023, 08, "Haunted Wasteland", RequiresData = true)]
public sealed class Day08 : Puzzle
{
    /// <summary>
    /// Gets the number of steps required to reach ZZZ.
    /// </summary>
    public long Steps { get; private set; }

    /// <summary>
    /// Gets the number of steps required to simultaneously
    /// reach nodes all ending with Z.
    /// </summary>
    public long StepsAsGhost { get; private set; }

    /// <summary>
    /// Walks the network of nodes and returns the number of steps required to reach ZZZ.
    /// </summary>
    /// <param name="nodes">The description of the network of nodes.</param>
    /// <param name="asGhost">Whether to walk the network as a ghost.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The number of steps required to reach ZZZ, or all nodes to end with Z
    /// if <paramref name="asGhost"/> is <see langword="true"/>.
    /// </returns>
    public static long WalkNetwork(IList<string> nodes, bool asGhost, CancellationToken cancellationToken)
    {
        (string path, var network) = BuildNetwork(nodes);

        Func<string, bool> destination;
        List<string> locations;

        if (asGhost)
        {
            destination = (p) => p.EndsWith('Z');
            locations = network.Edges.Keys.Where((p) => p.EndsWith('A')).ToList();
        }
        else
        {
            destination = (p) => p is "ZZZ";
            locations = ["AAA"];
        }

        var costs = new List<long>(locations.Count);

        foreach (string location in locations)
        {
            long cost = Walk(location, path, network, destination, cancellationToken);
            costs.Add(cost);
        }

        return costs.Aggregate(Maths.LowestCommonMultiple);

        static (string Path, Graph<string> Network) BuildNetwork(IList<string> nodes)
        {
            string path = nodes[0];
            var network = new Graph<string>();

            foreach (string node in nodes.Skip(2))
            {
                string location = node[..3];
                string left = node.Substring(7, 3);
                string right = node.Substring(12, 3);

                var edge = network.Edges.GetOrAdd(location);
                edge.Add(left);
                edge.Add(right);
            }

            return (path, network);
        }

        static long Walk(
            string origin,
            string path,
            Graph<string> network,
            Func<string, bool> destination,
            CancellationToken cancellationToken)
        {
            int steps = 0;

            bool reached = false;

            while (!reached)
            {
                cancellationToken.ThrowIfCancellationRequested();

                foreach (int index in Directions(path))
                {
                    origin = network.Edges[origin][index];

                    steps++;

                    reached = destination(origin);

                    if (reached)
                    {
                        break;
                    }
                }
            }

            return steps;
        }

        static IEnumerable<int> Directions(string path)
        {
            if (path.Length > 0)
            {
                int i = 0;

                while (true)
                {
                    char direction = path[i++];
                    yield return direction is 'L' ? 0 : 1;

                    if (i == path.Length)
                    {
                        i = 0;
                    }
                }
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Steps = WalkNetwork(values, asGhost: false, cancellationToken);
        StepsAsGhost = WalkNetwork(values, asGhost: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("{0} steps are required to reach ZZZ.", Steps);
            Logger.WriteLine("{0} steps are required to simultaneously reach nodes all ending with Z.", StepsAsGhost);
        }

        return PuzzleResult.Create(Steps, StepsAsGhost);
    }
}
