// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogicGate.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   LogicGate.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    /// <summary>
    /// The base class for logic gates.
    /// </summary>
    internal abstract class LogicGate : ILogicGate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicGate"/> class.
        /// </summary>
        /// <param name="id">The Id of the logic gate.</param>
        protected LogicGate(string id)
        {
            Id = id;
        }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public ushort Signal { get; protected set; }
    }
}
