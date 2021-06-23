// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace MartinCostello.AdventOfCode
{
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

        /// <summary>
        /// Writes a grid to the log.
        /// </summary>
        /// <param name="logger">The logger to write to.</param>
        /// <param name="array">The array to write to the log.</param>
        /// <returns>
        /// The visualization of the grid.
        /// </returns>
        internal static string WriteGrid(this ILogger logger, char[,] array)
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);

            var builder = new StringBuilder((width + Environment.NewLine.Length) * height);

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    builder.Append(array[i, j]);
                }

                builder.AppendLine();
            }

            string visualization = builder.ToString();

            logger?.WriteLine(visualization);

            return visualization;
        }
    }
}
