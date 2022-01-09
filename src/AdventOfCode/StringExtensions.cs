// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class containing extension methods for strings. This class cannot be inherited.
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Birfurcates the specified string.
    /// </summary>
    /// <param name="value">The string to bifurcate.</param>
    /// <param name="separator">The separator between the two strings.</param>
    /// <returns>
    /// The two strings separated by the specified character.
    /// </returns>
    public static (string First, string Second) Bifurcate(this string value, char separator)
        => value.AsSpan().Bifurcate(separator);

    /// <summary>
    /// Birfurcates the specified span.
    /// </summary>
    /// <param name="value">The span to bifurcate.</param>
    /// <param name="separator">The separator between the two spans.</param>
    /// <returns>
    /// The two strings separated by the specified character.
    /// </returns>
    public static (string First, string Second) Bifurcate(this ReadOnlySpan<char> value, char separator)
    {
        var tokens = value.Tokenize(separator);

        tokens.MoveNext();
        var first = tokens.Current;

        tokens.MoveNext();
        var second = tokens.Current;

        return (new(first), new(second));
    }

    /// <summary>
    /// Trifurcates the specified string.
    /// </summary>
    /// <param name="value">The string to trifurcate.</param>
    /// <param name="separator">The separator between the three strings.</param>
    /// <returns>
    /// The three strings separated by the specified character.
    /// </returns>
    public static (string First, string Second, string Third) Trifurcate(this string value, char separator)
        => value.AsSpan().Trifurcate(separator);

    /// <summary>
    /// Trifurcates the specified span.
    /// </summary>
    /// <param name="value">The span to trifurcate.</param>
    /// <param name="separator">The separator between the three spans.</param>
    /// <returns>
    /// The three strings separated by the specified character.
    /// </returns>
    public static (string First, string Second, string Third) Trifurcate(this ReadOnlySpan<char> value, char separator)
    {
        var tokens = value.Tokenize(separator);

        tokens.MoveNext();
        var first = tokens.Current;

        tokens.MoveNext();
        var second = tokens.Current;

        tokens.MoveNext();
        var third = tokens.Current;

        return (new(first), new(second), new(third));
    }

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
    /// Parses the specified string as a pair of numbers.
    /// </summary>
    /// <typeparam name="T">The type of the numbers.</typeparam>
    /// <param name="value">The string to parse as a pair of numbers.</param>
    /// <param name="separator">The optional separator between the two numbers.</param>
    /// <returns>
    /// The two numbers parsed from the string.
    /// </returns>
    public static (T First, T Second) AsNumberPair<T>(this string value, char separator = ',')
        where T : INumber<T>
        => value.AsSpan().AsNumberPair<T>(separator);

    /// <summary>
    /// Parses the specified span as a pair of numbers.
    /// </summary>
    /// <typeparam name="T">The type of the numbers.</typeparam>
    /// <param name="value">The span to parse as a pair of numbers.</param>
    /// <param name="separator">The optional separator between the two numbers.</param>
    /// <returns>
    /// The two numbers parsed from the span.
    /// </returns>
    public static (T First, T Second) AsNumberPair<T>(this ReadOnlySpan<char> value, char separator = ',')
        where T : INumber<T>
    {
        var tokens = value.Tokenize(separator);

        tokens.MoveNext();
        T first = Puzzle.Parse<T>(tokens.Current);

        tokens.MoveNext();
        T second = Puzzle.Parse<T>(tokens.Current);

        return (first, second);
    }

    /// <summary>
    /// Parses the specified string as a triple of numbers.
    /// </summary>
    /// <typeparam name="T">The type of the numbers.</typeparam>
    /// <param name="value">The string to parse as a triple of numbers.</param>
    /// <param name="separator">The optional separator between the three numbers.</param>
    /// <returns>
    /// The three numbers parsed from the string.
    /// </returns>
    public static (T First, T Second, T Third) AsNumberTriple<T>(this string value, char separator = ',')
        where T : INumber<T>
        => value.AsSpan().AsNumberTriple<T>(separator);

    /// <summary>
    /// Parses the specified span as a triple of numbers.
    /// </summary>
    /// <typeparam name="T">The type of the numbers.</typeparam>
    /// <param name="value">The span to parse as a triple of numbers.</param>
    /// <param name="separator">The optional separator between the three numbers.</param>
    /// <returns>
    /// The three numbers parsed from the span.
    /// </returns>
    public static (T First, T Second, T Third) AsNumberTriple<T>(this ReadOnlySpan<char> value, char separator = ',')
        where T : INumber<T>
    {
        var tokens = value.Tokenize(separator);

        tokens.MoveNext();
        T first = Puzzle.Parse<T>(tokens.Current);

        tokens.MoveNext();
        T second = Puzzle.Parse<T>(tokens.Current);

        tokens.MoveNext();
        T third = Puzzle.Parse<T>(tokens.Current);

        return (first, second, third);
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
        => value.Split(separator, options).Select((p) => Puzzle.Parse<T>(p));
}
