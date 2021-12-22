// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Collections;

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/22</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 22, "Reactor Reboot", RequiresData = true)]
public sealed class Day22 : Puzzle
{
    /// <summary>
    /// Gets the number of cubes that are on once the reactor has been initialized.
    /// </summary>
    public long InitializedCubeCount { get; private set; }

    /// <summary>
    /// Gets the number of cubes that are on once the reactor has been rebooted.
    /// </summary>
    public long RebootedCubeCount { get; private set; }

    /// <summary>
    /// Reboots the reactor.
    /// </summary>
    /// <param name="instructions">The instructions to follow to reboot the reactor.</param>
    /// <param name="initialize">Whether to only initialize the reactor, rather than fully reboot it.</param>
    /// <returns>
    /// The number of cubes that are on once the reactor has been rebooted.
    /// </returns>
    public static long Reboot(IList<string> instructions, bool initialize)
    {
        var cuboids = new List<(Cuboid Cuboid, bool TurnOn)>(instructions.Count);

        foreach (string instruction in instructions)
        {
            cuboids.Add(Parse(instruction));
        }

        return initialize ? Initialize(cuboids) : Reboot(cuboids);

        static long Initialize(List<(Cuboid Cuboid, bool TurnOn)> cuboids)
        {
            var bounds = new Cuboid(new(-50, -50, -50), new(100, 100, 100));
            var reactor = new HashSet<Point3D>();

            foreach ((Cuboid cuboid, bool turnOn) in cuboids)
            {
                var points = cuboid.Intersection(bounds);

                if (turnOn)
                {
                    reactor.UnionWith(points);
                }
                else
                {
                    reactor.ExceptWith(points);
                }
            }

            return reactor.Count;
        }

        static long Reboot(List<(Cuboid Cuboid, bool TurnOn)> cuboids)
        {
            return 0;
        }

        static (Cuboid Cuboid, bool TurnOn) Parse(string instruction)
        {
            string[] split = instruction.Split(' ');
            string[] dimensions = split[1].Split(',');

            string rangeX = dimensions[0][2..];
            string rangeY = dimensions[1][2..];
            string rangeZ = dimensions[2][2..];

            string[] valuesX = rangeX.Split("..");
            string[] valuesY = rangeY.Split("..");
            string[] valuesZ = rangeZ.Split("..");

            int minX = Parse<int>(valuesX[0]);
            int maxX = Parse<int>(valuesX[1]);

            int minY = Parse<int>(valuesY[0]);
            int maxY = Parse<int>(valuesY[1]);

            int minZ = Parse<int>(valuesZ[0]);
            int maxZ = Parse<int>(valuesZ[1]);

            int lengthX = maxX - minX + 1;
            int lengthY = maxY - minY + 1;
            int lengthZ = maxZ - minZ + 1;

            var cuboid = new Cuboid(new(minX, minY, minZ), new(lengthX, lengthY, lengthZ));
            bool turnOn = string.Equals(split[0], "on", StringComparison.Ordinal);

            return (cuboid, turnOn);
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> instructions = await ReadResourceAsLinesAsync();

        InitializedCubeCount = Reboot(instructions, initialize: true);
        RebootedCubeCount = Reboot(instructions, initialize: false);

        if (Verbose)
        {
            Logger.WriteLine("{0:N0} cubes in the reactor are on after initialization.", InitializedCubeCount);
            Logger.WriteLine("{0:N0} cubes in the reactor are on after reboot.", RebootedCubeCount);
        }

        return PuzzleResult.Create(InitializedCubeCount, RebootedCubeCount);
    }

    [System.Diagnostics.DebuggerDisplay("({Origin.X}, {Origin.Y}, {Origin.Z}), ({Length.X}, {Length.Y}, {Length.Z})")]
    private readonly struct Cuboid : IEnumerable<Point3D>
    {
        public readonly Point3D Origin;

        public readonly Point3D Length;

        private static readonly Cuboid _unit = new(Point3D.Origin, new(1, 1, 1));

        public Cuboid(Point3D origin, Point3D length)
        {
            Origin = origin;
            Length = length;
        }

        public static ref readonly Cuboid Unit => ref _unit;

        public readonly bool Contains(in Cuboid other)
        {
            foreach (Point3D vertex in other.Verticies())
            {
                if (!Contains(vertex))
                {
                    return false;
                }
            }

            return true;
        }

        public override readonly int GetHashCode()
            => HashCode.Combine(Origin.GetHashCode(), Length.GetHashCode());

        public readonly bool Contains(in Point3D point)
            => Origin.X <= point.X &&
               point.X <= Origin.X + Length.X &&
               Origin.Y <= point.Y &&
               point.Y <= Origin.Y + Length.Y &&
               Origin.Z <= point.Z &&
               point.Z <= Origin.Z + Length.Z;

        public readonly bool IntersectsWith(in Cuboid other)
        {
            int volumeThis = Volume();
            int volumeOther = other.Volume();

            bool otherIsLarger = volumeOther > volumeThis;
            Cuboid largest = otherIsLarger ? other : this;
            Cuboid smallest = !otherIsLarger ? other : this;

            foreach (Point3D vertex in smallest.Verticies())
            {
                if (largest.Contains(vertex))
                {
                    return true;
                }
            }

            return false;
        }

        public readonly HashSet<Point3D> Intersection(in Cuboid other)
        {
            var result = new HashSet<Point3D>();

            if (!IntersectsWith(other))
            {
                return result;
            }

            int volumeThis = Volume();
            int volumeOther = other.Volume();

            bool otherIsLarger = volumeOther > volumeThis;
            Cuboid largest = otherIsLarger ? other : this;
            Cuboid smallest = !otherIsLarger ? other : this;

            int lengthX = smallest.Origin.X + smallest.Length.X;
            int lengthY = smallest.Origin.Y + smallest.Length.Y;
            int lengthZ = smallest.Origin.Z + smallest.Length.Z;

            for (int z = Origin.Z; z < lengthZ; z++)
            {
                for (int y = Origin.Y; y < lengthY; y++)
                {
                    for (int x = Origin.X; x < lengthX; x++)
                    {
                        var point = new Point3D(x, y, z);

                        if (largest.Contains(point))
                        {
                            result.Add(point);
                        }
                    }
                }
            }

            return result;
        }

        public readonly IEnumerator<Point3D> GetEnumerator()
        {
            return Points(this).GetEnumerator();

            static IEnumerable<Point3D> Points(Cuboid value)
            {
                int lengthX = value.Origin.X + value.Length.X;
                int lengthY = value.Origin.Y + value.Length.Y;
                int lengthZ = value.Origin.Z + value.Length.Z;

                for (int z = value.Origin.Z; z <= lengthZ; z++)
                {
                    for (int y = value.Origin.Y; y <= lengthY; y++)
                    {
                        for (int x = value.Origin.X; x <= lengthX; x++)
                        {
                            yield return new(x, y, z);
                        }
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public readonly HashSet<Point3D> ToHashSet() => new(this);

        public readonly int Volume() => Length.X * Length.Y * Length.Z;

        private readonly IEnumerable<Point3D> Verticies()
        {
            yield return Origin;
            yield return Origin + new Point3D(Length.X, 0, 0);
            yield return Origin + new Point3D(0, Length.Y, 0);
            yield return Origin + new Point3D(0, 0, Length.Z);
            yield return Origin + new Point3D(0, Length.Y, Length.Z);
            yield return Origin + new Point3D(Length.X, 0, Length.Z);
            yield return Origin + new Point3D(Length.X, Length.Y, 0);
            yield return Origin + new Point3D(Length.X, Length.Y, Length.Z);
        }
    }
}
