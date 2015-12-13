// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.IO;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/12</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day12 : IPuzzle
    {
        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No input file path specified.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            JToken document = JToken.Parse(File.ReadAllText(args[0]));

            long sum = SumIntegerValues(document);

            Console.WriteLine("The sum of the integers in the JSON document is {0:N0}.", sum);

            return 0;
        }

        internal static long SumIntegerValues(JToken token)
        {
            JValue value = token as JValue;

            long sum = 0;

            if (value != null && value.Type == JTokenType.Integer)
            {
                sum = (long)value.Value;
            }
            else
            {
                foreach (JToken child in token.Children())
                {
                    sum += SumIntegerValues(child);
                }
            }

            return sum;
        }
    }
}
