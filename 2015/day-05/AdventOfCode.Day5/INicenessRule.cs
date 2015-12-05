// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INicenessRule.cs" company="https://github.com/martincostello/adventofcode">
//   Martin Costello (c) 2015
// </copyright>
// <license>
//   See license.txt in the project root for license information.
// </license>
// <summary>
//   INicenessRule.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MartinCostello.AdventOfCode.Day5
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
