// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Security.Cryptography;

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/14</c>. This class cannot be inherited.
/// </summary>
[Puzzle("One-Time Pad", 2016, 14, MinimumArguments = 1, IsHidden = true)]
public sealed class Day14 : Puzzle
{
    /// <summary>
    /// Gets the index that produces the 64th one-time pad key.
    /// </summary>
    public int IndexOfKey64 { get; private set; }

    /// <summary>
    /// Gets the index that produces the 64th one-time pad key
    /// when key stretching is in use.
    /// </summary>
    public int IndexOfKey64WithStretching { get; private set; }

    /// <summary>
    /// Returns the index of the one-time pad key for the specified ordinal.
    /// </summary>
    /// <param name="salt">The salt to use to generate the one-time pad key.</param>
    /// <param name="ordinal">The ordinal of the key to return the index of.</param>
    /// <param name="useKeyStretching">Whether to use key stretching.</param>
    /// <param name="cancellationToken">The cancellation token to use.</param>
    /// <returns>
    /// The index of the one-time pad key generated using <paramref name="salt"/>
    /// with the ordinal value specified by <paramref name="ordinal"/>.
    /// </returns>
    internal static int GetOneTimePadKeyIndex(
        string salt,
        int ordinal,
        bool useKeyStretching,
        CancellationToken cancellationToken)
    {
        int current = 0;
        var cache = new Dictionary<string, string>();

        for (int i = 0; current < ordinal; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            string value = salt + i.ToString(CultureInfo.InvariantCulture);
            string key = GenerateKey(value, useKeyStretching, cache);

            char triple = GetTripleCharacter(key);

            if (triple == default(char))
            {
                continue;
            }

            int next = i + 1;
            int last = next + 1000;

            bool found5InARow = false;

            for (int j = next; j <= last; j++)
            {
                value = salt + j.ToString(CultureInfo.InvariantCulture);
                key = GenerateKey(value, useKeyStretching, cache);

                if (HasFiveRepeatitionsOfCharacter(key, triple))
                {
                    found5InARow = true;
                    break;
                }
            }

            if (found5InARow)
            {
                current++;

                if (current == ordinal)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        string salt = args[0];

        IndexOfKey64 = GetOneTimePadKeyIndex(salt, 64, useKeyStretching: false, cancellationToken);
        IndexOfKey64WithStretching = GetOneTimePadKeyIndex(salt, 64, useKeyStretching: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine($"The index that produces the 64th one-time pad key is {IndexOfKey64:N0}.");
            Logger.WriteLine($"The index that produces the 64th one-time pad key when using key stretching is {IndexOfKey64WithStretching:N0}.");
        }

        return PuzzleResult.Create(IndexOfKey64, IndexOfKey64WithStretching);
    }

    /// <summary>
    /// Generates a one-time pad key for the specified value.
    /// </summary>
    /// <param name="value">The value to generate the one-time pad key for.</param>
    /// <param name="useKeyStretching">Whether to use key stretching.</param>
    /// <param name="cache">A cache of pre-generated hashes.</param>
    /// <returns>
    /// A <see cref="string"/> containing the one-time pad key derived from <paramref name="value"/>.
    /// </returns>
    private static string GenerateKey(
        string value,
        bool useKeyStretching,
        IDictionary<string, string> cache)
    {
        string key = string.Concat(value, useKeyStretching ? "|s" : "|u");

        if (!cache.TryGetValue(key, out string? result))
        {
            result = value;

            int rounds = useKeyStretching ? 2017 : 1;

            for (int i = 0; i < rounds; i++)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(result);
                byte[] hash = MD5.HashData(buffer);

                result = GetStringForHash(hash);
            }

            cache[key] = result;
        }

        return result;
    }

    /// <summary>
    /// Returns whether the specified <see cref="string"/> contains five consecutive
    /// occurrences of the specified character.
    /// </summary>
    /// <param name="value">The value to search for five consecutive characters in.</param>
    /// <param name="ch">The character to search for five consecutive occurrences of.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> contains that character
    /// specified by <paramref name="ch"/> five times consecutively.
    /// </returns>
    private static bool HasFiveRepeatitionsOfCharacter(string value, char ch)
    {
        int start = value.IndexOf(ch, StringComparison.Ordinal);

        if (start > -1)
        {
            for (int i = start; i < value.Length - 4; i++)
            {
                if (value[i] == ch &&
                    value[i + 1] == ch &&
                    value[i + 2] == ch &&
                    value[i + 3] == ch &&
                    value[i + 4] == ch)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Returns the character that first occurs three times consecutively
    /// in the specified <see cref="string"/>, if any.
    /// </summary>
    /// <param name="value">The value to search for three consecutive characters in.</param>
    /// <returns>
    /// The character that first occurs three times consecutively in
    /// <paramref name="value"/>, if any; otherwise the null character.
    /// </returns>
    private static char GetTripleCharacter(string value)
    {
        for (int i = 0; i < value.Length - 2; i++)
        {
            char ch = value[i];

            if (value[i + 1] == ch && value[i + 2] == ch)
            {
                return ch;
            }
        }

        return default;
    }

    /// <summary>
    /// Converts the specified <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> to a
    /// hexadecimal <see cref="string"/> representation of the hash.
    /// </summary>
    /// <param name="bytes">The hash bytes to generate the string representation of.</param>
    /// <returns>
    /// A <see cref="string"/> containing the hexadecimal representation of <paramref name="bytes"/>.
    /// </returns>
    private static string GetStringForHash(ReadOnlySpan<byte> bytes)
#pragma warning disable CA1308
        => Convert.ToHexString(bytes).ToLowerInvariant();
#pragma warning restore CA1308
}
