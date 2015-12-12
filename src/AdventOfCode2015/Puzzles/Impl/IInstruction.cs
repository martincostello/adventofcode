// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles.Impl
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
