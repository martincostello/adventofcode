// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/18</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 18, "Boiling Boulders", RequiresData = true)]
public sealed class Day18 : Puzzle
{
    /// <summary>
    /// Gets the total surface area of the scanned lava droplet.
    /// </summary>
    public int TotalDropletSurfaceArea { get; private set; }

    /// <summary>
    /// Gets the external surface area of the scanned lava droplet.
    /// </summary>
    public int ExternalDropletSurfaceArea { get; private set; }

    /// <summary>
    /// Gets the total surface area of the lava droplet described by the specified cubes.
    /// </summary>
    /// <param name="cubes">The cubes that make up the lava droplet.</param>
    /// <param name="excludeInterior">Whether to exclude the internal surface area.</param>
    /// <returns>
    /// The total surface area of the lava droplet described by <paramref name="cubes"/>.
    /// </returns>
    public static int GetSurfaceArea(IList<string> cubes, bool excludeInterior)
    {
        var droplet = Parse(cubes);

        return GetTotalSurfaceArea(droplet, excludeInterior);

        static Droplet Parse(IList<string> cubes)
        {
            var points = cubes
                .Select((p) => p.AsNumberTriple<int>())
                .Select((p) => new Vector3(p.First, p.Second, p.Third));

            return new Droplet(points);
        }

        static int GetTotalSurfaceArea(Droplet droplet, bool excludeInterior)
        {
            int surfaceArea = droplet.TotalSurfaceArea;

            if (!excludeInterior || droplet.Count < 2)
            {
                return surfaceArea;
            }

            int count = 0;

            droplet.Scan((point, normal, row, length) =>
            {
                if (row.Count > 0)
                {
                    (Vector3 Position, bool IsEmpty, bool IsInternal) previous = (point, true, false);

                    for (int i = 0; i < length; i++)
                    {
                        var location = point + (normal * i);
                        bool found = row.Contains(location);

                        if (previous.IsEmpty && found && (!excludeInterior || !previous.IsInternal))
                        {
                            count++;
                        }

                        previous = (location, !found, !droplet.IsSurface(location, normal));
                    }
                }
            });

            return count;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var values = await ReadResourceAsLinesAsync();

        TotalDropletSurfaceArea = GetSurfaceArea(values, excludeInterior: false);
        ExternalDropletSurfaceArea = GetSurfaceArea(values, excludeInterior: true);

        if (Verbose)
        {
            Logger.WriteLine("The total surface area of the scanned lava droplet is {0}.", TotalDropletSurfaceArea);
            Logger.WriteLine("The external surface area of the scanned lava droplet is {0}.", ExternalDropletSurfaceArea);
        }

        return PuzzleResult.Create(TotalDropletSurfaceArea, ExternalDropletSurfaceArea);
    }

    private sealed class Droplet : HashSet<Vector3>
    {
        public Droplet(IEnumerable<Vector3> points)
            : base(points)
        {
            MinX = this.Min((p) => p.X);
            MaxX = this.Max((p) => p.X);
            MinY = this.Min((p) => p.Y);
            MaxY = this.Max((p) => p.Y);
            MinZ = this.Min((p) => p.Z);
            MaxZ = this.Max((p) => p.Z);

            var corners = new Vector3[]
            {
                new(MinX, MinY, MinZ),
                new(MinX, MinY, MaxZ),
                new(MinX, MaxY, MinZ),
                new(MinX, MaxY, MaxZ),
                new(MaxX, MinY, MinZ),
                new(MaxX, MinY, MaxZ),
                new(MaxX, MaxY, MinZ),
                new(MaxX, MaxY, MaxZ),
            };

            Planes = new (IReadOnlyList<Vector3> Plane, Vector3 Normal)[]
            {
                (new[] { corners[0], corners[1], corners[2], corners[3] }, Vector3.UnitX),
                (new[] { corners[4], corners[5], corners[6], corners[7] }, -Vector3.UnitX),
                (new[] { corners[0], corners[1], corners[4], corners[5] }, Vector3.UnitY),
                (new[] { corners[2], corners[3], corners[6], corners[7] }, -Vector3.UnitY),
                (new[] { corners[0], corners[2], corners[4], corners[6] }, Vector3.UnitZ),
                (new[] { corners[1], corners[3], corners[5], corners[7] }, -Vector3.UnitZ),
            };

            // Create a regular polygon that contains all of the points of the droplet
            Bounds = new((int)(MaxX * MaxY * MaxZ));

            for (float x = MinX; x <= MaxX; x++)
            {
                for (float y = MinY; y <= MaxY; y++)
                {
                    for (float z = MinZ; z <= MaxZ; z++)
                    {
                        Bounds.Add(new(x, y, z));
                    }
                }
            }

            // Create a regular polygon that contains all of the points of the droplet
            // which includes a boundary of at least one unit around its surface.
            var boundsWithFrame = new HashSet<Vector3>();

            for (float x = MinX - 1; x <= MaxX + 1; x++)
            {
                for (float y = MinY - 1; y <= MaxY + 1; y++)
                {
                    for (float z = MinZ - 1; z <= MaxZ + 1; z++)
                    {
                        boundsWithFrame.Add(new(x, y, z));
                    }
                }
            }

            // Find a point that is definitely not already touching the surface
            var origin = boundsWithFrame.Except(Bounds).First();

            // Find all of the points that can be reached from outside the droplet
            var graph = new SurfaceWalker(boundsWithFrame.Except(this), this);
            var reachable = PathFinding.BreadthFirst(graph, origin);

            // The surface of the droplet is all the points that can be reached from outside.
            var surface = new HashSet<Vector3>(this);
            surface.And(reachable);

            // All reachable points that are not already part
            // of the droplet must be outside the droplet.
            var outside = new HashSet<Vector3>(reachable);
            outside.Not(surface);

            Surfaces = new();

            foreach (var item in surface)
            {
                foreach (var neighbor in item.Neighbors())
                {
                    if (outside.Contains(neighbor))
                    {
                        Surfaces.Add((item, neighbor - item));
                    }
                }
            }
        }

        public float MinX { get; }

        public float MaxX { get; }

        public float MinY { get; }

        public float MaxY { get; }

        public float MinZ { get; }

        public float MaxZ { get; }

        public float LengthX => Math.Abs(MaxX - MinX) + 1;

        public float LengthY => Math.Abs(MaxY - MinY) + 1;

        public float LengthZ => Math.Abs(MaxZ - MinZ) + 1;

        public int TotalSurfaceArea => Surfaces.Count;

        private HashSet<Vector3> Bounds { get; }

        private IReadOnlyList<(IReadOnlyList<Vector3> Plane, Vector3 Normal)> Planes { get; }

        private HashSet<(Vector3 Position, Vector3 Normal)> Surfaces { get; }

        public float Length(Vector3 dimension)
        {
            dimension = Vector3.Abs(dimension);

            if (dimension == Vector3.UnitX)
            {
                return LengthX;
            }
            else if (dimension == Vector3.UnitY)
            {
                return LengthY;
            }
            else if (dimension == Vector3.UnitZ)
            {
                return LengthZ;
            }
            else
            {
                throw new ArgumentException($"The specified dimension {dimension} is not valid.", nameof(dimension));
            }
        }

        public HashSet<Vector3> Carve(Vector3 position, Vector3 direction)
        {
            var result = new HashSet<Vector3>((int)Length(direction));

            while (Bounds.Contains(position))
            {
                if (Contains(position))
                {
                    result.Add(position);
                }

                position += direction;
            }

            return result;
        }

        public bool IsSurface(Vector3 position, Vector3 normal)
            => Surfaces.Contains((position, normal));

        public void Scan(Action<Vector3, Vector3, HashSet<Vector3>, float> onRow)
        {
            foreach ((var plane, var normal) in Planes)
            {
                var slice = new HashSet<Vector3>();

                for (float x = plane.Min((p) => p.X); x <= plane.Max((p) => p.X); x++)
                {
                    for (float y = plane.Min((p) => p.Y); y <= plane.Max((p) => p.Y); y++)
                    {
                        for (float z = plane.Min((p) => p.Z); z <= plane.Max((p) => p.Z); z++)
                        {
                            slice.Add(new(x, y, z));
                        }
                    }
                }

                float length = Length(normal);

                foreach (var point in slice)
                {
                    onRow(point, normal, Carve(point, normal), length);
                }
            }
        }
    }

    private sealed class SurfaceWalker : HashSet<Vector3>, IGraph<Vector3>
    {
        public SurfaceWalker(IEnumerable<Vector3> outside, HashSet<Vector3> item)
            : base(outside)
        {
            Item = item;
        }

        public HashSet<Vector3> Item { get; }

        public IEnumerable<Vector3> Neighbors(Vector3 id)
        {
            foreach (var neighbor in id.Neighbors())
            {
                bool isInside = Item.Contains(id);

                if (isInside)
                {
                    if (Contains(neighbor))
                    {
                        yield return neighbor;
                    }
                }
                else if (Contains(neighbor) || Item.Contains(neighbor))
                {
                    yield return neighbor;
                }
            }
        }
    }
}
