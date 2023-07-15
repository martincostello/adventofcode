// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2018;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2018/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2018, 08, "Memory Maneuver", RequiresData = true)]
public sealed class Day08 : Puzzle
{
    /// <summary>
    /// Gets the sum of the tree's metadata entries.
    /// </summary>
    public long SumOfMetadata { get; private set; }

    /// <summary>
    /// Gets the value of the tree's root node.
    /// </summary>
    public long RootNodeValue { get; private set; }

    /// <summary>
    /// Parses the specified tree of nodes.
    /// </summary>
    /// <param name="data">The raw node data for the tree.</param>
    /// <returns>
    /// The sum of the tree's metadata entries and the value of the root node.
    /// </returns>
    public static (long SumOfMetadata, long RootNodeValue) ParseTree(IEnumerable<int> data)
    {
        int[] span = data.ToArray();

        var root = Parse(span, out _);

        return (root.MetadataSum, root.Value);

        static Node Parse(ReadOnlySpan<int> data, out int valuesRead)
        {
            int index = 0;

            var node = new Node(data[index++], data[index++]);

            if (node.ChildCount > 0)
            {
                for (int i = 0; i < node.ChildCount; i++)
                {
                    var child = Parse(data[index..], out int childValuesRead);

                    index += childValuesRead;
                    node.MetadataSum += child.MetadataSum;

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

            valuesRead = index;

            long nodeMetadataSum = node.Metadata.Sum();
            node.MetadataSum += nodeMetadataSum;

            if (node.ChildCount < 1)
            {
                node.Value = nodeMetadataSum;
            }
            else
            {
                foreach (int metadata in node.Metadata)
                {
                    if (metadata < 1 || metadata > node.ChildCount)
                    {
                        continue;
                    }

                    node.Value += node.Children[metadata - 1].Value;
                }
            }

            return node;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var data = (await ReadResourceAsStringAsync(cancellationToken)).AsNumbers<int>(' ');

        (SumOfMetadata, RootNodeValue) = ParseTree(data);

        if (Verbose)
        {
            Logger.WriteLine("The sum of the tree's metadata entries is {0}.", SumOfMetadata);
            Logger.WriteLine("The value the tree's root node is {0}.", RootNodeValue);
        }

        return PuzzleResult.Create(SumOfMetadata, RootNodeValue);
    }

    private sealed class Node(int childCount, int metadataCount)
    {
        public int ChildCount { get; init; } = childCount;

        public int MetadataCount { get; init; } = metadataCount;

        public List<Node> Children { get; } = new(childCount);

        public List<int> Metadata { get; } = new(metadataCount);

        public long MetadataSum { get; set; }

        public long Value { get; set; }
    }
}
