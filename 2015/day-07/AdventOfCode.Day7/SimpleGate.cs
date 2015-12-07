// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleGate.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   SimpleGate.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    /// <summary>
    /// A class representing a simple logic gate that just has a raw signal. This class cannot be inherited.
    /// </summary>
    internal sealed class SimpleGate : LogicGate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleGate"/> class.
        /// </summary>
        /// <param name="id">The Id of the logic gate.</param>
        /// <param name="signal">The gate's signal.</param>
        internal SimpleGate(string id, ushort signal)
            : base(id)
        {
            Signal = signal;
        }
    }
}
