// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Numerics;
using System.Reflection;

namespace MartinCostello.AdventOfCode;

/// <summary>
/// The base class for puzzles.
/// </summary>
public abstract class Puzzle : IPuzzle
{
    /// <summary>
    /// Gets or sets the optional resource stream associated with the puzzle.
    /// </summary>
    public Stream? Resource { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the puzzle should be run verbosely.
    /// </summary>
    public bool Verbose { get; set; }

    /// <summary>
    /// Gets or sets the logger to use.
    /// </summary>
    internal ILogger Logger { get; set; } = new ConsoleLogger();

    /// <summary>
    /// Gets the minimum number of arguments required to solve the puzzle.
    /// </summary>
    protected virtual int MinimumArguments
        => Metadata()?.MinimumArguments ?? 0;

    /// <inheritdoc />
    public async Task<PuzzleResult> SolveAsync(string[] args, CancellationToken cancellationToken)
    {
        if (!EnsureArguments(args, MinimumArguments))
        {
            string message = string.Format(
                CultureInfo.InvariantCulture,
                "At least {0:N0} argument{1} {2} required.",
                MinimumArguments,
                MinimumArguments == 1 ? string.Empty : "s",
                MinimumArguments == 1 ? "is" : "are");

            throw new PuzzleException(message);
        }

        return await SolveCoreAsync(args, cancellationToken);
    }

    /// <summary>
    /// Returns the metadata for the puzzle.
    /// </summary>
    /// <returns>
    /// The puzzle's metadata.
    /// </returns>
    internal PuzzleAttribute Metadata()
        => GetType().GetCustomAttribute<PuzzleAttribute>() !;

    /// <summary>
    /// Replaces the format items in a specified string with the string representations
    /// of corresponding objects in a specified array.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An object array that contains zero or more objects to format.</param>
    /// <returns>
    /// A copy of format in which the format items have been replaced by the string
    /// representation of the corresponding objects in args.
    /// </returns>
    protected internal static string Format(string format, params object[] args)
        => string.Format(CultureInfo.InvariantCulture, format, args);

    /// <summary>
    /// Parses the specified <see cref="string"/> as an <see cref="int"/>.
    /// </summary>
    /// <param name="s">The value to parse.</param>
    /// <returns>
    /// The parsed value of <paramref name="s"/>.
    /// </returns>
    protected internal static int ParseInt32(string s)
        => ParseInt32(s, NumberStyles.Integer);

    /// <summary>
    /// Parses the specified <see cref="ReadOnlySpan{T}"/> as an <see cref="int"/>.
    /// </summary>
    /// <param name="s">The value to parse.</param>
    /// <param name="style">
    /// An optional bitwise combination of enumeration values that indicates
    /// the style elements that can be present in <paramref name="s"/>.
    /// </param>
    /// <returns>
    /// The parsed value of <paramref name="s"/>.
    /// </returns>
    protected internal static int ParseInt32(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer)
        => int.Parse(s, style, CultureInfo.InvariantCulture);

    /// <summary>
    /// Parses the specified <see cref="string"/> as an <see cref="long"/>.
    /// </summary>
    /// <param name="s">The value to parse.</param>
    /// <param name="style">
    /// A bitwise combination of enumeration values that indicates
    /// the style elements that can be present in <paramref name="s"/>.
    /// </param>
    /// <returns>
    /// The parsed value of <paramref name="s"/>.
    /// </returns>
    protected internal static long ParseInt64(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer)
        => long.Parse(s, style, CultureInfo.InvariantCulture);

    /// <summary>
    /// Tries to parse the specified <see cref="string"/> as an <see cref="int"/>.
    /// </summary>
    /// <param name="s">The value to parse.</param>
    /// <param name="value">
    /// When the method returns contains the parsed value
    /// of <paramref name="s"/>; otherwise zero.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="s"/> was parsed
    /// successfully; otherwise <see langword="false"/>.
    /// </returns>
    protected internal static bool TryParseInt32(ReadOnlySpan<char> s, out int value)
        => int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);

