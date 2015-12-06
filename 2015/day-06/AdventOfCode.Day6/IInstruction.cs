// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInstruction.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   IInstruction.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day6
{
    /// <summary>
    /// Defines an instruction for performing an action on a <see cref="LightGrid"/>.
    /// </summary>
    internal interface IInstruction
    {
        /// <summary>
        /// Performs the instruction on the specified <see cref="LightGrid"/>.
        /// </summary>
        /// <param name="grid">The grid to perform the instruction on.</param>
        void Act(LightGrid grid);
    }
}
