// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotGate.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   NotGate.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    /// <summary>
    /// A class representing a logical NOT gate. This class cannot be inherited.
    /// </summary>
    internal sealed class NotGate : LogicGate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotGate"/> class.
        /// </summary>
        /// <param name="id">The Id of the logic gate.</param>
        /// <param name="other">The other logic gate that provides the input signal.</param>
        internal NotGate(string id, ILogicGate other)
            : base(id)
        {
            Signal = (ushort)((other.Signal * -1) - 1);
        }
    }
}
