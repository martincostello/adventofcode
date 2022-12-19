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
        var droplet = Parse(cubes, excludeInterior);
        return droplet.TotalSurfaceArea;

        static Droplet Parse(IList<string> cubes, bool excludeInterior)
        {
            var points = cubes
                .Select((p) => p.AsNumberTriple<int>())
                .Select((p) => new Vector3(p.First, p.Second, p.Third));

            return new Droplet(points, excludeInterior);
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
        public Droplet(IEnumerable<Vector3> points, bool excludeInterior)
            : base(points)
        {
            MinX = this.Min((p) => p.X);
            MaxX = this.Max((p) => p.X);
            MinY = this.Min((p) => p.Y);
            MaxY = this.Max((p) => p.Y);
            MinZ = this.Min((p) => p.Z);
            MaxZ = this.Max((p) => p.Z);

            // Create a regular polygon that contains all of the points of the droplet
            var bounds = new HashSet<Vector3>((int)(MaxX * MaxY * MaxZ));

            for (float x = MinX; x <= MaxX; x++)
            {
                for (float y = MinY; y <= MaxY; y++)
                {
                    for (float z = MinZ; z <= MaxZ; z++)
                    {
                        bounds.Add(new(x, y, z));
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
            var origin = boundsWithFrame.Except(bounds).First();

            // Find all of the points that can be reached from outside the droplet
            var graph = new SurfaceWalker(boundsWithFrame.Except(this), this, excludeInterior);
            var reachable = PathFinding.BreadthFirst(graph, origin);

            TotalSurfaceArea = graph.Surfaces.Count;
        }

        public float MinX { get; }

        public float MaxX { get; }

        public float MinY { get; }

        public float MaxY { get; }

        public float MinZ { get; }

        public float MaxZ { get; }

        public int TotalSurfaceArea { get; }
    }

    private sealed class SurfaceWalker : IGraph<Vector3>
    {
        public SurfaceWalker(
            IEnumerable<Vector3> outside,
            HashSet<Vector3> item,
            bool excludeInterior)
        {
            Outside = new HashSet<Vector3>(outside);
            Item = item;
            ExcludeInterior = excludeInterior;
        }

        public HashSet<Vector3> Outside { get; }

        public HashSet<Vector3> Item { get; }

        public HashSet<(Vector3 Point, Vector3 Normal)> Surfaces { get; } = new();

        private bool ExcludeInterior { get; }

        public IEnumerable<Vector3> Neighbors(Vector3 id)
        {
            foreach (var neighbor in id.Neighbors())
            {
                if (ExcludeInterior)
                {
                    if (Item.Contains(neighbor))
                    {
                        Surfaces.Add((id, neighbor - id));
                    }

                    if (Outside.Contains(neighbor))
                    {
                        yield return neighbor;
                    }
                }
                else
                {
                    bool isInside = Item.Contains(id);

                    if (isInside)
                    {
                        if (Outside.Contains(neighbor))
                        {
                            yield return neighbor;
                        }
                    }
                    else if (Outside.Contains(neighbor) || Item.Contains(neighbor))
                    {
                        yield return neighbor;
                    }

                    if (!isInside && Item.Contains(neighbor))
                    {
                        Surfaces.Add((id, neighbor - id));
                    }
                }
            }
        }
    }
}
