// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogicGate.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   ILogicGate.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    /// <summary>
    /// Defines a logic gate.
    /// </summary>
    internal interface ILogicGate
    {
        /// <summary>
        /// Gets the Id of the logic gate.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the current signal of the logic gate.
        /// </summary>
        ushort Signal { get; }
    }
}
