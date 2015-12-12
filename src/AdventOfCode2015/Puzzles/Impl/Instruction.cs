// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles.Impl
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

        /// <summary>
        /// Parses the specified origin and termination points, as strings, to a bounding rectangle.
        /// </summary>
        /// <param name="origin">The origin point of the bounding rectangle, as a <see cref="string"/>.</param>
        /// <param name="termination">The termination point of the bounding rectangle, as a <see cref="string"/>.</param>
        /// <returns>A bounding <see cref="Rectangle"/> parsed from <paramref name="origin"/> and <paramref name="termination"/>.</returns>
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
