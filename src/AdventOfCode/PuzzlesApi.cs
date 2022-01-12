// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Text.Json.Serialization;

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class representing the puzzles API. This class cannot be inherited.
/// </summary>
internal static partial class PuzzlesApi
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
            .Select((p) => new PuzzleMetadata()
            {
                Name = p.Name,
                Year = p.Year,
                Day = p.Day,
                MinimumArguments = p.MinimumArguments,
                RequiresData = p.RequiresData,
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
    /// <param name="form">The form read from the HTTP request.</param>
    /// <param name="factory">The <see cref="PuzzleFactory"/> to use.</param>
    /// <param name="logger">The <see cref="ILogger"/> to use.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation to solve the puzzle.
    /// </returns>
    internal static async Task<IResult> SolvePuzzleAsync(
        int year,
        int day,
        IFormCollection form,
        PuzzleFactory factory,
        ILogger<Puzzle> logger,
        CancellationToken cancellationToken)
    {
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
            if (metadata.RequiresData)
            {
                if (form.Files["resource"] is not { } resource)
                {
                    return Results.Problem("No puzzle resource provided.", statusCode: StatusCodes.Status400BadRequest);
                }

                puzzle.Resource = resource.OpenReadStream();
            }

            if (form.TryGetValue("arguments", out var values))
            {
                arguments = values!.Select((p) => p!).ToArray();
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
            Log.FailedToSolvePuzzle(logger, ex, year, day);
            return Results.Problem("Failed to solve puzzle.", statusCode: StatusCodes.Status500InternalServerError);
        }

        stopwatch.Stop();

        var result = new PuzzleSolution
        {
            Year = year,
            Day = day,
            Solutions = solution.Solutions,
            Visualizations = solution.Visualizations,
            TimeToSolve = stopwatch.Elapsed.TotalMilliseconds,
        };

        return Results.Json(result);
    }

    /// <summary>
    /// A class representing metadata for a puzzle. This class cannot be inherited.
    /// </summary>
    internal sealed class PuzzleMetadata
    {
        /// <summary>
        /// Gets or sets the puzzle's name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the puzzle's year.
        /// </summary>
        [JsonPropertyName("year")]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the puzzle's day.
        /// </summary>
        [JsonPropertyName("day")]
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of arguments required for the puzzle.
        /// </summary>
        [JsonPropertyName("minimumArguments")]
        public int MinimumArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the puzzle requires data.
        /// </summary>
        [JsonPropertyName("requiresData")]
        public bool RequiresData { get; set; }

        /// <summary>
        /// Gets or sets the relative URI of the puzzle's solution endpoint.
        /// </summary>
        [JsonPropertyName("location")]
        public string Location { get; set; } = default!;
    }

    /// <summary>
    /// A class representing the solution for a puzzle. This class cannot be inherited.
    /// </summary>
    internal sealed class PuzzleSolution
    {
        /// <summary>
        /// Gets or sets the puzzle's year.
        /// </summary>
        [JsonPropertyName("year")]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the puzzle's day.
        /// </summary>
        [JsonPropertyName("day")]
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the puzzle's solutions.
        /// </summary>
        [JsonPropertyName("solutions")]
        public IList<object> Solutions { get; set; } = default!;

        /// <summary>
        /// Gets or sets the puzzle's visualizations.
        /// </summary>
        [JsonPropertyName("visualizations")]
        public IList<string> Visualizations { get; set; } = default!;

        /// <summary>
        /// Gets or sets the time taken to solve the puzzle in milliseconds.
        /// </summary>
        [JsonPropertyName("timeToSolve")]
        public double TimeToSolve { get; set; }
    }

    private static partial class Log
    {
        [LoggerMessage(1, LogLevel.Error, "Failed to solve puzzle for year {Year} and day {Day}.")]
        public static partial void FailedToSolvePuzzle(Microsoft.Extensions.Logging.ILogger logger, Exception exception, int year, int day);
    }
}
