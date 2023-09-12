// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class that creates instances of <see cref="IPuzzle"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PuzzleFactory"/> class.
/// </remarks>
/// <param name="cache">The <see cref="ICache"/> to use.</param>
/// <param name="logger">The <see cref="ILogger"/> to use.</param>
public class PuzzleFactory(ICache cache, ILogger logger)
{
    /// <summary>
    /// Creates the puzzle for the specified year and day.
    /// </summary>
    /// <param name="year">The year associated with the puzzle.</param>
    /// <param name="day">The day associated with the puzzle.</param>
    /// <returns>
    /// The <see cref="Puzzle"/> associated with the specified year and day.
    /// </returns>
    public Puzzle Create(int year, int day)
    {
        Type? type;

        if (day < 1 ||
            year < 2015 ||
            year > DateTime.UtcNow.Year ||
            (type = GetPuzzleType(year, day)) == null ||
            Activator.CreateInstance(type) is not Puzzle puzzle)
        {
            throw new PuzzleException("The year and/or puzzle number specified is invalid.");
        }

        puzzle.Cache = cache;
        puzzle.Logger = logger;
        puzzle.Verbose = true;

        return puzzle;
    }

    /// <summary>
    /// Gets the puzzle type to use for the specified number.
    /// </summary>
    /// <param name="year">The year to get the puzzle for.</param>
    /// <param name="day">The day to get the puzzle for.</param>
    /// <returns>
    /// The <see cref="Type"/> for the specified year and puzzle number, if found; otherwise <see langword="null"/>.
    /// </returns>
    private static Type? GetPuzzleType(int year, int day)
    {
        string typeName = string.Format(
            CultureInfo.InvariantCulture,
            "MartinCostello.AdventOfCode.Puzzles.Y{0}.Day{1:00}",
            year,
            day);

        return Type.GetType(typeName);
    }
}
