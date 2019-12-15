// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// Defines a logger.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes a grid to the log.
        /// </summary>
        /// <param name="array">The array to write to the log.</param>
        /// <param name="falseChar">The character to display for <see langword="false"/>.</param>
        /// <param name="trueChar">The character to display for <see langword="true"/>.</param>
        void WriteGrid(bool[,] array, char falseChar, char trueChar);

        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="format">The format string to use to generate the message.</param>
        /// <param name="args">The arguments for the format string.</param>
        void WriteLine(string format, params object[] args);
    }
}
