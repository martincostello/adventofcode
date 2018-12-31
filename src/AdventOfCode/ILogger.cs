// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// Defines a logger.
    /// </summary>
    internal interface ILogger
    {
        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="format">The format string to use to generate the message.</param>
        /// <param name="args">The arguments for the format string.</param>
        void WriteLine(string format, params object[] args);
    }
}
