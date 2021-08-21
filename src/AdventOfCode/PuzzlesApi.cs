// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class representing the puzzles API. This class cannot be inherited.
/// </summary>
internal static class PuzzlesApi
{
    /// <summary>
    /// Gets the available puzzles as an asynchronous operation.
    /// </summary>
    /// <param name="attributes">The available puzzles.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation to solve the puzzle.
    /// </returns>
    internal static IResult GetPuzzlesAsync(IEnumerable<PuzzleAttribute> attributes)
    {
        var puzzles = attributes
            .OrderBy((p) => p.Year)
            .ThenBy((p) => p.Day)
            .Select((p) =>
            new
            {
                p.Year,
                p.Day,
                p.MinimumArguments,
                p.RequiresData,
                Location = FormattableString.Invariant($"/api/puzzles/{p.Year}/{p.Day}/solve"),
            })
            .ToList();

        return Results.Json(puzzles);
    }

    /// <summary>
    /// Solves the puzzle associated with the specified HTTP request as an asynchronous operation.
    /// </summary>
    /// <param name="year">The year the puzzle to solve is from.</param>
    /// <param name="day">The day the puzzle to solve is from.</param>
    /// <param name="request">The HTTP request.</param>
    /// <param name="factory">The <see cref="PuzzleFactory"/> to use.</param>
    /// <param name="logger">The <see cref="ILogger"/> to use.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation to solve the puzzle.
    /// </returns>
    internal static async Task<IResult> SolvePuzzleAsync(
        int year,
        int day,
        HttpRequest request,
        PuzzleFactory factory,
        ILogger<Puzzle> logger,
        CancellationToken cancellationToken)
    {
        if (!request.HasFormContentType)
        {
            return Results.Problem("The specified media type is not supported.", statusCode: StatusCodes.Status415UnsupportedMediaType);
        }

        Puzzle puzzle;

        try
        {
            puzzle = factory.Create(year, day);
        }
        catch (PuzzleException ex)
        {
            return Results.Problem(ex.Message, statusCode: StatusCodes.Status404NotFound);
        }

        var metadata = puzzle.Metadata();

        if (metadata.IsHidden)
        {
            return Results.Problem("This puzzle cannot be solved.", statusCode: StatusCodes.Status403Forbidden);
        }

        string[] arguments = Array.Empty<string>();

        if (metadata.RequiresData || metadata.MinimumArguments > 0)
        {
            var form = await request.ReadFormAsync(cancellationToken);

            if (metadata.RequiresData)
            {
                if (!form.TryGetValue("resource", out var resource))
                {
                    return Results.Problem("No puzzle resource provided.", statusCode: StatusCodes.Status400BadRequest);
                }

                puzzle.Resource = new MemoryStream(Encoding.UTF8.GetBytes(resource));
            }

            if (form.TryGetValue("arguments", out var values))
            {
                arguments = values.Select((p) => p).ToArray();
            }
        }

        var timeout = TimeSpan.FromMinutes(1);

        var stopwatch = Stopwatch.StartNew();

        PuzzleResult solution;

        try
        {
            solution = await puzzle.SolveAsync(arguments, cancellationToken).WaitAsync(timeout, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return Results.Problem($"The puzzle was not solved within {timeout}.", statusCode: StatusCodes.Status408RequestTimeout);
        }
        catch (PuzzleException ex)
        {
            return Results.Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
#pragma warning disable CA1031
        catch (Exception ex)
#pragma warning restore CA1031
        {
            logger.LogError(ex, "Failed to solve puzzle for year {Year} and day {Day}.", year, day);
            return Results.Problem("Failed to solve puzzle.", statusCode: StatusCodes.Status500InternalServerError);
        }

        stopwatch.Stop();

        var result = new
        {
            year,
            day,
            solutions = solution.Solutions,
            visualizations = solution.Visualizations,
            timeToSolve = stopwatch.Elapsed.TotalMilliseconds,
        };

        return Results.Json(result);
    }
}
