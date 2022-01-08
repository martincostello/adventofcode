// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

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
            var parsed = Parse(instruction);

            if (parsed.TurnOn || cuboids.Count > 0)
            {
                cuboids.Add(Parse(instruction));
            }
        }

        return initialize ? Initialize(cuboids) : Reboot(cuboids);

        static long Initialize(List<(Cuboid Cuboid, bool TurnOn)> cuboids)
        {
            var bounds = new Cuboid(new(-50, -50, -50), new(100, 100, 100));
            var reactor = new HashSet<Point3D>();

            foreach ((Cuboid cuboid, bool turnOn) in cuboids)
            {
                var points = cuboid.IntersectWith(bounds);

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
            var reactor = new Dictionary<Cuboid, long>();

            foreach ((Cuboid cuboid, bool turnOn) in cuboids)
            {
                var newCuboids = new Dictionary<Cuboid, long>();

                // Remove the new cuboid from each existing cuboid.
                // This will effectively perform the operation to
                // turn it off if that is the required action.
                foreach ((Cuboid existing, long count) in reactor)
                {
                    Cuboid? abjunction = cuboid.Abjunction(existing);

                    if (abjunction is { } value)
                    {
                        newCuboids.AddOrDecrement(value, -count, count);
                    }
                }

                if (turnOn)
                {
                    newCuboids.AddOrIncrement(cuboid, 1);
                }

                foreach ((Cuboid newCuboid, long count) in newCuboids)
                {
                    reactor.AddOrIncrement(newCuboid, count, count);
                }
            }

            return reactor.Sum((p) => p.Key.Volume() * p.Value);
        }

        static (Cuboid Cuboid, bool TurnOn) Parse(string instruction)
        {
            (string state, string coordinates) = instruction.Bifurcate(' ');
            (string rangeX, string rangeY, string rangeZ) = coordinates.Trifurcate(',');

            const string Range = "..";
            string[] valuesX = rangeX[2..].Split(Range);
            string[] valuesY = rangeY[2..].Split(Range);
            string[] valuesZ = rangeZ[2..].Split(Range);

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
            bool turnOn = string.Equals(state, "on", StringComparison.Ordinal);

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
    private readonly struct Cuboid
    {
        public readonly Point3D Origin;

        public readonly Point3D Length;

        public Cuboid(Point3D origin, Point3D length)
        {
            Origin = origin;
            Length = length;
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

        public readonly Cuboid? Abjunction(in Cuboid other)
        {
            int minX = Math.Max(Origin.X, other.Origin.X);
            int maxX = Math.Min(Origin.X + Length.X, other.Origin.X + other.Length.X);
            int minY = Math.Max(Origin.Y, other.Origin.Y);
            int maxY = Math.Min(Origin.Y + Length.Y, other.Origin.Y + other.Length.Y);
            int minZ = Math.Max(Origin.Z, other.Origin.Z);
            int maxZ = Math.Min(Origin.Z + Length.Z, other.Origin.Z + other.Length.Z);

            if (minX <= maxX && minY <= maxY && minZ <= maxZ)
            {
                var origin = new Point3D(minX, minY, minZ);
                var length = new Point3D(maxX - minX, maxY - minY, maxZ - minZ);

                return new(origin, length);
            }

            return null;
        }

        public readonly bool IntersectsWith(in Cuboid other)
        {
            long volumeThis = Volume();
            long volumeOther = other.Volume();

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

        public readonly HashSet<Point3D> IntersectWith(in Cuboid other)
        {
            var result = new HashSet<Point3D>();

            if (!IntersectsWith(other))
            {
                return result;
            }

            long volumeThis = Volume();
            long volumeOther = other.Volume();

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

        public readonly long Volume() => (long)Length.X * Length.Y * Length.Z;

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
