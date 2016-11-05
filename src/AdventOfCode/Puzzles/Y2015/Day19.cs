// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2015/day/19</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day19 : Puzzle2015
    {
        /// <summary>
        /// Gets the solution for fabrication or calibration.
        /// </summary>
        internal int Solution { get; private set; }

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <summary>
        /// Gets the possible molecules that can be created from single step transformations of a molecule.
        /// </summary>
        /// <param name="molecule">The input molecule.</param>
        /// <param name="replacements">The possible replacements.</param>
        /// <returns>
        /// The distinct molecules that can be created from <paramref name="molecule"/> using all of the
        /// possible replacements specified by <paramref name="replacements"/>.
        /// </returns>
        internal static ICollection<string> GetPossibleMolecules(string molecule, ICollection<string> replacements)
        {
            List<string> molecules = new List<string>();

            foreach (string replacement in replacements)
            {
                string[] split = replacement.Split(new[] { " => " }, StringSplitOptions.None);

                string source = split[0];
                string target = split[1];

                for (int i = 0; i < molecule.Length; i++)
                {
                    int index = molecule.IndexOf(source, i, StringComparison.Ordinal);

                    if (index > -1)
                    {
                        string newMolecule = Format(
                            "{0}{1}{2}",
                            index == 0 ? string.Empty : molecule.Substring(0, index),
                            target,
                            molecule.Substring(index + source.Length));

                        if (!molecules.Contains(newMolecule))
                        {
                            molecules.Add(newMolecule);
                        }
                    }
                }
            }

            return molecules
                .OrderBy((p) => p, StringComparer.Ordinal)
                .ToList();
        }

        /// <summary>
        /// Gets the minimum number of steps that can be performed to make the specified molecule using the specified replacements.
        /// </summary>
        /// <param name="molecule">The desired molecule.</param>
        /// <param name="replacements">The possible replacements.</param>
        /// <returns>
        /// The minimum number of steps required to create <paramref name="molecule"/> using the possible
        /// replacements specified by <paramref name="replacements"/>.
        /// </returns>
        internal static int GetMinimumSteps(string molecule, ICollection<string> replacements)
        {
            return GetMinimumSteps(molecule, replacements, "e", 1);
        }

        /// <summary>
        /// Gets the minimum number of steps that can be performed to make the specified molecule using the specified replacements.
        /// </summary>
        /// <param name="molecule">The desired molecule.</param>
        /// <param name="replacements">The possible replacements.</param>
        /// <param name="current">The current molecule being worked with.</param>
        /// <param name="step">The current step number.</param>
        /// <returns>
        /// The minimum number of steps required to create <paramref name="molecule"/> using the possible
        /// replacements specified by <paramref name="replacements"/>.
        /// </returns>
        internal static int GetMinimumSteps(string molecule, ICollection<string> replacements, string current, int step)
        {
            ICollection<string> nextSteps = GetPossibleMolecules(current, replacements);

            if (nextSteps.Contains(molecule))
            {
                Console.WriteLine("Found solution that takes {0:N0} steps.", step);
                return step;
            }

            List<int> steps = new List<int>();

            foreach (string next in nextSteps.Where((p) => p.Length < molecule.Length))
            {
                steps.Add(GetMinimumSteps(molecule, replacements, next, step + 1));
            }

            return steps
                .Where((p) => p > 0)
                .DefaultIfEmpty()
                .Min();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            bool fabricate;

            switch (args[0].ToUpperInvariant())
            {
                case "CALIBRATE":
                    fabricate = false;
                    break;

                case "FABRICATE":
                    fabricate = true;
                    break;

                default:
                    Console.WriteLine("The mode specified is invalid.");
                    return -1;
            }

            IList<string> lines = ReadResourceAsLines();

            string molecule = lines.Last();

            ICollection<string> replacements = lines
                .Take(lines.Count - 1)
                .Where((p) => !string.IsNullOrEmpty(p))
                .ToList();

            if (fabricate)
            {
                Solution = GetMinimumSteps(molecule, replacements);
                Console.WriteLine("The target molecule can be made in a minimum of {0:N0} steps.", Solution);
            }
            else
            {
                var molecules = GetPossibleMolecules(molecule, replacements);

                Solution = molecules.Count;

                Console.WriteLine(
                    "{0:N0} distinct molecules can be created from {1:N0} possible replacements.",
                    Solution,
                    replacements.Count);
            }

            return 0;
        }
    }
}
