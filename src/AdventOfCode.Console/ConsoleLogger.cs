// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class representing an <see cref="ILogger"/> implementation for the console. This class cannot be inherited.
/// </summary>
internal sealed class ConsoleLogger : ILogger
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
        int lengthY;

        if (!Console.IsOutputRedirected && (lengthY = array.GetLength(1)) <= Console.WindowHeight)
        {
            Console.WriteLine();

            for (int y = 0; y < lengthY; y++)
            {
                foreach (bool value in array.GetColumn(y))
                {
                    Console.Write(value ? trueChar : falseChar);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        return string.Empty;
    }

    /// <summary>
    /// Writes a message to the log.
    /// </summary>
    /// <param name="format">The format string to use to generate the message.</param>
    /// <param name="args">The arguments for the format string.</param>
    public void WriteLine(string format, params object[] args)
        => Console.WriteLine(format, args);
}
