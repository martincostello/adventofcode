// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class containing often-used arrays. This class cannot be inherited.
    /// </summary>
    internal static class Arrays
    {
        /// <summary>
        /// An array containing dashes. This field is read-only.
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
    }
}
