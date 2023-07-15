// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// Represents metadata about a puzzle.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class PuzzleAttribute(int year, int day, string name) : Attribute
{
    /// <summary>
    /// Gets the name associated with the puzzle.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the day associated with the puzzle.
    /// </summary>
    public int Day { get; } = day;

    /// <summary>
    /// Gets or sets a value indicating whether the puzzle is hidden.
    /// </summary>
    public bool IsHidden { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the puzzle is too slow to be benchmarked.
    /// </summary>
    public bool IsSlow { get; set; }

    /// <summary>
    /// Gets or sets the minimum number of arguments required to solve the puzzle.
    /// </summary>
    public int MinimumArguments { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the puzzle requires input data.
    /// </summary>
    public bool RequiresData { get; set; }

    /// <summary>
    /// Gets the year associated with the puzzle.
    /// </summary>
    public int Year { get; } = year;
}
