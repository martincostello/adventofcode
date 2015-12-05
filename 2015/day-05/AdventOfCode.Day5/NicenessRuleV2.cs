// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NicenessRuleV2.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   NicenessRuleV2.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day5
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class defining version 2 of the niceness rule. This class cannot be inherited.
    /// </summary>
    internal sealed class NicenessRuleV2 : INicenessRule
    {
        /// <inheritdoc />
        public bool IsNice(string value)
        {
            return HasPairOfLettersWithMoreThanOneOccurence(value) && HasLetterThatIsTheBreadOfALetterSandwich(value);
        }

        /// <summary>
        /// Tests whether a string contains a pair of any two letters that appear at least twice in the string without overlapping.
        /// </summary>
        /// <param name="value">The value to test against the rule.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.</returns>
        internal static bool HasPairOfLettersWithMoreThanOneOccurence(string value)
        {
            Dictionary<string, IList<int>> letterPairs = new Dictionary<string, IList<int>>();

            for (int i = 0; i < value.Length - 1; i++)
            {
                char first = value[i];
                char second = value[i + 1];

                string pair = new string(new[] { first, second });

                IList<int> indexes;

                if (!letterPairs.TryGetValue(pair, out indexes))
                {
                    indexes = letterPairs[pair] = new List<int>();
                }

                if (!indexes.Contains(i - 1))
                {
                    indexes.Add(i);
                }
            }

            return letterPairs.Any((p) => p.Value.Count > 1);
        }

        /// <summary>
        /// Tests whether a string contains at least one letter which repeats with exactly one letter between them.
        /// </summary>
        /// <param name="value">The value to test against the rule.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> meets the rule; otherwise <see langword="false"/>.</returns>
        internal static bool HasLetterThatIsTheBreadOfALetterSandwich(string value)
        {
            if (value.Length < 3)
            {
                // The value is not long enough
                return false;
            }

            for (int i = 1; i < value.Length - 1; i++)
            {
                if (value[i - 1] == value[i + 1])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
