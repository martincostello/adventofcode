// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/13</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day13 : IPuzzle
    {
        /// <summary>
        /// Gets the maximum total change in happiness.
        /// </summary>
        internal int MaximumTotalChangeInHappiness { get; private set; }

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

            string[] potentialHappiness = File.ReadAllLines(args[0]);

            MaximumTotalChangeInHappiness = GetMaximumTotalChangeInHappiness(potentialHappiness);

            Console.WriteLine("The total change in happiness is {0:N0}.", MaximumTotalChangeInHappiness);

            return 0;
        }

        /// <summary>
        /// Gets the maximum total change in happiness for the specified potential happiness of the guests.
        /// </summary>
        /// <param name="potentialHappiness">A collection of potential guess happinesses.</param>
        /// <returns>The optional total change in happiness for the specified potentials.</returns>
        internal static int GetMaximumTotalChangeInHappiness(ICollection<string> potentialHappiness)
        {
            // Parse the input data
            List<Potential> potentials = new List<Potential>();

            foreach (string value in potentialHappiness)
            {
                potentials.Add(ParsePotentialHappiness(value));
            }

            // Determine all of the possible seating arrangements
            List<string> names = potentials
                .Select((p) => p.Name)
                .Distinct()
                .ToList();

            IList<IList<string>> permutations = GetPermutations(names)
                .Select((p) => new List<string>(p) as IList<string>)
                .ToList();

            // Key the happiness for each person for the people they could sit next to
            IDictionary<string, Dictionary<string, int>> happinesses = names.ToDictionary(
                (p) => p,
                (p) => potentials.Where((r) => r.Name == p).ToDictionary((r) => r.AdjacentName, (r) => r.Happiness));

            // Get the maximum potential happiness from all the seating arrangements
            return permutations
                .Select((p) => GetHappiness(p, happinesses))
                .Max();
        }

        /// <summary>
        /// Gets the total happiness for the specified place setting.
        /// </summary>
        /// <param name="setting">The place setting.</param>
        /// <param name="happinesses">The potential happinesses</param>
        /// <returns>The total change in happiness for the given place setting.</returns>
        private static int GetHappiness(IList<string> setting, IDictionary<string, Dictionary<string, int>> happinesses)
        {
            string head = setting.First();
            string tail = setting.Last();

            // Because the table is circular, find the happinesses for the first and last items
            int happiness = happinesses[head][tail];
            happiness += happinesses[tail][head];
            happiness += happinesses[head][setting[1]];
            happiness += happinesses[tail][setting[setting.Count - 2]];

            // Find the happiness for each seat which has a seat on either side
            for (int i = 1; i < setting.Count - 1; i++)
            {
                string current = setting[i];

                happiness += happinesses[current][setting[i - 1]];
                happiness += happinesses[current][setting[i + 1]];
            }

            return happiness;
        }

        /// <summary>
        /// Returns all the permutations of the specified collection of strings.
        /// </summary>
        /// <param name="collection">The collection to get the permutations of.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that returns the permutations of <paramref name="collection"/>.
        /// </returns>
        private static IEnumerable<IEnumerable<string>> GetPermutations(ICollection<string> collection)
        {
            return GetPermutations(collection, collection.Count);
        }

        /// <summary>
        /// Returns all the permutations of the specified collection of strings.
        /// </summary>
        /// <param name="collection">The collection to get the permutations of.</param>
        /// <param name="count">The number of items to calculate the permutations from.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that returns the permutations of <paramref name="collection"/>.
        /// </returns>
        private static IEnumerable<IEnumerable<string>> GetPermutations(IEnumerable<string> collection, int count)
        {
            if (count == 1)
            {
                return collection.Select((p) => new[] { p });
            }

            return GetPermutations(collection, count - 1)
                .SelectMany((p) => collection.Where((r) => !p.Contains(r)), (set, value) => set.Concat(new[] { value }));
        }

        /// <summary>
        /// Parses the potential happiness from the specified value.
        /// </summary>
        /// <param name="value">The value to parse.</param>
        /// <returns>The <see cref=""/> representation of <paramref name="value"/>.</returns>
        private static Potential ParsePotentialHappiness(string value)
        {
            string[] split = value.Split(' ');

            Potential result = new Potential()
            {
                Name = split.First(),
                AdjacentName = split.Last().TrimEnd('.'),
            };

            result.Happiness = int.Parse(split[3], CultureInfo.InvariantCulture);

            if (split[2] == "lose")
            {
                result.Happiness *= -1;
            }

            return result;
        }

        /// <summary>
        /// A class representing a happiness potential for a person sat next to another person. This class cannot be inherited.
        /// </summary>
        private sealed class Potential
        {
            /// <summary>
            /// Gets or sets the name of the person.
            /// </summary>
            internal string Name { get; set; }

            /// <summary>
            /// Gets or sets the name of the adjacent person.
            /// </summary>
            internal string AdjacentName { get; set; }

            /// <summary>
            /// Gets or sets the potential happiness.
            /// </summary>
            internal int Happiness { get; set; }
        }
    }
}
