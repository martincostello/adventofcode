// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/22</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 22, RequiresData = true)]
    public sealed class Day22 : Puzzle
    {
        /// <summary>
        /// Gets the number of viable nodes found from processing the input.
        /// </summary>
        public int ViableNodePairs { get; private set; }

        /// <summary>
        /// Counts the number of viable node pairs.
        /// </summary>
        /// <param name="output">The output from a <c>df</c> command.</param>
        /// <returns>
        /// The number of viable nodes found by parsing <paramref name="output"/>.
        /// </returns>
        public static int CountViableNodePairs(IEnumerable<string> output)
        {
            IList<Node> nodes = output
                .Skip(2)
                .Select(Node.Parse)
                .ToList();

            int count = 0;
            int nodeCount = nodes.Count;

            for (int i = 0; i < nodeCount; i++)
            {
                Node a = nodes[i];

                for (int j = 0; j < nodeCount; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    Node b = nodes[j];

                    if (a.Used > 0 && a.Used <= b.Available)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> output = await ReadResourceAsLinesAsync();

            ViableNodePairs = CountViableNodePairs(output);

            if (Verbose)
            {
                Logger.WriteLine("The number of viable pairs of nodes is {0:N0}.", ViableNodePairs);
            }

            return PuzzleResult.Create(ViableNodePairs);
        }

        /// <summary>
        /// A class representing a storage node. This class cannot be inherited.
        /// </summary>
        private sealed class Node
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class.
            /// </summary>
            /// <param name="name">The name of the storage node.</param>
            /// <param name="size">The size of the storage node in terabytes.</param>
            /// <param name="used">The amount of used space in the storage node, in terabytes.</param>
            private Node(string name, int size, int used)
            {
                Name = name;
                Size = size;
                Used = used;
            }

            /// <summary>
            /// Gets the name of the storage node.
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Gets the total size of the node in terabytes.
            /// </summary>
            public int Size { get; }

            /// <summary>
            /// Gets the used capacity of the node in terabytes.
            /// </summary>
            public int Used { get; private set; }

            /// <summary>
            /// Gets the availabe capacity of the node in terabytes.
            /// </summary>
            public int Available => Size - Used;

            /// <summary>
            /// Parses an instance of <see cref="Node"/> from the specified line
            /// of output from the <c>df</c> command.
            /// </summary>
            /// <param name="value">The line from the <c>df</c> command to parse.</param>
            /// <returns>
            /// The instance of <see cref="Node"/> parsed from <paramref name="value"/>.
            /// </returns>
            public static Node Parse(string value)
            {
                string[] split = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                return new Node(
                    split[0],
                    ParseInt32(split[1].TrimEnd('T')),
                    ParseInt32(split[2].TrimEnd('T')));
            }

            /// <summary>
            /// Moves the contents of this storage node to the specified node
            /// if it has enough available unused storage capacity.
            /// </summary>
            /// <param name="other">The node to move this node's data to.</param>
            /// <returns>
            /// <see langword="true"/> if the contents of this node were successfully
            /// moved to the node specified by <paramref name="other"/>; otherwise <see langword="false"/>.
            /// </returns>
            public bool Move(Node other)
            {
                bool moved = false;

                if (other.Available >= Used)
                {
                    other.Used += Used;
                    Used = 0;
                    moved = true;
                }

                return moved;
            }
        }
    }
}
