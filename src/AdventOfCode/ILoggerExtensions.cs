// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System.ComponentModel;

    /// <summary>
    /// A class containing extension methods for <see cref="ILogger"/>. This class cannot be inherited.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class ILoggerExtensions
    {
        /// <summary>
        /// Writes a new line to the log.
        /// </summary>
        /// <param name="logger">The logger to write to.</param>
        internal static void WriteLine(this ILogger logger)
            => logger.WriteLine(string.Empty);
    }
}
