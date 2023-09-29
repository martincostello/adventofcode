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
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The total surface area of the lava droplet described by <paramref name="cubes"/>.
    /// </returns>
    public static int GetSurfaceArea(
        IList<string> cubes,
        bool excludeInterior,
        CancellationToken cancellationToken = default)
    {
        var droplet = cubes
            .Select((p) => p.AsNumberTriple<int>())
            .Select((p) => new Vector3(p.First, p.Second, p.Third))
            .ToHashSet();

        float minX = droplet.Min((p) => p.X);
        float maxX = droplet.Max((p) => p.X);
        float minY = droplet.Min((p) => p.Y);
        float maxY = droplet.Max((p) => p.Y);
        float minZ = droplet.Min((p) => p.Z);
        float maxZ = droplet.Max((p) => p.Z);

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

        outside.Not(droplet);

        // Find a point that is definitely not already touching the surface
        var origin = new Vector3(minX - 1, minY - 1, minZ - 1);

#pragma warning disable SA1010
        HashSet<(Vector3 Point, Vector3 Normal)> surfaces = [];
#pragma warning restore SA1010

        // Find all of the surfaces that can be reached from outside the droplet
        _ = PathFinding.BreadthFirst(Neighbors, origin, cancellationToken);

        return surfaces.Count;

        IEnumerable<Vector3> Neighbors(Vector3 point)
        {
            foreach (var neighbor in point.Neighbors())
            {
                bool isNeighborInside = droplet.Contains(neighbor);

                if (outside.Contains(neighbor) || (!excludeInterior && isNeighborInside))
                {
                    yield return neighbor;
                }

                if (isNeighborInside && (excludeInterior || !droplet.Contains(point)))
                {
                    surfaces.Add((point, neighbor - point));
                }
            }
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        var cubes = await ReadResourceAsLinesAsync(cancellationToken);

        TotalDropletSurfaceArea = GetSurfaceArea(cubes, excludeInterior: false, cancellationToken);
        ExternalDropletSurfaceArea = GetSurfaceArea(cubes, excludeInterior: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The total surface area of the scanned lava droplet is {0}.", TotalDropletSurfaceArea);
            Logger.WriteLine("The external surface area of the scanned lava droplet is {0}.", ExternalDropletSurfaceArea);
        }

        return PuzzleResult.Create(TotalDropletSurfaceArea, ExternalDropletSurfaceArea);
    }
}