    /// <summary>
    /// Parses the specified <see cref="string"/> as an <see cref="uint"/>.
    /// </summary>
    /// <param name="s">The value to parse.</param>
    /// <returns>
    /// The parsed value of <paramref name="s"/>.
    /// </returns>
    protected internal static uint ParseUInt32(ReadOnlySpan<char> s)
        => uint.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);

    /// <summary>
    /// Ensures that the specified number of arguments are present.
    /// </summary>
    /// <param name="args">The input arguments to the puzzle.</param>
    /// <param name="minimumLength">The minimum number of arguments required.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="args"/> is at least
    /// <paramref name="minimumLength"/> in length; otherwise <see langword="false"/>.
    /// </returns>
    protected static bool EnsureArguments(ICollection<string> args, int minimumLength)
        => args.Count >= minimumLength;

    /// <summary>
    /// Returns the <see cref="Stream"/> associated with the resource for the puzzle.
    /// </summary>
    /// <returns>
    /// A <see cref="Stream"/> containing the resource associated with the puzzle.
    /// </returns>
    protected Stream ReadResource()
    {
        var thisType = GetType();

        string year = thisType.Namespace!.Split('.')[^1];

        string name = FormattableString.Invariant(
            $"MartinCostello.{thisType.Assembly.GetName().Name}.Input.{year}.{thisType.Name}.input.txt");

        return thisType.Assembly.GetManifestResourceStream(name) !;
    }

    /// <summary>
    /// Returns the lines associated with the resource for the puzzle as a <see cref="string"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation which returns an
    /// <see cref="IList{T}"/> containing the lines of the resource associated with the puzzle.
    /// </returns>
    protected async Task<IList<string>> ReadResourceAsLinesAsync()
    {
        var lines = new List<string>();

        using var reader = new StreamReader(Resource ?? ReadResource(), leaveOpen: Resource is not null);

        string? value = null;

        while ((value = await reader.ReadLineAsync()) != null)
        {
            lines.Add(value);
        }

        return lines;
    }

    /// <summary>
    /// Returns the sequence associated with the resource for the puzzle as the specified type.
    /// </summary>
    /// <typeparam name="T">
    /// The type to read the resource as a sequence of.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation which returns an
    /// <see cref="IList{T}"/> containing the lines of the resource associated with the puzzle.
    /// </returns>
    protected async Task<IList<T>> ReadResourceAsSequenceAsync<T>()
    {
        var lines = await ReadResourceAsLinesAsync();

        if (typeof(T) == typeof(int))
        {
            return lines
                .Select((p) => ParseInt32(p))
                .Cast<T>()
                .ToList();
        }
        else if (typeof(T) == typeof(long))
        {
            return lines
                .Select((p) => ParseInt64(p))
                .Cast<T>()
                .ToList();
        }
        else if (typeof(T) == typeof(uint))
        {
            return lines
                .Select((p) => ParseUInt32(p))
                .Cast<T>()
                .ToList();
        }
        else if (typeof(T) == typeof(BigInteger))
        {
            return lines
                .Select((p) => BigInteger.Parse(p, CultureInfo.InvariantCulture))
                .Cast<T>()
                .ToList();
        }

        throw new NotSupportedException($"The {typeof(T).Name} type is not supported.");
    }

    /// <summary>
    /// Returns a <see cref="string"/> containing the content of the
    /// resource associated with the puzzle as an asynchronous operation.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that represents the asynchronous operation
    /// to return a <see cref="string"/> containing the content of the resource
    /// associated with the puzzle.
    /// </returns>
    protected async Task<string> ReadResourceAsStringAsync()
    {
        using var reader = new StreamReader(Resource ?? ReadResource(), leaveOpen: Resource is not null);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    /// Solves the puzzle given the specified arguments as an asynchronous operation.
    /// </summary>
    /// <param name="args">The input arguments to the puzzle.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous
    /// operation which returns the solution to the puzzle.
    /// </returns>
    protected abstract Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken);
}
