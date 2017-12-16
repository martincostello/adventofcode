// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;

    /// <summary>
    /// A class containing often-used arrays. This class cannot be inherited.
    /// </summary>
    internal static class Arrays
    {
        /// <summary>
        /// An array containing the ':' character. This field is read-only.
        /// </summary>
        internal static readonly char[] Colon = new[] { ':' };

        /// <summary>
        /// An array containing the ',' character. This field is read-only.
        /// </summary>
        internal static readonly char[] Comma = new[] { ',' };

        /// <summary>
        /// An array containing the '-' character. This field is read-only.
        /// </summary>
        internal static readonly char[] Dash = new[] { '-' };

        /// <summary>
        /// An array containing the '=' character. This field is read-only.
        /// </summary>
        internal static readonly char[] EqualsSign = new[] { '=' };

        /// <summary>
        /// An array containing the ' ' character. This field is read-only.
        /// </summary>
        internal static readonly char[] Space = new[] { ' ' };

        /// <summary>
        /// An array containing the 'x' character. This field is read-only.
        /// </summary>
        internal static readonly char[] X = new[] { 'x' };

        /// <summary>
        /// Prints the array to the console.
        /// </summary>
        /// <param name="array">The array to print to the console.</param>
        /// <param name="falseChar">The character to display for <see langword="false"/>.</param>
        /// <param name="trueChar">The character to display for <see langword="true"/>.</param>
        internal static void Print(bool[,] array, char falseChar, char trueChar)
        {
            Console.WriteLine();

            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    Console.Write(array[x, y] ? trueChar : falseChar);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Counts the number of <see langword="true"/> values in the specified array.
        /// </summary>
        /// <param name="array">The array to count the number of true values in.</param>
        /// <returns>
        /// The number of indexes that are <see langword="true"/> lit in <paramref name="array"/>.
        /// </returns>
        internal static int TrueCount(bool[,] array)
        {
            int result = 0;

            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    if (array[x, y])
                    {
                        result++;
                    }
                }
            }

            return result;
        }
    }
}
