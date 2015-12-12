// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles.Impl
{
    /// <summary>
    /// Defines a rule for testing for the whether a <see cref="String"/> is 'nice'.
    /// </summary>
    internal interface INicenessRule
    {
        /// <summary>
        /// Returns whether the specified string is 'nice'.
        /// </summary>
        /// <param name="value">The string to test for niceness.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> is 'nice'; otherwise <see langword="false"/>.</returns>
        bool IsNice(string value);
    }
}
