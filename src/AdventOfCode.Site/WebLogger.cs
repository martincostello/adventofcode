// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using IMicrosoftLogger = Microsoft.Extensions.Logging.ILogger;

namespace MartinCostello.AdventOfCode.Site;

/// <summary>
/// A class representing an <see cref="ILogger"/> implementation for the web application. This class cannot be inherited.
/// </summary>
/// <param name="logger">The <see cref="ILogger{T}"/> to use.</param>
public sealed partial class WebLogger(ILogger<WebLogger> logger) : ILogger
{
    /// <summary>
    /// Writes a grid to the log.
    /// </summary>
    /// <param name="array">The array to write to the log.</param>
    /// <param name="falseChar">The character to display for <see langword="false"/>.</param>
    /// <param name="trueChar">The character to display for <see langword="true"/>.</param>
    /// <returns>
    /// The visualization of the grid.
    /// </returns>
    public string WriteGrid(bool[,] array, char falseChar, char trueChar)
    {
        int lengthX = array.GetLength(0);
        int lengthY = array.GetLength(1);

        var builder = new StringBuilder(((lengthX + 2) * lengthY) + 2);

        for (int y = 0; y < lengthY; y++)
        {
            foreach (bool value in array.GetColumn(y))
            {
                builder.Append(value ? trueChar : falseChar);
            }

            builder.AppendLine();
        }

        builder.AppendLine();

        string result = builder.ToString();

        Log.Result(logger, result);

        return result;
    }

    /// <summary>
    /// Writes a message to the log.
    /// </summary>
    /// <param name="format">The format string to use to generate the message.</param>
    /// <param name="args">The arguments for the format string.</param>
    public void WriteLine(string format, params object[] args)
        => Log.WriteLine(logger, string.Format(CultureInfo.InvariantCulture, format, args));

    private static partial class Log
    {
        [LoggerMessage(1, LogLevel.Information, "{Message}")]
        public static partial void WriteLine(IMicrosoftLogger logger, string message);

        [LoggerMessage(2, LogLevel.Information, "{Result}")]
        public static partial void Result(IMicrosoftLogger logger, string result);
    }
}
