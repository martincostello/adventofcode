// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RightShiftGate.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   RightShiftGate.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    /// <summary>
    /// A class representing a right-shift logic gate. This class cannot be inherited.
    /// </summary>
    internal sealed class RightShiftGate : LogicGate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RightShiftGate"/> class.
        /// </summary>
        /// <param name="id">The Id of the logic gate.</param>
        /// <param name="other">The other logic gate to get the input signal from.</param>
        /// <param name="shift">The number of bits to shift right by.</param>
        internal RightShiftGate(string id, ILogicGate other, ushort shift)
            : base(id)
        {
            Signal = (ushort)(other.Signal >> shift);
        }
    }
}
