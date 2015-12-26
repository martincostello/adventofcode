// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/12</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day12 : Puzzle
    {
        /// <summary>
        /// Gets the sum of the integers in the JSON document.
        /// </summary>
        internal long Sum { get; private set; }

        /// <inheritdoc />
        protected override bool IsFirstArgumentFilePath => true;

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Sums the integer values in the specified JSON token, ignoring any values from
        /// child tokens that contain the specified string value, if specified.
        /// </summary>
        /// <param name="token">The JSON token.</param>
        /// <param name="valueToIgnore">The tokens to ignore if they contain this value.</param>
        /// <returns>The sum of the tokens in <paramref name="token"/>.</returns>
        internal static long SumIntegerValues(JToken token, string valueToIgnore)
        {
            JValue value = token as JValue;

            long sum = 0;

            if (value != null && value.Type == JTokenType.Integer)
            {
                sum = (long)value.Value;
            }
            else
            {
                JToken[] children = token
                    .Children()
                    .ToArray();

                bool ignore = children
                    .OfType<JProperty>()
                    .Where((p) => p.Value.Type == JTokenType.String)
                    .Where((p) => (string)((JValue)p.Value).Value == valueToIgnore)
                    .Any();

                if (!ignore)
                {
                    foreach (JToken child in children)
                    {
                        ignore = child
                            .Children<JProperty>()
                            .Where((p) => p.Value.Type == JTokenType.String)
                            .Where((p) => (string)((JValue)p.Value).Value == valueToIgnore)
                            .Any();

                        if (!ignore)
                        {
                            sum += SumIntegerValues(child, valueToIgnore);
                        }
                    }
                }
            }

            return sum;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            JToken document = JToken.Parse(File.ReadAllText(args[0]));
            string keyToIgnore = args.Length > 1 ? args[1] : string.Empty;

            Sum = SumIntegerValues(document, keyToIgnore);

            Console.WriteLine("The sum of the integers in the JSON document is {0:N0}.", Sum);

            return 0;
        }
    }
}
