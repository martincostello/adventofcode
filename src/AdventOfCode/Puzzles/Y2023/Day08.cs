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
    /// Walks the network of nodes and returns the number of steps required to reach ZZZ.
    /// </summary>
    /// <param name="nodes">The description of the network of nodes.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The number of steps required to reach ZZZ.
    /// </returns>
    public static int WalkNetwork(IList<string> nodes, CancellationToken cancellationToken)
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

        int steps = 0;

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

        return steps;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        Steps = WalkNetwork(values, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("{0} steps are required to reach ZZZ.", Steps);
        }

        return PuzzleResult.Create(Steps);
    }
}
