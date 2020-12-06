// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2017/day/7</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2017, 07, RequiresData = true)]
    public sealed class Day07 : Puzzle
    {
        /// <summary>
        /// Gets the name of the bottom program.
        /// </summary>
        public string? BottomProgramName { get; private set; }

        /// <summary>
        /// Gets the weight that the disc that unbalances the structure should be to balance it.
        /// </summary>
        public int DesiredWeightOfUnbalancedDisc { get; private set; }

        /// <summary>
        /// Finds the name of the program at the bottom of the specified structure.
        /// </summary>
        /// <param name="structure">A collection of strings representing the structure of the tower.</param>
        /// <returns>
        /// The name of the program at the bottom of the tower described by <paramref name="structure"/>.
        /// </returns>
        public static string FindBottomProgramName(IEnumerable<string> structure)
        {
            IDictionary<string, ProgramDisc> tower = BuildStructure(structure);
            return FindBottomProgramName(tower.Values);
        }

        /// <summary>
        /// Finds the weight that the program that unbalances the structure should be to balance it.
        /// </summary>
        /// <param name="structure">A collection of strings representing the structure of the tower.</param>
        /// <returns>
        /// The weight the program that unbalances the tower described by <paramref name="structure"/> should be.
        /// </returns>
        public static int FindDesiredWeightOfUnbalancedDisc(IEnumerable<string> structure)
        {
            IDictionary<string, ProgramDisc> tower = BuildStructure(structure);
            string bottomName = FindBottomProgramName(tower.Values);

            ProgramDisc bottom = tower[bottomName];

            return FindDesiredWeightOfUnbalancedDisc(bottom);
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> structure = await ReadResourceAsLinesAsync();

            BottomProgramName = FindBottomProgramName(structure);
            DesiredWeightOfUnbalancedDisc = FindDesiredWeightOfUnbalancedDisc(structure);

            if (Verbose)
            {
                Logger.WriteLine($"The name of the bottom program is '{BottomProgramName}'.");
                Logger.WriteLine($"The desired weight of the program to balance the structure is {DesiredWeightOfUnbalancedDisc:N0}.");
            }

            return PuzzleResult.Create(BottomProgramName, DesiredWeightOfUnbalancedDisc);
        }

        /// <summary>
        /// Builds the structure of discs.
        /// </summary>
        /// <param name="structure">A collection of strings representing the structure of the tower.</param>
        /// <returns>
        /// An <see cref="IDictionary{TKey, TValue}"/> containing the programs with references to their parent and children, keyed by their names.
        /// </returns>
        private static IDictionary<string, ProgramDisc> BuildStructure(IEnumerable<string> structure)
        {
            var programs = structure
                .Select((p) => new ProgramDisc(p))
                .ToDictionary((p) => p.Name, (p) => p);

            foreach (ProgramDisc program in programs.Values)
            {
                foreach (string name in program.ProgramsHeld)
                {
                    ProgramDisc child = programs[name];

                    child.Parent = program;
                    program.Children.Add(child);
                }
            }

            return programs;
        }

        /// <summary>
        /// Finds the name of the program at the bottom of the specified tower.
        /// </summary>
        /// <param name="tower">A collection of discs representing the structure of the tower.</param>
        /// <returns>
        /// The name of the program at the bottom of the tower described by <paramref name="tower"/>.
        /// </returns>
        private static string FindBottomProgramName(ICollection<ProgramDisc> tower)
        {
            return tower
                .Where((p) => p.Parent == null)
                .Select((p) => p.Name)
                .Single();
        }

        /// <summary>
        /// Finds the weight that the program that unbalances the structure should be to balance it, if found.
        /// </summary>
        /// <param name="root">The root node of the structure.</param>
        /// <returns>
        /// The weight the program that unbalances the tower described by <paramref name="root"/> should be, if any; otherwise zero.
        /// </returns>
        private static int FindDesiredWeightOfUnbalancedDisc(ProgramDisc root)
        {
            if (root.ProgramsHeld.Count == 0)
            {
                // Leaf node, no children to inspect
                return 0;
            }

            var unbalancedSubtree = root.Children
                .GroupBy((p) => FindDesiredWeightOfUnbalancedDisc(p))
                .Where((p) => p.Key != 0)
                .SingleOrDefault();

            if (unbalancedSubtree != null)
            {
                // The unbalanced program was found in the subtree
                return unbalancedSubtree.Key;
            }

            var childWeights = root.Children.GroupBy((p) => p.TotalWeight);
            ProgramDisc? unbalanced = childWeights.FirstOrDefault((p) => p.Count() == 1)?.FirstOrDefault();

            if (unbalanced == null)
            {
                // All children's trees weigh the same
                return 0;
            }

            int currentWeight = unbalanced.TotalWeight;
            int targetWeight = childWeights.First((p) => p.Count() != 1).Key;

            int delta = currentWeight - targetWeight;

            return unbalanced.Weight - delta;
        }

        /// <summary>
        /// A class representing a program's disk.
        /// </summary>
        private sealed class ProgramDisc
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ProgramDisc"/> class.
            /// </summary>
            /// <param name="description">The raw description of the program.</param>
            internal ProgramDisc(string description)
            {
                string[] split = description.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                Name = split[0];
                Weight = ParseInt32(split[1].Trim('(', ')'));

                ProgramsHeld =
                    split.Length > 2 ?
                    split.Skip(3).Select((p) => p.Trim(',')).ToArray() :
                    Array.Empty<string>();
            }

            /// <summary>
            /// Gets the name of the program.
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Gets the weight of the disc.
            /// </summary>
            public int Weight { get; }

            /// <summary>
            /// Gets the names of the programs held by this program.
            /// </summary>
            public ICollection<string> ProgramsHeld { get; }

            /// <summary>
            /// Gets or sets the parent of the program.
            /// </summary>
            public ProgramDisc? Parent { get; set; }

            /// <summary>
            /// Gets the child programs of the program.
            /// </summary>
            public ICollection<ProgramDisc> Children { get; } = new List<ProgramDisc>();

            /// <summary>
            /// Gets the total weight of this program and all its children.
            /// </summary>
            public int TotalWeight => Weight + Children.Sum((p) => p.TotalWeight);
        }
    }
}
