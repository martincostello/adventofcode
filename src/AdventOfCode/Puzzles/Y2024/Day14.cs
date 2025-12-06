// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2024;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2024/day/14</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2024, 14, "Restroom Redoubt", RequiresData = true)]
public sealed class Day14 : Puzzle<int, int>
{
    /// <summary>
    /// Gets the safety factor after 100 seconds.
    /// </summary>
    public int SafetyFactor { get; private set; }

    /// <summary>
    /// Gets the number of seconds after which the robots display the Easter egg, if any.
    /// </summary>
    public int EasterEggSeconds { get; private set; }

    /// <summary>
    /// Simulates the specified robots' positions after the specified number of seconds.
    /// </summary>
    /// <param name="robots">A description of the robots placed in the room.</param>
    /// <param name="bounds">The bounds of the space the robots move in.</param>
    /// <param name="seconds">The period of time to simulate the positions for.</param>
    /// <param name="findEasterEgg">Whether to attempt to find the Easter egg.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The safety factor after the specified number of seconds when <paramref name="findEasterEgg"/>
    /// is <see langword="false"/>, otherwise the number of seconds after which the Easter egg is displayed, if any.
    /// </returns>
    public static int Simulate(
        IList<string> robots,
        Rectangle bounds,
        int seconds,
        bool findEasterEgg,
        CancellationToken cancellationToken)
    {
        var patrol = new List<Robot>(robots.Count);

        foreach (ReadOnlySpan<char> specification in robots)
        {
            int delimiter = specification.IndexOf(' ');
            (int px, int py) = specification[2..delimiter].AsNumberPair<int>();
            (int vx, int vy) = specification[(delimiter + 3)..].AsNumberPair<int>();

            patrol.Add(new(new(px, py), new(vx, vy), bounds));
        }

        int limitX = (bounds.Width / 3) - 1;
        int limitY = (bounds.Height / 3) - 1;

        for (int t = 0; (t < seconds || findEasterEgg) && !cancellationToken.IsCancellationRequested; t++)
        {
            for (int i = 0; i < patrol.Count; i++)
            {
                var robot = patrol[i];
                robot.Move();
            }

            if (!findEasterEgg)
            {
                continue;
            }

            int maximum = 0;

            for (int y = 0; y < bounds.Height; y++)
            {
                maximum = Math.Max(maximum, patrol.Count((p) => p.Position.Y == y));
            }

            if (maximum < limitX)
            {
                continue;
            }

            maximum = 0;

            for (int x = 0; x < bounds.Width; x++)
            {
                maximum = Math.Max(maximum, patrol.Count((p) => p.Position.X == x));
            }

            if (maximum > limitY)
            {
                return t + 1;
            }
        }

        if (findEasterEgg)
        {
            return Unsolved;
        }

        var quadrant = new Size(bounds.Width / 2, bounds.Height / 2);
        var topLeft = new Rectangle(new(bounds.Left, bounds.Top), quadrant);
        var topRight = new Rectangle(new(quadrant.Width + 1, bounds.Top), quadrant);
        var bottomLeft = new Rectangle(new(bounds.Left, quadrant.Height + 1), quadrant);
        var bottomRight = new Rectangle(new(quadrant.Width + 1, quadrant.Height + 1), quadrant);

        int safetyFactor = 1;

        foreach (var quad in new[] { topLeft, topRight, bottomLeft, bottomRight })
        {
            safetyFactor *= patrol.Count((p) => quad.Contains(p.Position));
        }

        return safetyFactor;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync(cancellationToken);

        var bounds = new Rectangle(0, 0, 101, 103);

        SafetyFactor = Simulate(values, bounds, seconds: 100, findEasterEgg: false, cancellationToken);
        EasterEggSeconds = Simulate(values, bounds, seconds: int.MaxValue, findEasterEgg: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The safety factor after 100 seconds is {0}.", SafetyFactor);
            Logger.WriteLine("The robots first display the Easter egg after {0} seconds.", EasterEggSeconds);
        }

        Solution1 = SafetyFactor;
        Solution2 = EasterEggSeconds;

        return Result();
    }

    private record Robot(Point Origin, Size Velocity, Rectangle Bounds)
    {
        public Point Position { get; private set; } = Origin;

        public void Move()
        {
            Position += Velocity;

            if (!Bounds.Contains(Position))
            {
                Position = new(Move(Position.X, Bounds.Width), Move(Position.Y, Bounds.Height));
            }

            static int Move(int position, int limit)
            {
                position %= limit;

                if (Math.Sign(position) is -1)
                {
                    position += limit;
                }

                return position;
            }
        }
    }
}
