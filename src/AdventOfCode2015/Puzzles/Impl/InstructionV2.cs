// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles.Impl
{
    using System;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// A class representing a version 2 instruction for manipulating the light grid. This class cannot be inherited.
    /// </summary>
    internal sealed class InstructionV2 : Instruction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionV2"/> class.
        /// </summary>
        /// <param name="delta">The delta to apply to the brightness of the light.</param>
        /// <param name="bounds">The bounds of the lights to perform the action on.</param>
        private InstructionV2(int delta, Rectangle bounds)
        {
            Delta = delta;
            Bounds = bounds;
        }

        /// <summary>
        /// Gets the bounds of the lights to perform the action on.
        /// </summary>
        internal Rectangle Bounds { get; }

        /// <summary>
        /// Gets the delta to apply to the brightness of the light.
        /// </summary>
        internal int Delta { get; }

        /// <inheritdoc />
        public override void Act(LightGrid grid)
        {
            grid.IncrementBrightness(Bounds, Delta);
        }

        /// <summary>
        /// Parses the specified instruction.
        /// </summary>
        /// <param name="value">A <see cref="string"/> representing the instruction.</param>
        /// <returns>The <see cref="InstructionV2"/> parsed from <paramref name="value"/>.</returns>
        internal static InstructionV2 Parse(string value)
        {
            // Split the instructions into 'words'
            string[] words = value.Split(' ');

            string firstWord = words.ElementAtOrDefault(0);

            int? delta = null;
            string origin = null;
            string termination = null;

            // Determine the action to perform for this instruction (OFF, ON or TOGGLE)
            if (string.Equals(firstWord, "turn", StringComparison.OrdinalIgnoreCase))
            {
                string secondWord = words.ElementAtOrDefault(1);

                if (string.Equals(secondWord, "off", StringComparison.OrdinalIgnoreCase))
                {
                    delta = -1;
                }
                else if (string.Equals(secondWord, "on", StringComparison.OrdinalIgnoreCase))
                {
                    delta = 1;
                }

                origin = words.ElementAtOrDefault(2);
                termination = words.ElementAtOrDefault(4);
            }
            else if (string.Equals(firstWord, "toggle", StringComparison.OrdinalIgnoreCase))
            {
                delta = 2;
                origin = words.ElementAtOrDefault(1);
                termination = words.ElementAtOrDefault(3);
            }

            if (delta == null || origin == null || termination == null)
            {
                throw new ArgumentException("The specified instruction is invalid.", nameof(value));
            }

            Rectangle bounds = ParseBounds(origin, termination);

            return new InstructionV2(delta.Value, bounds);
        }
    }
}
