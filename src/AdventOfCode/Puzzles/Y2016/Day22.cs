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
        /// <param name="logger">The optional logger to use.</param>
        /// <returns>
        /// The number of viable nodes found by parsing <paramref name="output"/>.
        /// </returns>
        public static int CountViableNodePairs(IEnumerable<string> output, ILogger? logger = null)
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

            char[,] grid = new char[nodes.Max((p) => p.X) + 1, nodes.Max((p) => p.Y) + 1];

            Node goal = nodes
                .Where((p) => p.Y == 0)
                .OrderByDescending((p) => p.X)
                .First();

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Node node = nodes.First((p) => p.X == x && p.Y == y);

                    if (node == goal)
                    {
                        grid[x, y] = 'G';
                    }
                    else if (node.Size > 200)
                    {
                        grid[x, y] = '#';
                    }
                    else if (node.Used == 0)
                    {
                        grid[x, y] = '_';
                    }
                    else
                    {
                        grid[x, y] = '.';
                    }
                }
            }

            logger?.WriteGrid(grid);

            return count;
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> output = await ReadResourceAsLinesAsync();

            ViableNodePairs = CountViableNodePairs(output, Logger);

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
            /// <param name="x">The X coordinate of the node.</param>
            /// <param name="y">The Y cooridnate of the node.</param>
            private Node(string name, int size, int used, int x, int y)
            {
                Name = name;
                Size = size;
                Used = used;
                X = x;
                Y = y;
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
            /// Gets the X coordinate of the node.
            /// </summary>
            public int X { get; private set; }

            /// <summary>
            /// Gets the Y coordinate of the node.
            /// </summary>
            public int Y { get; private set; }

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

                int indexX = split[0].IndexOf("x", StringComparison.Ordinal);
                int indexY = split[0].IndexOf("y", StringComparison.Ordinal);

                int x = ParseInt32(split[0].Substring(indexX + 1, indexY - indexX - 2));
                int y = ParseInt32(split[0][(indexY + 1) ..]);

                return new Node(
                    split[0],
                    ParseInt32(split[1].TrimEnd('T')),
                    ParseInt32(split[2].TrimEnd('T')),
                    x,
                    y);
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
