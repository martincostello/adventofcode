// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstructionV1.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   InstructionV1.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day6
{
    using System;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// A class representing a version 1 instruction for manipulating the light grid. This class cannot be inherited.
    /// </summary>
    internal sealed class InstructionV1 : Instruction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionV1"/> class.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="bounds">The bounds of the lights to perform the action on.</param>
        private InstructionV1(string action, Rectangle bounds)
        {
            Action = action;
            Bounds = bounds;
        }

        /// <summary>
        /// Gets the action to perform.
        /// </summary>
        internal string Action { get; }

        /// <summary>
        /// Gets the bounds of the lights to perform the action on.
        /// </summary>
        internal Rectangle Bounds { get; }

        /// <inheritdoc />
        public override void Act(LightGrid grid)
        {
            switch (Action)
            {
                case "OFF":
                    grid.TurnOff(Bounds);
                    break;

                case "ON":
                    grid.TurnOn(Bounds);
                    break;

                case "TOGGLE":
                    grid.Toggle(Bounds);
                    break;

                default:
                    throw new InvalidOperationException("The current instruction is invalid.");
            }
        }

        /// <summary>
        /// Parses the specified instruction.
        /// </summary>
        /// <param name="value">A <see cref="string"/> representing the instruction.</param>
        /// <returns>The <see cref="InstructionV1"/> parsed from <paramref name="value"/>.</returns>
        internal static InstructionV1 Parse(string value)
        {
            // Split the instructions into 'words'
            string[] words = value.Split(' ');

            string firstWord = words.ElementAtOrDefault(0);

            string action = null;
            string origin = null;
            string termination = null;

            // Determine the action to perform for this instruction (OFF, ON or TOGGLE)
            if (string.Equals(firstWord, "turn", StringComparison.OrdinalIgnoreCase))
            {
                string secondWord = words.ElementAtOrDefault(1);

                if (string.Equals(secondWord, "off", StringComparison.OrdinalIgnoreCase))
                {
                    action = "OFF";
                }
                else if (string.Equals(secondWord, "on", StringComparison.OrdinalIgnoreCase))
                {
                    action = "ON";
                }

                origin = words.ElementAtOrDefault(2);
                termination = words.ElementAtOrDefault(4);
            }
            else if (string.Equals(firstWord, "toggle", StringComparison.OrdinalIgnoreCase))
            {
                action = "TOGGLE";
                origin = words.ElementAtOrDefault(1);
                termination = words.ElementAtOrDefault(3);
            }

            if (action == null || origin == null || termination == null)
            {
                throw new ArgumentException("The specified instruction is invalid.", nameof(value));
            }

            Rectangle bounds = ParseBounds(origin, termination);

            return new InstructionV1(action, bounds);
        }
    }
}
