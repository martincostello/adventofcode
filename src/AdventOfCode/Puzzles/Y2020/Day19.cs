// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2020/day/19</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2020, 19, RequiresData = true)]
    public sealed class Day19 : Puzzle
    {
        /// <summary>
        /// Gets the number of messages that completely match rule zero.
        /// </summary>
        public int MatchesRule0 { get; private set; }

        /// <summary>
        /// Gets the count of the number of messages that completely match the specified rule.
        /// </summary>
        /// <param name="input">The input of rules and messages to get the count for.</param>
        /// <param name="ruleIndex">The index of the rule to count the number of messages that match.</param>
        /// <returns>
        /// The number of messages that completely match the rule specified by <paramref name="ruleIndex"/>.
        /// </returns>
        public static int GetMatchCount(IList<string> input, int ruleIndex)
        {
            int delimiter = input.IndexOf(string.Empty);

            IDictionary<string, string> rules = ParseRules(input.Take(delimiter));

            string rule = rules[ruleIndex.ToString(CultureInfo.InvariantCulture)];

            string pattern = "^" + rule + "$";
            var messages = input.Skip(delimiter + 1);

            return messages.Count((p) => Regex.IsMatch(p, pattern));

            static IDictionary<string, string> ParseRules(IEnumerable<string> foo)
            {
                var rawRules = new Dictionary<string, List<string>>();

                foreach (string rule in foo)
                {
                    string[] split = rule.Split(':');

                    string key = split[0];
                    var value = split[1]
                        .TrimStart()
                        .Trim('"')
                        .Split(' ')
                        .ToList();

                    rawRules[key] = value;
                }

                string ruleA = rawRules.First((p) => p.Value.Count == 1 && p.Value[0] == "a").Key;
                string ruleB = rawRules.First((p) => p.Value.Count == 1 && p.Value[0] == "b").Key;

                rawRules.Remove(ruleA);
                rawRules.Remove(ruleB);

                var rules = new Dictionary<string, string>(rawRules.Count)
                {
                    [ruleA] = "a",
                    [ruleB] = "b",
                };

                while (rawRules.Count > 0)
                {
                    foreach (string rule in rawRules.Keys.ToArray())
                    {
                        List<string> values = rawRules[rule];

                        for (int i = 0; i < values.Count; i++)
                        {
                            // Replace any known rule keys with the pattern for that rule
                            if (rules.TryGetValue(values[i], out string? r))
                            {
                                values[i] = "(" + r + ")";
                            }
                        }

                        // When the pattern is fully reduced then construct Regex from its parts remove it
                        if (!values.Any((p) => p.Any(char.IsDigit)))
                        {
                            rules[rule] = string.Join(string.Empty, values);
                            rawRules.Remove(rule);
                        }
                    }
                }

                return rules;
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> input = await ReadResourceAsLinesAsync();

            int rule = 0;

            MatchesRule0 = GetMatchCount(input, rule);

            if (Verbose)
            {
                Logger.WriteLine("The number of messages that completely match rule {0} is {1}.", rule, MatchesRule0);
            }

            return PuzzleResult.Create(MatchesRule0);
        }
    }
}
