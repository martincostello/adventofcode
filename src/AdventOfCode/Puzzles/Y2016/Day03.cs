// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle("Squares With Three Sides", 2016, 03, RequiresData = true)]
public sealed class Day03 : Puzzle
{
    /// <summary>
    /// Gets the number of possible triangles by columns.
    /// </summary>
    public int PossibleTrianglesByColumns { get; private set; }

    /// <summary>
    /// Gets the number of possible triangles by rows.
    /// </summary>
    public int PossibleTrianglesByRows { get; private set; }

    /// <summary>
    /// Returns the number of valid triangles from the specified triangle instructions.
    /// </summary>
    /// <param name="dimensions">A collection of strings containing the dimensions of possible triangles.</param>
    /// <param name="readAsColumns">Whether to parse the dimensions as columns instead of rows.</param>
    /// <returns>
    /// The number of valid triangles in the dimensions specified in <paramref name="dimensions"/>.
    /// </returns>
    internal static int GetPossibleTriangleCount(ICollection<string> dimensions, bool readAsColumns)
    {
        IList<(int A, int B, int C)> triangles = ParseTriangles(dimensions, readAsColumns);
        return triangles.Count((p) => IsValidTriangle(p.A, p.B, p.C));
    }

    /// <summary>
    /// Retuns whether the dimensions of the specified triangle are valid.
    /// </summary>
    /// <param name="a">The length of the first side.</param>
    /// <param name="b">The length of the second side.</param>
    /// <param name="c">The length of the third side.</param>
    /// <returns>
    /// <see langword="true"/> if the triangle with the specified dimensions is valid; otherwise <see langword="false"/>.
    /// </returns>
    internal static bool IsValidTriangle(int a, int b, int c)
    {
        return
            a + b > c &&
            a + c > b &&
            b + c > a;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> dimensions = await ReadResourceAsLinesAsync();

        PossibleTrianglesByRows = GetPossibleTriangleCount(dimensions, readAsColumns: false);
        PossibleTrianglesByColumns = GetPossibleTriangleCount(dimensions, readAsColumns: true);

        if (Verbose)
        {
            Logger.WriteLine("The number of possible triangles using rows is {0:N0}.", PossibleTrianglesByRows);
            Logger.WriteLine("The number of possible triangles using columns is {0:N0}.", PossibleTrianglesByColumns);
        }

        return PuzzleResult.Create(PossibleTrianglesByRows, PossibleTrianglesByColumns);
    }

    /// <summary>
    /// Parses the specified set of triangle dimensions.
    /// </summary>
    /// <param name="dimensions">The triangle dimensions to parse.</param>
    /// <param name="readAsColumns">Whether to parse the dimensions as columns instead of rows.</param>
    /// <returns>
    /// An <see cref="IList{T}"/> containing the parsed triangle dimensions.
    /// </returns>
    private static IList<(int A, int B, int C)> ParseTriangles(ICollection<string> dimensions, bool readAsColumns)
    {
        var result = new List<(int A, int B, int C)>(dimensions.Count);

        foreach (string dimension in dimensions)
        {
            string[] components = dimension.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int a = Parse<int>(components[0]);
            int b = Parse<int>(components[1]);
            int c = Parse<int>(components[2]);

            result.Add((a, b, c));
        }

        if (readAsColumns)
        {
            var resultFromColumns = new List<(int A, int B, int C)>(result.Count);

            for (int i = 0; i < result.Count; i += 3)
            {
                var (a1, b1, c1) = result[i];
                var (a2, b2, c2) = result[i + 1];
                var (a3, b3, c3) = result[i + 2];

                resultFromColumns.Add((a1, a2, a3));
                resultFromColumns.Add((b1, b2, b3));
                resultFromColumns.Add((c1, c2, c3));
            }

            result = resultFromColumns;
        }

        return result;
    }
}
