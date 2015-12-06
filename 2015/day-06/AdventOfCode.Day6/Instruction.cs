// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Instruction.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   Instruction.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace MartinCostello.AdventOfCode.Day6
{
    /// <summary>
    /// A class representing an instruction for manipulating the light grid. This class cannot be inherited.
    /// </summary>
    internal sealed class Instruction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Instruction"/> class.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="bounds">The bounds of the lights to perform the action on.</param>
        private Instruction(string action, Rectangle bounds)
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

        /// <summary>
        /// Parses the specified instruction.
        /// </summary>
        /// <param name="value">A <see cref="string"/> representing the instruction.</param>
        /// <returns>The <see cref="Instruction"/> parsed from <paramref name="value"/>.</returns>
        internal static Instruction Parse(string value)
        {
            // Split the instructions into 'words'
            string[] words = value.Split(' ');

            string action = null;
            Rectangle bounds = default(Rectangle);

            string firstWord = words.ElementAtOrDefault(0);

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

            // Determine the termination and origin points of the bounds of the lights to operate on
            string[] originPoints = origin.Split(',');
            string[] terminationPoints = termination.Split(',');

            int left = int.Parse(originPoints[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int bottom = int.Parse(originPoints[1], NumberStyles.Integer, CultureInfo.InvariantCulture);

            int right = int.Parse(terminationPoints[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int top = int.Parse(terminationPoints[1], NumberStyles.Integer, CultureInfo.InvariantCulture);

            // Add one to the termination point so that the grid always has a width of at least one light
            bounds = Rectangle.FromLTRB(left, bottom, right + 1, top + 1);

            return new Instruction(action, bounds);
        }

        /// <summary>
        /// Performs the instruction on the specified <see cref="LightGrid"/>.
        /// </summary>
        /// <param name="grid">The grid to perform the instruction on.</param>
        /// <exception cref="InvalidOperationException">The current instruction is invalid.</exception>
        internal void Act(LightGrid grid)
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
    }
}
