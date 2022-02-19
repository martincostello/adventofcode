// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Numerics;

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
            var reactor = new HashSet<Vector3>();

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
            var reactor = new Dictionary<Cuboid, long>(cuboids.Count * 2);

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

                if (newCuboids.Count > 0)
                {
                    reactor.EnsureCapacity(reactor.Count + (newCuboids.Count * 2));

                    foreach ((Cuboid newCuboid, long count) in newCuboids)
                    {
                        reactor.AddOrIncrement(newCuboid, count, count);
                    }
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
        public readonly Vector3 Origin;

        public readonly Vector3 Length;

        public Cuboid(Vector3 origin, Vector3 length)
        {
            Origin = origin;
            Length = length;
        }

        public override readonly int GetHashCode()
            => HashCode.Combine(Origin.GetHashCode(), Length.GetHashCode());

        public readonly bool Contains(in Vector3 point)
        {
            var limit = Origin + Length;
            return Origin.X <= point.X &&
               point.X <= limit.X &&
               Origin.Y <= point.Y &&
               point.Y <= limit.Y &&
               Origin.Z <= point.Z &&
               point.Z <= limit.Z;
        }

        public readonly Cuboid? Abjunction(in Cuboid other)
        {
            var min = Vector3.Max(Origin, other.Origin);
            var max = Vector3.Min(Origin + Length, other.Origin + other.Length);

            if (min.X <= max.X && min.Y <= max.Y && min.Z <= max.Z)
            {
                var origin = min;
                var length = max - min;

                return new(origin, length);
            }

            return null;
        }

        public readonly bool IntersectsWith(in Cuboid other)
        {
            float volumeThis = Volume();
            float volumeOther = other.Volume();

            bool otherIsLarger = volumeOther > volumeThis;
            Cuboid largest = otherIsLarger ? other : this;
            Cuboid smallest = !otherIsLarger ? other : this;

            foreach (Vector3 vertex in smallest.Verticies())
            {
                if (largest.Contains(vertex))
                {
                    return true;
                }
            }

            return false;
        }

        public readonly HashSet<Vector3> IntersectWith(in Cuboid other)
        {
            var result = new HashSet<Vector3>();

            if (!IntersectsWith(other))
            {
                return result;
            }

            float volumeThis = Volume();
            float volumeOther = other.Volume();

            bool otherIsLarger = volumeOther > volumeThis;
            Cuboid largest = otherIsLarger ? other : this;
            Cuboid smallest = !otherIsLarger ? other : this;

            var length = smallest.Origin + smallest.Length;

            for (float z = Origin.Z; z < length.Z; z++)
            {
                for (float y = Origin.Y; y < length.Y; y++)
                {
                    for (float x = Origin.X; x < length.X; x++)
                    {
                        var point = new Vector3(x, y, z);

                        if (largest.Contains(point))
                        {
                            result.Add(point);
                        }
                    }
                }
            }

            return result;
        }

        public readonly long Volume() => (long)Length.X * (long)Length.Y * (long)Length.Z;

        private readonly Vector3[] Verticies()
        {
            var limit = Origin + Length;
            return new[]
            {
                Origin,
                new Vector3(limit.X,  0,       0),
                new Vector3(0,        limit.Y, 0),
                new Vector3(0,        0,       limit.Z),
                new Vector3(0,        limit.Y, limit.Z),
                new Vector3(Length.X, 0,       limit.Z),
                new Vector3(limit.X,  limit.Y, 0),
                limit,
            };
        }
    }
}
