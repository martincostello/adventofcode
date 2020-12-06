// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;

    /// <summary>
    /// Represents the error that occurs when a puzzle cannot be solved.
    /// </summary>
    public sealed class PuzzleException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PuzzleException"/> class.
        /// </summary>
        public PuzzleException()
            : base("Failed to solve puzzle due to an exception.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PuzzleException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PuzzleException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PuzzleException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public PuzzleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
