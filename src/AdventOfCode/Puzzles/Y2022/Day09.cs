// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/9</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 09, "Rope Bridge", RequiresData = true)]
public sealed class Day09 : Puzzle
{
    /// <summary>
    /// Gets the number of positions that the tail of the rope with two knots visits at least once.
    /// </summary>
    public int PositionsVisited2 { get; private set; }

    /// <summary>
    /// Gets the number of positions that the tail of the rope with ten knots visits at least once.
    /// </summary>
    public int PositionsVisited10 { get; private set; }

    /// <summary>
    /// Moves a rope using the specified moves and returns the
    /// number of positions that the tail of the rope visits at least once.
    /// </summary>
    /// <param name="moves">The moves to put the rope through.</param>
    /// <param name="knots">The number of knots in the rope.</param>
    /// <returns>
    /// The number of positions that the tail of the rope visits at least once.
    /// </returns>
    public static int Move(IList<string> moves, int knots)
    {
        var directions = Parse(moves);
        var rope = new Rope(Enumerable.Repeat(Point.Empty, knots).ToArray());

        var positions = new HashSet<Point>(moves.Count)
        {
            rope.Tail,
        };

        foreach (var direction in directions)
        {
            rope.Move(direction, (tail) => positions.Add(tail));
        }

        return positions.Count;

        static List<Size> Parse(IList<string> moves)
        {
            var result = new List<Size>(moves.Count);

            foreach (string move in moves)
            {
                char direction = move[0];
                int distance = Parse<int>(move[2..]);

                result.Add(direction switch
                {
                    'U' => new(0, distance),
                    'D' => new(0, -distance),
                    'L' => new(-distance, 0),
                    'R' => new(distance, 0),
                    _ => throw new PuzzleException($"Invalid direction '{direction}'."),
                });
            }

            return result;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var moves = await ReadResourceAsLinesAsync();

        PositionsVisited2 = Move(moves, knots: 2);
        PositionsVisited10 = Move(moves, knots: 10);

        if (Verbose)
        {
            Logger.WriteLine("The tail of the rope with two knots visits {0} positions at least once.", PositionsVisited2);
            Logger.WriteLine("The tail of the rope with ten knots visits {0} positions at least once.", PositionsVisited10);
        }

        return PuzzleResult.Create(PositionsVisited2, PositionsVisited10);
    }

    /// <summary>
    /// A class representing a rope.
    /// </summary>
    public sealed class Rope
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rope"/> class.
        /// </summary>
        /// <param name="knots">The positions of the knots of the rope.</param>
        public Rope(params Point[] knots)
        {
            Knots = knots;
        }

        /// <summary>
        /// Gets the number of knots in the rope.
        /// </summary>
        public int Count => Knots.Count;

        /// <summary>
        /// Gets the position of the rope's head.
        /// </summary>
        public Point Head
        {
            get => Knots[0];
            private set => Knots[0] = value;
        }

        /// <summary>
        /// Gets the position of the rope's tail.
        /// </summary>
        public Point Tail
        {
            get => Knots[^1];
            private set => Knots[^1] = value;
        }

        /// <summary>
        /// Gets the positions of all the knots in the rope.
        /// </summary>
        public IList<Point> Knots { get; }

        /// <summary>
        /// Moves the rope in the specified direction.
        /// </summary>
        /// <param name="direction">The direction to move the rope in.</param>
        /// <param name="onMoveTail">A delegate to invoke when the tail of the rope is moved.</param>
        public void Move(Size direction, Action<Point> onMoveTail)
        {
            int magnitude = Math.Max(Math.Abs(direction.Width), Math.Abs(direction.Height));
            var unit = direction / magnitude;

            for (int i = 0; i < magnitude; i++)
            {
                var previousTail = Tail;
                Head += unit;

                for (int j = 1; j < Count; j++)
                {
                    var leader = Knots[j - 1];
                    var follower = Knots[j];

                    int deltaX = leader.X - follower.X;
                    int deltaY = leader.Y - follower.Y;

                    if (Math.Abs(deltaX) > 1 || Math.Abs(deltaY) > 1)
                    {
                        follower += new Size(Math.Sign(deltaX), Math.Sign(deltaY));
                    }

                    Knots[j] = follower;
                }

                if (previousTail != Tail)
                {
                    onMoveTail(Tail);
                }
            }
        }
    }
}
