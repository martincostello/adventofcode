// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   Program.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day4
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A console application that solves <c>http://adventofcode.com/day/4</c>. This class cannot be inherited.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry-point to the application.
        /// </summary>
        /// <param name="args">The arguments to the application.</param>
        /// <returns>The exit code from the application.</returns>
        internal static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No secret key specified.");
                return -1;
            }

            string secretKey = args[0];

            Stopwatch stopwatch = Stopwatch.StartNew();

            int answerFor5 = GetLowestPositiveNumberWithStartingZeroes(secretKey, 5);
            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine(
                "The lowest positive number for a hash starting with five zeroes is {0}. Took {1:N2} seconds.",
                answerFor5,
                stopwatch.Elapsed.TotalSeconds);

            stopwatch.Reset();
            stopwatch.Start();

            int answerFor6 = GetLowestPositiveNumberWithStartingZeroes(secretKey, 6);

            Console.WriteLine();
            Console.WriteLine(
                "The lowest positive number for a hash starting with six zeroes is {0}. Took {1:N2} seconds.",
                answerFor6,
                stopwatch.Elapsed.TotalSeconds);

            return 0;
        }

        /// <summary>
        /// Gets the lowest positive integer which when combined with a secret key has an MD5 hash whose
        /// hexadecimal representation starts with the specified number of zeroes.
        /// </summary>
        /// <param name="secretKey">The secret key to use.</param>
        /// <param name="zeroes">The number of zeroes to get the value for.</param>
        /// <returns>The lowest positive integer that generates an MD5 hash with the number of zeroes specified.</returns>
        private static int GetLowestPositiveNumberWithStartingZeroes(string secretKey, int zeroes)
        {
            string prefix = new string('0', zeroes);

            var bag = new ConcurrentBag<int>();
            var source = Partitioner.Create(1, int.MaxValue - 1, 50000);

            Parallel.ForEach(
                source,
                (range, state) =>
                {
                    // Does this range start at a value greater than an already found value?
                    if (bag.Count > 0 && range.Item1 > bag.Min())
                    {
                        return;
                    }

                    for (int i = range.Item1; !state.ShouldExitCurrentIteration && i < range.Item2; i++)
                    {
                        string value = string.Format(CultureInfo.InvariantCulture, "{0}{1}", secretKey, i);
                        string hash = ComputeMD5(value);

                        if (hash.StartsWith(prefix, StringComparison.Ordinal))
                        {
                            // We've found a possible solution stop this loop
                            bag.Add(i);
                        }

                        if (i % 100000 == 0)
                        {
                            Console.Write('.');
                        }
                    }
                });

            if (bag.Count < 1)
            {
                throw new ArgumentException("No answer was found for the specified secret key.", nameof(secretKey));
            }

            return bag
                .ToArray()
                .Min();
        }

        /// <summary>
        /// Gets the hexadecimal representation of the MD5 hash of the specified <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to get the MD5 hash of.</param>
        /// <returns>The hexadecimal MD5 hash of <paramref name="value"/>.</returns>
        private static string ComputeMD5(string value)
        {
            using (HashAlgorithm algorithm = HashAlgorithm.Create("MD5"))
            {
                byte[] buffer = Encoding.UTF8.GetBytes(value);

                byte[] hashBytes = algorithm.ComputeHash(buffer);

                StringBuilder builder = new StringBuilder();

                foreach (byte b in hashBytes)
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", b);
                }

                return builder.ToString();
            }
        }
    }
}
