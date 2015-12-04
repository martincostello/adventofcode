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
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    internal static class Program
    {
        internal static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No secret key specified.");
                return -1;
            }

            string secretKey = args[0];

            int answer5 = GetLowestPositiveNumberWithStartingZeroes(secretKey, 5);

            Console.WriteLine();
            Console.WriteLine("The lowest positive number for a hash starting with five zeroes is {0}.", answer5);

            int answer6 = GetLowestPositiveNumberWithStartingZeroes(secretKey, 6);

            Console.WriteLine();
            Console.WriteLine("The lowest positive number for a hash starting with six zeroes is {0}.", answer6);

            Console.ReadKey();

            return 0;
        }

        private static int GetLowestPositiveNumberWithStartingZeroes(string secretKey, int zeroes)
        {
            int? answer = null;
            string prefix = new string('0', zeroes);

            using (HashAlgorithm algorithm = HashAlgorithm.Create("MD5"))
            {
                for (int i = 1; !answer.HasValue && i < int.MaxValue; i++)
                {
                    string value = string.Format(CultureInfo.InvariantCulture, "{0}{1}", secretKey, i);
                    byte[] buffer = Encoding.UTF8.GetBytes(value);

                    byte[] hashBytes = algorithm.ComputeHash(buffer);

                    StringBuilder builder = new StringBuilder();

                    foreach (byte b in hashBytes)
                    {
                        builder.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", b);
                    }

                    string hash = builder.ToString();

                    if (hash.StartsWith(prefix, StringComparison.Ordinal))
                    {
                        answer = i;
                    }

                    if (i % 100000 == 0)
                    {
                        Console.Write('.');
                    }
                }
            }

            if (answer == null)
            {
                throw new ArgumentException("No answer was found for the specified secret key.", nameof(secretKey));
            }

            return answer.Value;
        }
    }
}
