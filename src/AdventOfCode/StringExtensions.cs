// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class containing extension methods for strings. This class cannot be inherited.
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Returns a string that is a mirror image of the string.
    /// </summary>
    /// <param name="value">The string to mirror.</param>
    /// <returns>
    /// A string which is the mirror of the original.
    /// </returns>
    public static string Mirror(this string value)
    {
        if (value.Length < 2)
        {
            return value;
        }

        char[] reversed = value.ToCharArray();
        Array.Reverse(reversed);

        return new(reversed);
    }

    /// <summary>
    /// Parse the specified string as a sequence of numbers.
    /// </summary>
    /// <typeparam name="T">The type of the numbers.</typeparam>
    /// <param name="value">The string to parse into a sequence.</param>
    /// <param name="separator">The optional separator character for the numbers.</param>
    /// <param name="options">The optional options to use to split the string.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> containing the parsed numbers.
    /// </returns>
    public static IEnumerable<T> AsNumbers<T>(this string value, char separator = ',', StringSplitOptions options = StringSplitOptions.None)
        where T : INumber<T>
        => value.Split(separator, options).Select(Puzzle.Parse<T>);
}
