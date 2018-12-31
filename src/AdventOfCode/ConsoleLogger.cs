// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;

    /// <summary>
    /// A class representing an <see cref="ILogger"/> implementation for the console. This class cannot be inherited.
    /// </summary>
    internal sealed class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="format">The format string to use to generate the message.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void WriteLine(string format, params object[] args)
            => Console.WriteLine(format, args);
    }
}
