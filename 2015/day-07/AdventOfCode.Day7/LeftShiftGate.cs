// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LeftShiftGate.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   LeftShiftGate.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    /// <summary>
    /// A class representing a left-shift logic gate. This class cannot be inherited.
    /// </summary>
    internal sealed class LeftShiftGate : LogicGate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeftShiftGate"/> class.
        /// </summary>
        /// <param name="id">The Id of the logic gate.</param>
        /// <param name="other">The other logic gate to get the input signal from.</param>
        /// <param name="shift">The number of bits to shift left by.</param>
        internal LeftShiftGate(string id, ILogicGate other, ushort shift)
            : base(id)
        {
            Signal = (ushort)(other.Signal << shift);
        }
    }
}
