// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/8</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 08, "Matchsticks", RequiresData = true)]
public sealed class Day08 : Puzzle<int, int>
{
    /// <summary>
    /// Returns the number of characters in the specified collection of <see cref="string"/> if literals are encoded.
    /// </summary>
    /// <param name="collection">The values to get the encoded number of characters from.</param>
    /// <returns>The number of characters in <paramref name="collection"/> when encoded.</returns>
    internal static int GetEncodedCharacterCount(IEnumerable<string> collection)
        => collection.Sum(GetEncodedCharacterCount);

    /// <summary>
    /// Returns the number of characters in the specified <see cref="string"/> if literals are encoded.
    /// </summary>
    /// <param name="value">The value to get the encoded number of characters from.</param>
    /// <returns>The number of characters in <paramref name="value"/> when encoded.</returns>
    internal static int GetEncodedCharacterCount(string value)
    {
        var builder = new StringBuilder(value.Length + 2).Append('\"');

        for (int i = 0; i < value.Length; i++)
        {
            char current = value[i];

            switch (current)
            {
                case '\"':
                case '\\':
                case '\'':
                    builder.Append('\\');
                    break;

                default:
                    break;
            }

            builder.Append(current);
        }

        builder.Append('\"');

        return builder.Length;
    }

    /// <summary>
    /// Returns the number of literal characters in the specified collection of <see cref="string"/>.
    /// </summary>
    /// <param name="collection">The values to get the number of literal characters from.</param>
    /// <returns>The number of literal characters in <paramref name="collection"/>.</returns>
    internal static int GetLiteralCharacterCount(IEnumerable<string> collection)
        => collection.Sum((p) => GetLiteralCharacterCount(p));

    /// <summary>
    /// Returns the number of literal characters in the specified span.
    /// </summary>
    /// <param name="value">The value to get the number of literal characters from.</param>
    /// <returns>The number of literal characters in <paramref name="value"/>.</returns>
    internal static int GetLiteralCharacterCount(ReadOnlySpan<char> value)
    {
        int count = 0;

        // Remove quotes if present as first/last characters
        bool removeFirstQuote = value.Length > 0 && value[0] is '\"';
        bool removeLastQuote = value.Length > 1 && value[^1] is '\"';

        if (removeFirstQuote)
        {
            value = value[1..];
        }

        if (removeLastQuote)
        {
            value = value[0..^1];
        }

        if (value.Length > 0)
        {
            var characters = new Queue<char>(value.ToString());

            while (characters.Count > 0)
            {
                char current = characters.Dequeue();

                if (characters.Count > 0)
                {
                    switch (current)
                    {
                        case '\\':
                            char next = characters.Peek();

                            if (next is '\"' or '\'' or '\\')
                            {
                                characters.Dequeue();
                            }
                            else if (next == 'x' && characters.Count > 2)
                            {
                                characters.Dequeue();
                                characters.Dequeue();
                                characters.Dequeue();
                            }

                            break;

                        default:
                            break;
                    }
                }

                count++;
            }
        }

        return count;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, _) =>
            {
                int countForCode = values.Sum((p) => p.Length);
                int countInMemory = GetLiteralCharacterCount(values);
                int countEncoded = GetEncodedCharacterCount(values);

                int first = countForCode - countInMemory;
                int second = countEncoded - countForCode;

                if (logger is { })
                {
                    logger.WriteLine(
                        "The number of characters of code for string literals minus the number of characters in memory for the values of the strings is {0:N0}.",
                        first);

                    logger.WriteLine(
                        "The total number of characters to represent the newly encoded strings minus the number of characters of code in each original string literal is {0:N0}.",
                        second);
                }

                return (first, second);
            },
            cancellationToken);
    }
}
