// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

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
    /// Creates the puzzle of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the puzzle.</typeparam>
    /// <returns>
    /// The <see cref="Puzzle"/> associated with the specified year and day.
    /// </returns>
    public Puzzle Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>()
        where T : Puzzle => Create(typeof(T));

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
        var type = GetPuzzleType(year, day);
        return Create(type);
    }

    /// <summary>
    /// Gets the puzzle type to use for the specified number.
    /// </summary>
    /// <param name="year">The year to get the puzzle for.</param>
    /// <param name="day">The day to get the puzzle for.</param>
    /// <returns>
    /// The <see cref="Type"/> for the specified year and puzzle number.
    /// </returns>
    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    [UnconditionalSuppressMessage("Trimmer", "IL2057", Justification = "Assembly is trim rooted and only loads types from this assembly.")]
    private static Type GetPuzzleType(int year, int day)
    {
        if (day < 1 ||
            year < 2015 ||
            year > DateTime.UtcNow.Year)
        {
            throw new PuzzleException("The year and/or puzzle number specified is invalid.");
        }

        string typeName = string.Format(
            CultureInfo.InvariantCulture,
            "MartinCostello.AdventOfCode.Puzzles.Y{0}.Day{1:00}, AdventOfCode",
            year,
            day);

        var type = Type.GetType(typeName);

        if (type is null || type.IsAssignableFrom(typeof(Puzzle)))
        {
            throw new PuzzleException("The year and/or puzzle number specified is invalid.");
        }

        return type;
    }

    private Puzzle Create(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type)
    {
        if (Activator.CreateInstance(type) is not Puzzle puzzle)
        {
            throw new PuzzleException("The year and/or puzzle number specified is invalid.");
        }

        puzzle.Cache = cache;
        puzzle.Logger = logger;
        puzzle.Verbose = true;

        return puzzle;
    }
}
