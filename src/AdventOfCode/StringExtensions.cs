// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class containing extension methods for strings. This class cannot be inherited.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Returns a string that is a mirror image of the string.
        /// </summary>
        /// <param name="value">The string to mirror.</param>
        /// <returns>
        /// A string which is the mirror of the original.
        /// </returns>
        public static string Mirror(this string value)
        {
            if (value.Length < 2)
            {
                return value;
            }

            return new string(value.Reverse().ToArray());
        }
    }
}
