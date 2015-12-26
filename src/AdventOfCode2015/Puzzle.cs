// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// The base class for puzzles.
    /// </summary>
    internal abstract class Puzzle : IPuzzle
    {
        /// <summary>
        /// Gets a value indicating whether the first argument is expected to be a valid file path.
        /// </summary>
        protected virtual bool IsFirstArgumentFilePath => false;

        /// <summary>
        /// Gets the minimum number of arguments required to solve the puzzle.
        /// </summary>
        protected virtual int MinimumArguments => 0;

        /// <inheritdoc />
        public virtual int Solve(string[] args)
        {
            if (!EnsureArguments(args, MinimumArguments))
            {
                Console.Error.WriteLine(
                    "At least {0:N0} argument{1} {2} required.",
                    MinimumArguments,
                    MinimumArguments == 1 ? string.Empty : "s",
                    MinimumArguments == 1 ? "is" : "are");

                return -1;
            }

            if (IsFirstArgumentFilePath && !File.Exists(args[0]))
            {
                Console.Error.WriteLine("The input file path specified cannot be found.");
                return -1;
            }

            return SolveCore(args);
        }

        /// <summary>
        /// Replaces the format items in a specified string with the string representations
        /// of corresponding objects in a specified array.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the string
        /// representation of the corresponding objects in args.
        /// </returns>
        protected internal static string Format(string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        /// <summary>
        /// Parses the specified <see cref="string"/> as an <see cref="int"/>.
        /// </summary>
        /// <param name="s">The value to parsed.</param>
        /// <param name="style">
        /// A bitwise combination of enumeration values that indicates
        /// the style elements that can be present in <paramref name="s"/>.
        /// </param>
        /// <returns>
        /// The parsed value of <paramref name="s"/>.
        /// </returns>
        protected internal static int ParseInt32(string s, NumberStyles style = NumberStyles.Integer)
        {
            return int.Parse(s, style, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses the specified <see cref="string"/> as an <see cref="uint"/>.
        /// </summary>
        /// <param name="s">The value to parsed.</param>
        /// <returns>
        /// The parsed value of <paramref name="s"/>.
        /// </returns>
        protected internal static uint ParseUInt32(string s) => uint.Parse(s, CultureInfo.InvariantCulture);

        /// <summary>
        /// Ensures that the specified number of arguments are present.
        /// </summary>
        /// <param name="args">The input arguments to the puzzle.</param>
        /// <param name="minimumLength">The minimum number of arguments required.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="args"/> is at least
        /// <paramref name="minimumLength"/> in length; otherwise <see langword="false"/>.
        /// </returns>
        protected static bool EnsureArguments(ICollection<string> args, int minimumLength)
        {
            return args.Count >= minimumLength;
        }

        /// <summary>
        /// Solves the puzzle given the specified arguments.
        /// </summary>
        /// <param name="args">The input arguments to the puzzle.</param>
        /// <returns>The exit code the application should return.</returns>
        protected abstract int SolveCore(string[] args);
    }
}
