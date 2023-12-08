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
    public int Steps { get; private set; }

    /// <summary>
    /// Gets the number of steps required to simultaneously
    /// reach nodes all ending with Z.
    /// </summary>
    public int StepsAsGhost { get; private set; }

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
    public static int WalkNetwork(IList<string> nodes, bool asGhost, CancellationToken cancellationToken)
    {
        (string path, var graph) = BuildGraph(nodes);

        int steps = 0;

        if (asGhost)
        {
            var locations = graph.Edges.Keys.Where((p) => p.EndsWith('A')).ToList();

            while (!locations.All(Destination))
            {
                cancellationToken.ThrowIfCancellationRequested();

                foreach (char direction in path)
                {
                    int index = direction == 'L' ? 0 : 1;

                    for (int i = 0; i < locations.Count; i++)
                    {
                        locations[i] = graph.Edges[locations[i]][index];
                    }

                    steps++;

                    if (locations.All(Destination))
                    {
                        break;
                    }
                }
            }

            static bool Destination(string location) => location.EndsWith('Z');
        }
        else
        {
            string location = "AAA";
            string destination = "ZZZ";

            while (location != destination)
            {
                cancellationToken.ThrowIfCancellationRequested();

                foreach (char direction in path)
                {
                    int index = direction == 'L' ? 0 : 1;
                    location = graph.Edges[location][index];

                    steps++;

                    if (location == destination)
                    {
                        break;
                    }
                }
            }
        }

        return steps;

        static (string Path, Graph<string> Graph) BuildGraph(IList<string> nodes)
        {
            string path = nodes[0];
            var graph = new Graph<string>();

            foreach (string node in nodes.Skip(2))
            {
                string id = node[..3];
                string left = node.Substring(7, 3);
                string right = node.Substring(12, 3);

                var edge = graph.Edges.GetOrAdd(id);
                edge.Add(left);
                edge.Add(right);
            }

            return (path, graph);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Steps = WalkNetwork(values, asGhost: false, cancellationToken);
        Steps = WalkNetwork(values, asGhost: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("{0} steps are required to reach ZZZ.", Steps);
            Logger.WriteLine("{0} steps are required to simultaneously reach nodes all ending with Z.", Steps);
        }

        return PuzzleResult.Create(Steps);
    }
}
