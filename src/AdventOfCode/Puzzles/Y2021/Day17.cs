// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/17</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 17, "Trick Shot", RequiresData = true)]
public sealed class Day17 : Puzzle
{
    /// <summary>
    /// Gets the highest apogee reached by a velocity that is within the target area.
    /// </summary>
    public int MaxApogee { get; private set; }

    /// <summary>
    /// Gets the number of initial velocities that will hit the target area.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Calculates a ballistic trajectory that lands the probe within the specified target area.
    /// </summary>
    /// <param name="target">The target area to land the probe within.</param>
    /// <returns>
    /// The highest apogee reached by a velocity that is within the specified target area
    /// and the number of initial velocities that will hit the target area.
    /// </returns>
    public static (int MaxApogee, int Count) Calculate(string target)
    {
        Rectangle targetArea = GetTargetArea(target);
        Point extent = new(targetArea.Right, targetArea.Top);

        var apogees = new List<int>();

        int maxX = targetArea.Right;
        int maxY = Math.Abs(targetArea.Top);
        int minY = -maxY;

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                int? apogee = GetApogee(x, y, targetArea);

                if (apogee is not null)
                {
                    apogees.Add(apogee.Value);
                }
            }
        }

        return (apogees.Max(), apogees.Count);

        static Rectangle GetTargetArea(string target)
        {
            target = target["target area: ".Length..];

            string[] split = target.Split(',', StringSplitOptions.TrimEntries);
            string rangeX = split[0][2..];
            string rangeY = split[1][2..];

            string[] valuesX = rangeX.Split("..");
            string[] valuesY = rangeY.Split("..");

            int minX = Parse<int>(valuesX[0]);
            int maxX = Parse<int>(valuesX[1]);

            int minY = Parse<int>(valuesY[0]);
            int maxY = Parse<int>(valuesY[1]);

            return new Rectangle(
                minX,
                minY,
                maxX - minX + 1,
                maxY - minY + 1);
        }

        static List<Point> GetTrajectory(int x, int y, Point extent)
        {
            var current = new Point(x, y);

            var trajectory = new List<Point>()
            {
                Point.Empty,
                current,
            };

            for (int t = 2; ; t++)
            {
                if (x > 0)
                {
                    x--;
                }

                y--;

                current += new Size(x, y);

                if (current.X > extent.X || current.Y < extent.Y)
                {
                    break;
                }

                trajectory.Add(current);
            }

            return trajectory;
        }

        static int? GetApogee(int vx, int vy, Rectangle target)
        {
            Point extent = new(target.Right, target.Top);

            List<Point> trajectory = GetTrajectory(vx, vy, extent);

            bool hasImpact = trajectory.Any((p) => target.Contains(p));

            if (hasImpact)
            {
                return trajectory.Max((p) => p.Y);
            }

            return null;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string target = (await ReadResourceAsStringAsync()).Trim();

        (MaxApogee, Count) = Calculate(target);

        if (Verbose)
        {
            Logger.WriteLine(
                "The highest y position reached on a trajectory that lands in the target area is {0:N0}.",
                MaxApogee);

            Logger.WriteLine(
                "{0:N0} distinct initial velocity values cause the probe to land within the target area.",
                Count);
        }

        return PuzzleResult.Create(MaxApogee, Count);
    }
}
