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
    /// A class representing the puzzle for <c>http://adventofcode.com/day/19</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day19 : IPuzzle
    {
        /// <summary>
        /// Gets the number of distinct molecules that can be created.
        /// </summary>
        internal int DistinctMoleculeCount { get; private set; }

        /// <inheritdoc />
        public int Solve(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("No input file path.");
                return -1;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            string[] lines = File.ReadAllLines(args[0]);

            string molecule = lines.Last();

            ICollection<string> replacements = lines
                .Take(lines.Length - 1)
                .Where((p) => !string.IsNullOrEmpty(p))
                .ToList();

            var molecules = GetPossibleMolecules(molecule, replacements);

            DistinctMoleculeCount = molecules.Count;

            Console.WriteLine(
                "{0:N0} distinct molecules can be created from {1:N0} possible replacements.",
                DistinctMoleculeCount,
                replacements.Count);

            return 0;
        }

        /// <summary>
        /// Gets the possible molecules that can be created from single step transformations of a molecule.
        /// </summary>
        /// <param name="molecule">The input molecule.</param>
        /// <param name="replacements">The possible replacemnts.</param>
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
                        string newMolecule = string.Format(
                            CultureInfo.InvariantCulture,
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
    }
}
