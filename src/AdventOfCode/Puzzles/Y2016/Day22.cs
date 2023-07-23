// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/22</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 22, "Grid Computing", RequiresData = true)]
public sealed class Day22 : Puzzle
{
    /// <summary>
    /// Gets the number of viable nodes found from processing the input.
    /// </summary>
    public int ViableNodePairs { get; private set; }

    /// <summary>
    /// Gets the minimum number of steps required to extract the data.
    /// </summary>
    public int MinimumStepsToExtract { get; private set; }

    /// <summary>
    /// Counts the number of viable node pairs.
    /// </summary>
    /// <param name="output">The output from a <c>df</c> command.</param>
    /// <param name="logger">The optional logger to use.</param>
    /// <returns>
    /// The number of viable nodes found by parsing <paramref name="output"/> and
    /// the minimum number of moves required to extract the data on the target node.
    /// </returns>
    public static (int ViableNodes, int StepsToExtract) CountViableNodePairs(IEnumerable<string> output, ILogger? logger = null)
    {
        var nodes = output
            .Skip(2)
            .Select(Node.Parse)
            .ToList();

        int viableCount = 0;
        int nodeCount = nodes.Count;

        for (int i = 0; i < nodeCount; i++)
        {
            Node node = nodes[i];

            for (int j = 0; j < nodeCount; j++)
            {
                if (i == j)
                {
                    continue;
                }

                Node other = nodes[j];

                if (node.Used > 0 && node.Used <= other.Available)
                {
                    viableCount++;
                }
            }
        }

        int width = nodes.Max((p) => p.Location.X) + 1;
        int height = nodes.Max((p) => p.Location.Y) + 1;

        char[,] grid = new char[width, height];

        Node goalNode = nodes
            .Where((p) => p.Location.Y == 0)
            .OrderByDescending((p) => p.Location.X)
            .First();

        Node emptyNode = nodes.First((p) => p.Used == 0);

        int minimumSize = nodes.Select((p) => p.Size).Min();

        const char Available = '.';
        const char Empty = '_';
        const char Full = '#';
        const char Goal = 'G';

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = nodes.First((p) => p.Location.X == x && p.Location.Y == y);

                if (node == goalNode)
                {
                    grid[x, y] = Goal;
                }
                else if (node == emptyNode)
                {
                    grid[x, y] = Empty;
                }
                else if (node.Used > minimumSize)
                {
                    grid[x, y] = Full;
                }
                else
                {
                    grid[x, y] = Available;
                }
            }
        }

        logger?.WriteGrid(grid);

        var up = new Size(0, -1);
        var down = new Size(0, 1);
        var left = new Size(-1, 0);
        var right = new Size(1, 0);

        Point empty = emptyNode.Location;

        int steps = 0;

        // Move up until the "wall" is reached
        while (grid[empty.X, empty.Y - 1] == Available)
        {
            empty += up;
            steps++;
        }

        // Move left until there is no more blocking "wall"
        while (grid[empty.X, empty.Y - 1] == Full)
        {
            empty += left;
            steps++;
        }

        // Move to the top of the grid
        while (empty.Y > 0)
        {
            empty += up;
            steps++;
        }

        // Move to the square adjacent to the goal
        while (grid[empty.X, empty.Y] != Goal)
        {
            empty += right;
            steps++;
        }

        // Loop around to move the goal left
        while (empty.X != 1)
        {
            empty += right;
            empty += down;
            empty += left;
            empty += left;
            empty += up;
            steps += 5;
        }

        return (viableCount, steps);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var output = await ReadResourceAsLinesAsync(cancellationToken);

        (ViableNodePairs, MinimumStepsToExtract) = CountViableNodePairs(output, Logger);

        if (Verbose)
        {
            Logger.WriteLine("The number of viable pairs of nodes is {0:N0}.", ViableNodePairs);
            Logger.WriteLine("The fewest number of steps required to extract the goal data is {0:N0}.", MinimumStepsToExtract);
        }

        return PuzzleResult.Create(ViableNodePairs, MinimumStepsToExtract);
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
        /// <param name="location">The coordinates of the node's location.</param>
        private Node(string name, int size, int used, Point location)
        {
            Name = name;
            Size = size;
            Used = used;
            Location = location;
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
        /// Gets the available capacity of the node in terabytes.
        /// </summary>
        public int Available => Size - Used;

        /// <summary>
        /// Gets the coordinates of the node.
        /// </summary>
        public Point Location { get; private set; }

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

            var first = split[0].AsSpan();
            int indexX = first.IndexOf('x');
            int indexY = first.IndexOf('y');

            int x = Parse<int>(first.Slice(indexX + 1, indexY - indexX - 2));
            int y = Parse<int>(first[(indexY + 1)..]);

            return new Node(
                split[0],
                Parse<int>(split[1].TrimEnd('T')),
                Parse<int>(split[2].TrimEnd('T')),
                new(x, y));
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
