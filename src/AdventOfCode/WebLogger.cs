// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

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
    /// <returns>
    /// The visualization of the grid.
    /// </returns>
    public string WriteGrid(bool[,] array, char falseChar, char trueChar)
    {
        int lengthX = array.GetLength(0);
        int lengthY = array.GetLength(1);

        var builder = new StringBuilder((lengthX + 2) * lengthY);

        for (int x = 0; x < lengthX; x++)
        {
            for (int y = 0; y < lengthY; y++)
            {
                builder.Append(array[x, y] ? trueChar : falseChar);
            }

            builder.AppendLine();
        }

        builder.AppendLine();

        string result = builder.ToString();

        _logger.LogInformation("{Result}", result);

        return result;
    }

    /// <summary>
    /// Writes a message to the log.
    /// </summary>
    /// <param name="format">The format string to use to generate the message.</param>
    /// <param name="args">The arguments for the format string.</param>
    public void WriteLine(string format, params object[] args)
        => _logger.LogInformation("{Message}", string.Format(CultureInfo.InvariantCulture, format, args));
}
