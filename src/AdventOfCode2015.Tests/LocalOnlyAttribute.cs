// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015
{
    using System;
    using Xunit;

    /// <summary>
    /// Attribute that is applied to a method to indicate that it is a fact that should
    /// only be run by the test runner locally and not in the <c>AppVeyor</c> continuous
    /// integration. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class LocalOnlyAttribute : FactAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalOnlyAttribute"/> class.
        /// </summary>
        public LocalOnlyAttribute()
        {
            if (string.Equals(Environment.GetEnvironmentVariable("CI"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                Skip = "Too slow.";
            }
        }
    }
}
