// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2019
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2019/day/1</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day01 : Puzzle2019
    {
        /// <summary>
        /// Gets the total fuel requirement.
        /// </summary>
        public double TotalFuelRequired { get; private set; }

        /// <summary>
        /// Gets the fuel requirements for the specified mass.
        /// </summary>
        /// <param name="mass">The mass to get the fuel requirements for.</param>
        /// <returns>
        /// The fuel requirements for the mass specified by <paramref name="mass"/>.
        /// </returns>
        public static double GetFuelRequirementsForModule(string mass)
        {
            int massValue = ParseInt32(mass);

            double requirement = massValue / 3.0;

            requirement = Math.Floor(requirement);

            requirement -= 2;

            return requirement;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> masses = ReadResourceAsLines();

            double total = 0;

            foreach (string mass in masses)
            {
                total += GetFuelRequirementsForModule(mass);
            }

            TotalFuelRequired = total;

            if (Verbose)
            {
                Logger.WriteLine("{0} fuel is required for the modules.", TotalFuelRequired);
            }

            return 0;
        }
    }
}
