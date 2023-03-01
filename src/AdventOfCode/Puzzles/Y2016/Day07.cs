// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Buffers;

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/4</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2016, 07, "Internet Protocol Version 7", RequiresData = true)]
public sealed class Day07 : Puzzle
{
    /// <summary>
    /// Gets the number of IPv7 addresses that support SSL.
    /// </summary>
    public int IPAddressesSupportingSsl { get; private set; }

    /// <summary>
    /// Gets the number of IPv7 addresses that support TLS.
    /// </summary>
    public int IPAddressesSupportingTls { get; private set; }

    /// <summary>
    /// Returns whether the specified IPv7 address supports SSL.
    /// </summary>
    /// <param name="address">The IPv7 address to test for SSL.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="address"/> supports SSL; otherwise <see langword="false"/>.
    /// </returns>
    internal static bool DoesIPAddressSupportSsl(string address)
    {
        ParseIPAddress(address, out List<string> supernets, out List<string> hypernets);

        var abas = new List<string>(supernets.Count);

        foreach (string value in supernets)
        {
            abas.AddRange(ExtractAbas(value));
        }

        if (abas.Count < 1)
        {
            return false;
        }

        foreach (string aba in abas)
        {
            string bab = aba[1] + aba[0..2];

            foreach (string hypernet in hypernets)
            {
                if (hypernet.Contains(bab, StringComparison.Ordinal))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Returns whether the specified IPv7 address supports TLS.
    /// </summary>
    /// <param name="address">The IPv7 address to test for TLS.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="address"/> supports TLS; otherwise <see langword="false"/>.
    /// </returns>
    internal static bool DoesIPAddressSupportTls(string address)
    {
        ParseIPAddress(address, out List<string> supernets, out List<string> hypernets);

        bool foundAbbaInSupernet = supernets.Any(DoesStringContainAbba);
        bool foundAbbaInHypernet = hypernets.Any(DoesStringContainAbba);

        return foundAbbaInSupernet && !foundAbbaInHypernet;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> addresses = await ReadResourceAsLinesAsync(cancellationToken);

        IPAddressesSupportingTls = addresses.Count(DoesIPAddressSupportTls);
        IPAddressesSupportingSsl = addresses.Count(DoesIPAddressSupportSsl);

        if (Verbose)
        {
            Logger.WriteLine("{0:N0} IPv7 addresses support TLS.", IPAddressesSupportingTls);
            Logger.WriteLine("{0:N0} IPv7 addresses support SSL.", IPAddressesSupportingSsl);
        }

        return PuzzleResult.Create(IPAddressesSupportingTls, IPAddressesSupportingSsl);
    }

    /// <summary>
    /// Returns whether the specified <see cref="string"/> contains an ABBA.
    /// </summary>
    /// <param name="value">The value to test for an ABBA.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> contains an ABBA; otherwise <see langword="false"/>.
    /// </returns>
    private static bool DoesStringContainAbba(string value)
    {
        for (int i = 0; i < value.Length - 3; i++)
        {
            char ch1 = value[i];
            char ch2 = value[i + 1];
            char ch3 = value[i + 2];
            char ch4 = value[i + 3];

            if (ch1 != ch2 && ch2 == ch3 && ch1 == ch4)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns the ABAs in the specified <see cref="string"/>.
    /// </summary>
    /// <param name="value">The value to extract ABA values from.</param>
    /// <returns>
    /// A <see cref="List{T}"/> containing the extracted ABA values.
    /// </returns>
    private static List<string> ExtractAbas(ReadOnlySpan<char> value)
    {
        var result = new List<string>(value.Length);

        for (int i = 0; i < value.Length - 2; i++)
        {
            char ch1 = value[i];
            char ch2 = value[i + 1];
            char ch3 = value[i + 2];

            char[] shared = ArrayPool<char>.Shared.Rent(3);

            try
            {
                shared[0] = ch1;
                shared[1] = ch2;
                shared[2] = ch3;

                if (ch1 != ch2 && ch1 == ch3)
                {
                    result.Add(new(shared.AsSpan(0, 3)));
                }
            }
            finally
            {
                ArrayPool<char>.Shared.Return(shared);
            }
        }

        return result;
    }

    /// <summary>
    /// Parses the specified IP address into supernets and hypernets.
    /// </summary>
    /// <param name="address">The IP addresses to parse.</param>
    /// <param name="supernets">When the method returns, contains the supernets.</param>
    /// <param name="hypernets">When the method returns, contains the hypernets.</param>
    private static void ParseIPAddress(string address, out List<string> supernets, out List<string> hypernets)
    {
        supernets = new List<string>();
        hypernets = new List<string>();

        var builder = new StringBuilder();

        bool isInHypernet = false;

        foreach (char ch in address)
        {
            if (isInHypernet && ch == ']')
            {
                isInHypernet = false;

                if (builder.Length > 0)
                {
                    hypernets.Add(builder.ToString());
                    builder.Clear();
                }

                continue;
            }
            else if (ch == '[')
            {
                isInHypernet = true;

                if (builder.Length > 0)
                {
                    supernets.Add(builder.ToString());
                    builder.Clear();
                }

                continue;
            }

            builder.Append(ch);
        }

        if (builder.Length > 0)
        {
            supernets.Add(builder.ToString());
            builder.Clear();
        }
    }
}
