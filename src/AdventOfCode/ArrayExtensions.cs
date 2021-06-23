// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class containing extension methods for <see cref="Array"/>. This class cannot be inherited.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class ArrayExtensions
    {
        /// <summary>
        /// Counts the number of <see langword="true"/> values in the specified array.
        /// </summary>
        /// <param name="array">The array to count the number of true values in.</param>
        /// <returns>
        /// The number of indexes that are <see langword="true"/> lit in <paramref name="array"/>.
        /// </returns>
        internal static int TrueCount(this bool[,] array)
        {
            int result = 0;
            int boundsX = array.GetLength(0);
            int boundsY = array.GetLength(1);

            for (int x = 0; x < boundsX; x++)
            {
                for (int y = 0; y < boundsY; y++)
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
