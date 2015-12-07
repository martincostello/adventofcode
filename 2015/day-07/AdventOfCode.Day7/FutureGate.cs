// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FutureGate.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   FutureGate.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day7
{
    using System;

    /// <summary>
    /// A class representing a logic gate that will yield a value in the future. This class cannot be inherited.
    /// </summary>
    internal sealed class FutureGate : ILogicGate
    {
        /// <summary>
        /// The just-in-time (future) value of the signal. This field is read-only.
        /// </summary>
        private readonly Lazy<ushort> _signal;

        /// <summary>
        /// Initializes a new instance of the <see cref="FutureGate"/> class.
        /// </summary>
        /// <param name="id">The Id of the logic gate.</param>
        /// <param name="signal">A delegate to a method that will provide the signal for the gate.</param>
        internal FutureGate(string id, Func<ushort> signal)
        {
            Id = id;
            _signal = new Lazy<ushort>(signal);
        }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public ushort Signal => _signal.Value;
    }
}
