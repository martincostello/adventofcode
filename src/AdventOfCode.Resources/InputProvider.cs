// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Reflection;

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class to obtain the input for puzzles. This class cannot be inherited.
/// </summary>
public static class InputProvider
{
    private static readonly Assembly ThisAssembly = typeof(InputProvider).Assembly;

    /// <summary>
    /// Returns the <see cref="Stream"/> associated with the resource for the puzzle.
    /// </summary>
    /// <param name="year">The year to get the input for.</param>
    /// <param name="day">The day to get the input for.</param>
    /// <returns>
    /// A <see cref="Stream"/> containing the resource associated with the specified puzzle, if found.
    /// </returns>
    public static Stream? Get(int year, int day) =>
        ThisAssembly.GetManifestResourceStream(
            FormattableString.Invariant($"MartinCostello.AdventOfCode.Input.Y{year:0000}.Day{day:00}.input.txt"));
}
