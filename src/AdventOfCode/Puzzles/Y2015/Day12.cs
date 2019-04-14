// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System.Linq;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2015/day/12</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day12 : Puzzle2015
    {
        /// <summary>
        /// Gets the sum of the integers in the JSON document.
        /// </summary>
        internal long Sum { get; private set; }

        /// <summary>
        /// Sums the integer values in the specified JSON token, ignoring any values from
        /// child tokens that contain the specified string value, if specified.
        /// </summary>
        /// <param name="token">The JSON token.</param>
        /// <param name="valueToIgnore">The tokens to ignore if they contain this value.</param>
        /// <returns>The sum of the tokens in <paramref name="token"/>.</returns>
        internal static long SumIntegerValues(JToken token, string valueToIgnore)
        {
            long sum = 0;

            if (token is JValue value && value.Type == JTokenType.Integer)
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
            var document = JToken.Parse(ReadResourceAsString());
            string keyToIgnore = args.Length > 0 ? args[0] : string.Empty;

            Sum = SumIntegerValues(document, keyToIgnore);

            if (Verbose)
            {
                Logger.WriteLine("The sum of the integers in the JSON document is {0:N0}.", Sum);
            }

            return 0;
        }
    }
}
