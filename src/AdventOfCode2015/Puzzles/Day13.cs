// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/13</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day13 : Puzzle
    {
        /// <summary>
        /// Gets the maximum total change in happiness.
        /// </summary>
        internal int MaximumTotalChangeInHappiness { get; private set; }

        /// <summary>
        /// Gets the maximum total change in happiness with the current user also seated.
        /// </summary>
        internal int MaximumTotalChangeInHappinessWithCurrentUser { get; private set; }

        /// <inheritdoc />
        protected override bool IsFirstArgumentFilePath => true;

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Gets the maximum total change in happiness for the specified potential happiness of the guests.
        /// </summary>
        /// <param name="potentialHappiness">A collection of potential guess happinesses.</param>
        /// <returns>The optional total change in happiness for the specified potentials.</returns>
        internal static int GetMaximumTotalChangeInHappiness(ICollection<string> potentialHappiness)
        {
            // Parse the input data
            IList<Potential> potentials = potentialHappiness
                .Select(ParsePotentialHappiness)
                .ToList();

            // Determine all of the possible seating arrangements
            List<string> names = potentials
                .Select((p) => p.Name)
                .Distinct()
                .ToList();

            // TODO Remove all permutations which are equal when considering the table is circular
            IList<IList<string>> permutations = Maths.GetPermutations(names)
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

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string[] potentialHappiness = File.ReadAllLines(args[0]);

            MaximumTotalChangeInHappiness = GetMaximumTotalChangeInHappiness(potentialHappiness);

            Console.WriteLine("The total change in happiness is {0:N0}.", MaximumTotalChangeInHappiness);

            // Create a new guest list which is the same as the previous one but with the current user added
            var potentialHappinessWithCurrentUser = new List<string>(potentialHappiness);

            var existingGuestNames = potentialHappiness
                .Select((p) => p.Split(' ').First())
                .Distinct()
                .ToList();

            // Add the current user to the guest list where everyone is neutral to them
            string currentUser = Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName);

            const string AmbivalentFormat = "{0} would gain 0 happiness units by sitting next to {1}.";

            foreach (string guest in existingGuestNames)
            {
                potentialHappinessWithCurrentUser.Add(Format(AmbivalentFormat, guest, currentUser));
                potentialHappinessWithCurrentUser.Add(Format(AmbivalentFormat, currentUser, guest));
            }

            MaximumTotalChangeInHappinessWithCurrentUser = GetMaximumTotalChangeInHappiness(potentialHappinessWithCurrentUser);

            Console.WriteLine("The total change in happiness with the current user seated is {0:N0}.", MaximumTotalChangeInHappinessWithCurrentUser);

            return 0;
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

            result.Happiness = ParseInt32(split[3]);

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
