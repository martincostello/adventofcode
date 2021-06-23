// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MartinCostello.AdventOfCode.Puzzles.Y2016
{
    /// <summary>
    /// A class representing the puzzle for <c>https://adventofcode.com/2016/day/15</c>. This class cannot be inherited.
    /// </summary>
    [Puzzle(2016, 15, RequiresData = true)]
    public sealed class Day15 : Puzzle
    {
        /// <summary>
        /// The delimiters used when parsing the input. This field is read-only.
        /// </summary>
        private static readonly char[] Separators = new[] { ' ', ',', '.' };

        /// <summary>
        /// Gets the value of T where the button can first be pressed to get a capsule.
        /// </summary>
        public int TimeOfFirstButtonPress { get; private set; }

        /// <summary>
        /// Gets the value of T where the button can first be pressed to get a capsule
        /// when an additional disc with 11 positions is present at the bottom of the sculpture.
        /// </summary>
        public int TimeOfFirstButtonPressWithExtraDisc { get; private set; }

        /// <summary>
        /// Finds the value of T when the discs described by the specified input are
        /// first aligned in such a manner as a capsule can be retrieved from the sculpture.
        /// </summary>
        /// <param name="input">One or more strings describing the discs in the sculpture.</param>
        /// <returns>
        /// The value of T when a capsule can first be retrieved using the specified discs.
        /// </returns>
        internal static int FindTimeForCapsuleRelease(IEnumerable<string> input)
        {
            IList<Disc> discs = input.Select(Disc.Parse).ToList();

            for (int t = 0; ; t++)
            {
                bool areAllDiscsAligned = discs.All((p) => p.GetPosition(t + p.Offset) == 0);

                if (areAllDiscsAligned)
                {
                    return t;
                }
            }
        }

        /// <inheritdoc />
        protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
        {
            IList<string> input = await ReadResourceAsLinesAsync();

            string extraDisc = $"Disc #{input.Count + 1} has 11 positions; at time=0, it is at position 0.";

            TimeOfFirstButtonPress = FindTimeForCapsuleRelease(input);
            TimeOfFirstButtonPressWithExtraDisc = FindTimeForCapsuleRelease(input.Append(extraDisc));

            if (Verbose)
            {
                Logger.WriteLine($"The first time the button can be pressed to get a capsule is {TimeOfFirstButtonPress:N0}.");
                Logger.WriteLine($"The first time the button can be pressed to get a capsule with the extra disc present is {TimeOfFirstButtonPressWithExtraDisc:N0}.");
            }

            return PuzzleResult.Create(TimeOfFirstButtonPress, TimeOfFirstButtonPressWithExtraDisc);
        }

        /// <summary>
        /// A class representing a disc in the sculpture. This class cannot be inherited.
        /// </summary>
        private sealed class Disc
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Disc"/> class.
            /// </summary>
            /// <param name="positions">The number of positions of the disc.</param>
            /// <param name="initialPosition">The initial position of the disc.</param>
            /// <param name="offset">The offset of the disc from the top of the sculpture.</param>
            public Disc(int positions, int initialPosition, int offset)
            {
                Current = initialPosition;
                Positions = positions;
                Offset = offset;
            }

            /// <summary>
            /// Gets the current position of the disc.
            /// </summary>
            public int Current { get; private set; }

            /// <summary>
            /// Gets the number of positions the disc has.
            /// </summary>
            public int Positions { get; }

            /// <summary>
            /// Gets the offset of the disc from the top of the sculpture.
            /// </summary>
            public int Offset { get; }

            /// <summary>
            /// Parses information about a disc from the specified <see cref="string"/>.
            /// </summary>
            /// <param name="value">The string to parse as a disc.</param>
            /// <returns>
            /// An instance of <see cref="Disc"/> created from <paramref name="value"/>.
            /// </returns>
            public static Disc Parse(string value)
            {
                string[] split = value.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

                int offset = ParseInt32(split[1].TrimStart('#'));
                int positions = ParseInt32(split[3]);
                int initialPosition = ParseInt32(split[11]);

                return new Disc(positions, initialPosition, offset);
            }

            /// <summary>
            /// Gets the position of the disc in the specified number of moves.
            /// </summary>
            /// <param name="t">The number of moves in the future for which to get the position.</param>
            /// <returns>
            /// The new position of the disc.
            /// </returns>
            public int GetPosition(int t)
            {
                int position = Current;

                int steps = t % Positions;

                for (int i = 0; i < steps; i++)
                {
                    if (position == Positions - 1)
                    {
                        position = 0;
                    }
                    else
                    {
                        position++;
                    }
                }

                return position;
            }
        }
    }
}
