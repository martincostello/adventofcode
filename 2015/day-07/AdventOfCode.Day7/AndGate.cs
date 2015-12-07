// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AndGate.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   AndGate.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    /// <summary>
    /// A class representing a logical AND gate. This class cannot be inherited.
    /// </summary>
    internal sealed class AndGate : LogicGate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndGate"/> class.
        /// </summary>
        /// <param name="id">The Id of the logic gate.</param>
        /// <param name="first">The other logic gate that provides the first input signal.</param>
        /// <param name="second">The other logic gate that provides the second input signal.</param>
        internal AndGate(string id, ILogicGate first, ILogicGate second)
            : base(id)
        {
            Signal = (ushort)(first.Signal & second.Signal);
        }
    }
}
