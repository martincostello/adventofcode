// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/17</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 17, "Trick Shot", RequiresData = true)]
public sealed class Day17 : Puzzle<int, int>
{
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

        var apogees = new List<int>();

        int maxX = targetArea.Right;
        int maxY = Math.Abs(targetArea.Top);
        int minY = -maxY;

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                if (GetApogee(x, y, targetArea) is { } value)
                {
                    apogees.Add(value);
                }
            }
        }

        return (apogees.Max(), apogees.Count);

        static Rectangle GetTargetArea(ReadOnlySpan<char> target)
        {
            target = target["target area: ".Length..];

            target.Bifurcate(',', out var rangeX, out var rangeY);

            (int minX, int maxX) = rangeX.Trim()[2..].AsNumberPair<int>("..");
            (int minY, int maxY) = rangeY.Trim()[2..].AsNumberPair<int>("..");

            return new(
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

            bool hasImpact = trajectory.Any(target.Contains);

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
        return await SolveWithStringAsync(
            static (target, logger) =>
            {
                target = target.Trim();

                (int maxApogee, int count) = Calculate(target);

                if (logger is { })
                {
                    logger.WriteLine("The highest y position reached on a trajectory that lands in the target area is {0:N0}.", maxApogee);
                    logger.WriteLine("{0:N0} distinct initial velocity values cause the probe to land within the target area.", count);
                }

                return (maxApogee, count);
            },
            cancellationToken);
    }
}
