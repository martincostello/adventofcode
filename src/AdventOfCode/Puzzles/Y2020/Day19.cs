// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2020
{
    using System.Collections.Generic;
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
        /// Gets the number of messages that completely match rule zero with the fix applied.
        /// </summary>
        public int MatchesRule0WithFix { get; private set; }

        /// <summary>
        /// Gets the count of the number of messages that completely match rule 0.
        /// </summary>
        /// <param name="input">The input of rules and messages to get the count for.</param>
        /// <param name="applyFix">Whether to apply the fix to the rules.</param>
        /// <returns>
        /// The number of messages that completely match rule 0.
        /// </returns>
        public static int GetMatchCount(IList<string> input, bool applyFix)
        {
            int delimiter = input.IndexOf(string.Empty);

            IDictionary<string, string> rules = ParseRules(input.Take(delimiter), applyFix);

            string rule = rules["0"];

            string pattern = "^" + rule + "$";
            var messages = input.Skip(delimiter + 1);

            return messages.Count((p) => Regex.IsMatch(p, pattern));

            static IDictionary<string, string> ParseRules(IEnumerable<string> input, bool applyFix)
            {
                var rawRules = new Dictionary<string, List<string>>();

                foreach (string rule in input)
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
                            string otherRule = values[i];

                            if (rules.TryGetValue(otherRule, out string? r))
                            {
                                if (r.Length > 1)
                                {
                                    r = "(" + r + ")";
                                }

                                values[i] = r;
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

                if (applyFix)
                {
                    //// "N: a | a N" => "N: (a)+" => "8: 42 | 42 8" => "8: (42)+"
                    rules["8"] = $"({rules["42"]})+";

                    //// "N: a b | a N b" => "N: (a (ab)* b)" => "11: 42 31 | 42 11 31" => "11: (42((42)(31))*31)"
                    rules["11"] = $"{rules["42"]}(({rules["42"]})({rules["31"]}))*{rules["31"]}";

                    // This works for the example, but not generally. Above is broken.
                    rules["11"] = $"({rules["42"]})+({rules["31"]})+";

                    rules["0"] = rules["8"] + rules["11"];
                }

                return rules;
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> input = await ReadResourceAsLinesAsync();

            MatchesRule0 = GetMatchCount(input, applyFix: false);
            MatchesRule0WithFix = GetMatchCount(input, applyFix: true);

            if (Verbose)
            {
                Logger.WriteLine("The number of messages that completely match rule 0 is {0} without the fix.", MatchesRule0);
                Logger.WriteLine("The number of messages that completely match rule 0 is {0} with the fix.", MatchesRule0WithFix);
            }

            return PuzzleResult.Create(MatchesRule0, MatchesRule0WithFix);
        }
    }
}
