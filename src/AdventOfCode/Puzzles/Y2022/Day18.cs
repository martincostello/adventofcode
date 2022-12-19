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
        var points = cubes
            .Select((p) => p.AsNumberTriple<int>())
            .Select((p) => new Vector3(p.First, p.Second, p.Third))
            .ToHashSet();

        float minX = points.Min((p) => p.X);
        float maxX = points.Max((p) => p.X);
        float minY = points.Min((p) => p.Y);
        float maxY = points.Max((p) => p.Y);
        float minZ = points.Min((p) => p.Z);
        float maxZ = points.Max((p) => p.Z);

        // Create a regular polygon that contains all of the points of the droplet
        // which includes a boundary of at least one unit around its surface.
        var outside = new HashSet<Vector3>((int)(maxX * maxY * maxZ));

        for (float x = minX - 1; x <= maxX + 1; x++)
        {
            for (float y = minY - 1; y <= maxY + 1; y++)
            {
                for (float z = minZ - 1; z <= maxZ + 1; z++)
                {
                    outside.Add(new(x, y, z));
                }
            }
        }

        outside.Not(points);

        // Find a point that is not already touching the surface
        var origin = new Vector3(minX - 1, minY - 1, minZ - 1);

        // Find all of the surfaces that can be reached from outside the droplet
        var graph = new SurfaceWalker(outside, points, excludeInterior);
        _ = PathFinding.BreadthFirst(graph, origin);

        return graph.Surfaces.Count;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var cubes = await ReadResourceAsLinesAsync();

        TotalDropletSurfaceArea = GetSurfaceArea(cubes, excludeInterior: false);
        ExternalDropletSurfaceArea = GetSurfaceArea(cubes, excludeInterior: true);

        if (Verbose)
        {
            Logger.WriteLine("The total surface area of the scanned lava droplet is {0}.", TotalDropletSurfaceArea);
            Logger.WriteLine("The external surface area of the scanned lava droplet is {0}.", ExternalDropletSurfaceArea);
        }

        return PuzzleResult.Create(TotalDropletSurfaceArea, ExternalDropletSurfaceArea);
    }

    private sealed class SurfaceWalker : IGraph<Vector3>
    {
        public SurfaceWalker(
            HashSet<Vector3> outside,
            IEnumerable<Vector3> item,
            bool excludeInterior)
        {
            Outside = outside;
            Item = new(item);
            ExcludeInterior = excludeInterior;
        }

        public HashSet<(Vector3 Point, Vector3 Normal)> Surfaces { get; } = new();

        private HashSet<Vector3> Outside { get; }

        private HashSet<Vector3> Item { get; }

        private bool ExcludeInterior { get; }

        public IEnumerable<Vector3> Neighbors(Vector3 id)
        {
            foreach (var neighbor in id.Neighbors())
            {
                bool isNeighborInside = Item.Contains(neighbor);

                if (Outside.Contains(neighbor) || (!ExcludeInterior && isNeighborInside))
                {
                    yield return neighbor;
                }

                if (isNeighborInside && (ExcludeInterior || !Item.Contains(id)))
                {
                    Surfaces.Add((id, neighbor - id));
                }
            }
        }
    }
}
