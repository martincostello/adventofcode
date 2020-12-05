﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;

    /// <summary>
    /// Represents metadata about a puzzle.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PuzzleAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PuzzleAttribute"/> class.
        /// </summary>
        public PuzzleAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the minimum number of arguments required to solve the puzzle.
        /// </summary>
        public int MinimumArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the puzzle requires input data.
        /// </summary>
        public bool RequiresData { get; set; }
    }
}
