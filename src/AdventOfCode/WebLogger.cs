// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System.Text;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A class representing an <see cref="ILogger"/> implementation for the web application. This class cannot be inherited.
    /// </summary>
    public sealed class WebLogger : ILogger
    {
        /// <summary>
        /// The <see cref="ILogger{T}"/> to use. This field is read-only.
        /// </summary>
        private readonly ILogger<WebLogger> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebLogger"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{T}"/> to use.</param>
        public WebLogger(ILogger<WebLogger> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Writes a grid to the log.
        /// </summary>
        /// <param name="array">The array to write to the log.</param>
        /// <param name="falseChar">The character to display for <see langword="false"/>.</param>
        /// <param name="trueChar">The character to display for <see langword="true"/>.</param>
        public void WriteGrid(bool[,] array, char falseChar, char trueChar)
        {
            var builder = new StringBuilder();

            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    builder.Append(array[x, y] ? trueChar : falseChar);
                }

                builder.AppendLine();
            }

            builder.AppendLine();

            _logger.LogInformation(builder.ToString());
        }

        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="format">The format string to use to generate the message.</param>
        /// <param name="args">The arguments for the format string.</param>
        public void WriteLine(string format, params object[] args)
            => _logger.LogInformation(format, args);
    }
}
