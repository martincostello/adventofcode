// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2019/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2019, 03, "Crossed Wires", RequiresData = true)]
public sealed class Day03 : Puzzle
{
    /// <summary>
    /// Represents the wires passing through a grid square.
    /// </summary>
    [Flags]
    private enum Wires
    {
        /// <summary>
        /// None.
        /// </summary>
        None,

        /// <summary>
        /// The first wire.
        /// </summary>
        First = 1,

        /// <summary>
        /// The second wire.
        /// </summary>
        Second = 2,
    }

    /// <summary>
    /// Gets the Manhattan distance of the central port to the closest intersection.
    /// </summary>
    public int ManhattanDistance { get; private set; }

    /// <summary>
    /// Gets the intersection that is reached in the minimum number of steps.
    /// </summary>
    public int MinimumSteps { get; private set; }

    /// <summary>
    /// Gets the Manhattan distance of the central port to the closest intersection for the specified wires.
    /// </summary>
    /// <param name="wires">The wires to compute the intersection distance for.</param>
    /// <returns>
    /// The Manhattan distance from the central port to the closest intersection and the
    /// minimum number of combined steps to reach an intersection.
    /// </returns>
    public static (int ManhattanDistance, int MinimumSteps) GetManhattanDistanceOfClosesIntersection(IList<string> wires)
    {
        var grid = new Dictionary<Point, Wires>();
        var stepsToIntersection = new Dictionary<Point, int[]>();
        var intersections = new List<Point>();

        void MarkWire(Point point, Wires wire, int steps)
        {
            Wires wiresSet = grid[point] = grid.GetValueOrDefault(point) | wire;

            if (wiresSet == (Wires.First | Wires.Second))
            {
                intersections.Add(point);
            }

            if (!stepsToIntersection.TryGetValue(point, out int[]? stepsForWires))
            {
                stepsForWires = stepsToIntersection[point] = new[] { int.MaxValue, int.MaxValue };
            }

            int index = (int)wire - 1;
            stepsForWires[index] = Math.Min(steps, stepsForWires[index]);
        }

        for (int i = 0; i < wires.Count; i++)
        {
            int x = 0;
            int y = 0;
            int steps = 0;

            var wireToMark = (Wires)(i + 1);

            foreach (ReadOnlySpan<char> instruction in wires[i].Split(','))
            {
                (int deltaX, int deltaY) = GetDelta(instruction);

                int sign = Math.Sign(deltaX);
                int magnitude = Math.Abs(deltaX);

                for (int j = 0; j < magnitude; j++, steps++)
                {
                    MarkWire(new(x + (j * sign), y), wireToMark, steps);
                }

                sign = Math.Sign(deltaY);
                magnitude = Math.Abs(deltaY);

                for (int j = 0; j < magnitude; j++, steps++)
                {
                    MarkWire(new(x, y + (j * sign)), wireToMark, steps);
                }

                x += deltaX;
                y += deltaY;
            }
        }

        int distance = intersections
            .Skip(1)
            .Min((p) => p.ManhattanDistance());

        int minimumSteps = stepsToIntersection
            .Where((p) => p.Key != default && intersections.Contains(p.Key))
            .Min((p) => p.Value.Sum());

        return (distance, minimumSteps);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var wires = await ReadResourceAsLinesAsync(cancellationToken);

        (ManhattanDistance, MinimumSteps) = GetManhattanDistanceOfClosesIntersection(wires);

        if (Verbose)
        {
            Logger.WriteLine("The Manhattan distance from the central port to the closest intersection is {0}.", ManhattanDistance);
            Logger.WriteLine("The minimum number of combined steps to get to an intersection is {0}.", MinimumSteps);
        }

        return PuzzleResult.Create(ManhattanDistance, MinimumSteps);
    }

    /// <summary>
    /// Gets the x and y coordinates to change by for the specified instruction.
    /// </summary>
    /// <param name="instruction">The instruction to get the coordinate delta for.</param>
    /// <returns>
    /// The x and y coordinates to move the grid by.
    /// </returns>
    private static (int X, int Y) GetDelta(ReadOnlySpan<char> instruction)
    {
        char direction = instruction[0];
        int distance = Parse<int>(instruction[1..]);

        return direction switch
        {
            'D' => (0, -distance),
            'L' => (-distance, 0),
            'R' => (distance, 0),
            'U' => (0, distance),
            _ => throw new PuzzleException($"The direction '{direction}' is invalid."),
        };
    }
}
