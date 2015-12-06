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

namespace MartinCostello.AdventOfCode.Day6
{
    using System.Drawing;
    using System.Globalization;

    /// <summary>
    /// The base class for instructions.
    /// </summary>
    internal abstract class Instruction : IInstruction
    {
        /// <inheritdoc />
        public abstract void Act(LightGrid grid);

        protected static Rectangle ParseBounds(string origin, string termination)
        {
            // Determine the termination and origin points of the bounds of the lights to operate on
            string[] originPoints = origin.Split(',');
            string[] terminationPoints = termination.Split(',');

            int left = int.Parse(originPoints[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int bottom = int.Parse(originPoints[1], NumberStyles.Integer, CultureInfo.InvariantCulture);

            int right = int.Parse(terminationPoints[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
            int top = int.Parse(terminationPoints[1], NumberStyles.Integer, CultureInfo.InvariantCulture);

            // Add one to the termination point so that the grid always has a width of at least one light
            return Rectangle.FromLTRB(left, bottom, right + 1, top + 1);
        }
    }
}
