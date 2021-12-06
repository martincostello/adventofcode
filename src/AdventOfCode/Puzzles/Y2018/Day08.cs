// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2018/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2018, 08, RequiresData = true)]
public sealed class Day08 : Puzzle
{
    /// <summary>
    /// Gets the sum of the tree's metadata entries.
    /// </summary>
    public long SumOfMetadata { get; private set; }

    /// <summary>
    /// Parses the specified tree of nodes.
    /// </summary>
    /// <param name="data">The raw node data for the tree.</param>
    /// <returns>
    /// The sum of the tree's metadata entries.
    /// </returns>
    public static long ParseTree(IEnumerable<int> data)
    {
        int[] span = data.ToArray();

        Parse(span, out _, out long metadataSum);

        return metadataSum;

        static Node Parse(ReadOnlySpan<int> data, out int valuesRead, out long metadataSum)
        {
            int index = 0;
            long sum = 0;

            var node = new Node()
            {
                ChildCount = data[index++],
                MetadataCount = data[index++],
            };

            if (node.ChildCount > 0)
            {
                for (int i = 0; i < node.ChildCount; i++)
                {
                    var child = Parse(data[index..], out int childValuesRead, out long childMetadataSum);

                    index += childValuesRead;
                    sum += childMetadataSum;

                    node.Children.Add(child);
                }
            }

            if (node.MetadataCount > 0)
            {
                for (int i = 0; i < node.MetadataCount; i++)
                {
                    node.Metadata.Add(data[index++]);
                }
            }

            sum += node.Metadata.Sum();

            valuesRead = index;
            metadataSum = sum;

            return node;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var data = (await ReadResourceAsStringAsync()).AsNumbers<int>(' ');

        SumOfMetadata = ParseTree(data);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the tree's metadata entries is {0}.", SumOfMetadata);
        }

        return PuzzleResult.Create(SumOfMetadata);
    }

    private sealed class Node
    {
        public int ChildCount { get; init; }

        public int MetadataCount { get; init; }

        public IList<Node> Children { get; } = new List<Node>();

        public IList<int> Metadata { get; } = new List<int>();
    }
}
