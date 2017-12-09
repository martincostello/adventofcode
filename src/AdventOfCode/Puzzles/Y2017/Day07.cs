// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2017/day/7</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day07 : Puzzle2017
    {
        /// <summary>
        /// Gets the name of the bottom program.
        /// </summary>
        public string BottomProgramName { get; private set; }

        /// <summary>
        /// Finds the name of the program at the bottom of the specified structure.
        /// </summary>
        /// <param name="structure">A collection of strings representing the structure of the tower.</param>
        /// <returns>
        /// The name of the program at the bottom of the tower described by <paramref name="structure"/>.
        /// </returns>
        public static string FindBottomProgramName(ICollection<string> structure)
        {
            var programs = structure
                .Select((p) => new ProgramDisc(p))
                .ToDictionary((p) => p.Name, (p) => p);

            foreach (var program in programs.Values)
            {
                foreach (string name in program.ProgramsHeld)
                {
                    var child = programs[name];

                    child.Parent = program;
                    program.Children.Add(child);
                }
            }

            return programs.Values
                .Where((p) => p.Parent == null)
                .Select((p) => p.Name)
                .Single();
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            IList<string> structure = ReadResourceAsLines();

            BottomProgramName = FindBottomProgramName(structure);

            Console.WriteLine($"The name of the bottom program is '{BottomProgramName}'.");

            return 0;
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
                string[] split = description.Split(Arrays.Space, StringSplitOptions.RemoveEmptyEntries);

                Name = split[0];
                Weight = ParseInt32(split[1].Trim('(', ')'));

                if (split.Length > 2)
                {
                    ProgramsHeld = split.Skip(3).Select((p) => p.Trim(',')).ToArray();
                }
                else
                {
                    ProgramsHeld = Array.Empty<string>();
                }
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
            public ProgramDisc Parent { get; set; }

            /// <summary>
            /// Gets the child programs of the program.
            /// </summary>
            public ICollection<ProgramDisc> Children { get; } = new List<ProgramDisc>();
        }
    }
}
