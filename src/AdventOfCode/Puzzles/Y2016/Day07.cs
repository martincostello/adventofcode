// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2016/day/4</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day07 : Puzzle2016
    {
        /// <summary>
        /// Gets the number of IPv7 addresses that support TLS.
        /// </summary>
        public int IPAddressesSupportingTls { get; private set; }

        /// <summary>
        /// Returns whether the specified IPv7 address supports TLS.
        /// </summary>
        /// <param name="address">The IPv7 address to test for TLS.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="address"/> supports TLS; otherwise <see langword="false"/>.
        /// </returns>
        internal static bool DoesIPAddressSupportTls(string address)
        {
            IList<string> nonhypernets = new List<string>();
            IList<string> hypernets = new List<string>();

            StringBuilder builder = new StringBuilder();

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
                        nonhypernets.Add(builder.ToString());
                        builder.Clear();
                    }

                    continue;
                }

                builder.Append(ch);
            }

            if (builder.Length > 0)
            {
                nonhypernets.Add(builder.ToString());
                builder.Clear();
            }

            bool foundAbba = nonhypernets.Any(DoesStringContainAbba);
            bool foundAbbaInHypernet = hypernets.Any(DoesStringContainAbba);

            return foundAbba && !foundAbbaInHypernet;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> addresses = ReadResourceAsLines();

            IPAddressesSupportingTls = addresses.Count(DoesIPAddressSupportTls);

            Console.WriteLine("{0:N0} IPv7 addresses support TLS.", IPAddressesSupportingTls);

            return 0;
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
    }
}
