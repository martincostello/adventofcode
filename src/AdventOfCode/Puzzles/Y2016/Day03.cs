// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/3</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 03, "Squares With Three Sides", RequiresData = true)]
public sealed class Day03 : Puzzle<int, int>
{
    /// <summary>
    /// Returns the number of valid triangles from the specified triangle instructions.
    /// </summary>
    /// <param name="dimensions">A collection of strings containing the dimensions of possible triangles.</param>
    /// <param name="readAsColumns">Whether to parse the dimensions as columns instead of rows.</param>
    /// <returns>
    /// The number of valid triangles in the dimensions specified in <paramref name="dimensions"/>.
    /// </returns>
    internal static int GetPossibleTriangleCount(IList<string> dimensions, bool readAsColumns)
    {
        var triangles = ParseTriangles(dimensions, readAsColumns);
        return triangles.Count((p) => IsValidTriangle(p.A, p.B, p.C));
    }

    /// <summary>
    /// Returns whether the dimensions of the specified triangle are valid.
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
        return await SolveWithLinesAsync(
            static (dimensions, logger, _) =>
            {
                int possibleTrianglesByColumns = GetPossibleTriangleCount(dimensions, readAsColumns: false);
                int possibleTrianglesByRows = GetPossibleTriangleCount(dimensions, readAsColumns: true);

                if (logger is { })
                {
                    logger.WriteLine("The number of possible triangles using rows is {0:N0}.", possibleTrianglesByColumns);
                    logger.WriteLine("The number of possible triangles using columns is {0:N0}.", possibleTrianglesByRows);
                }

                return (possibleTrianglesByColumns, possibleTrianglesByRows);
            },
            cancellationToken);
    }

    /// <summary>
    /// Parses the specified set of triangle dimensions.
    /// </summary>
    /// <param name="dimensions">The triangle dimensions to parse.</param>
    /// <param name="readAsColumns">Whether to parse the dimensions as columns instead of rows.</param>
    /// <returns>
    /// An <see cref="IList{T}"/> containing the parsed triangle dimensions.
    /// </returns>
    private static (int A, int B, int C)[] ParseTriangles(IList<string> dimensions, bool readAsColumns)
    {
        var result = new (int A, int B, int C)[dimensions.Count];

        for (int i = 0; i < dimensions.Count; i++)
        {
            string[] components = dimensions[i].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int a = Parse<int>(components[0]);
            int b = Parse<int>(components[1]);
            int c = Parse<int>(components[2]);

            result[i] = (a, b, c);
        }

        if (readAsColumns)
        {
            var resultFromColumns = new (int A, int B, int C)[result.Length];

            for (int i = 0; i < result.Length; i += 3)
            {
                var (a1, b1, c1) = result[i];
                var (a2, b2, c2) = result[i + 1];
                var (a3, b3, c3) = result[i + 2];

                resultFromColumns[i] = (a1, a2, a3);
                resultFromColumns[i + 1] = (b1, b2, b3);
                resultFromColumns[i + 2] = (c1, c2, c3);
            }

            result = resultFromColumns;
        }

        return result;
    }
}
