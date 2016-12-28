// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// The base class for puzzles.
    /// </summary>
    internal abstract class Puzzle : IPuzzle
    {
        /// <summary>
        /// Gets the minimum number of arguments required to solve the puzzle.
        /// </summary>
        protected virtual int MinimumArguments => 0;

        /// <summary>
        /// Gets the year associated with the puzzle.
        /// </summary>
        protected abstract int Year { get; }

        /// <inheritdoc />
        public virtual int Solve(string[] args)
        {
            if (!EnsureArguments(args, MinimumArguments))
            {
                Console.WriteLine(
                    "At least {0:N0} argument{1} {2} required.",
                    MinimumArguments,
                    MinimumArguments == 1 ? string.Empty : "s",
                    MinimumArguments == 1 ? "is" : "are");

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
        /// <param name="s">The value to parse.</param>
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
        /// Tries to parse the specified <see cref="string"/> as an <see cref="int"/>.
        /// </summary>
        /// <param name="s">The value to parse.</param>
        /// <param name="value">
        /// When the method returns contains the parsed value
        /// of <paramref name="s"/>; otherwise zero.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="s"/> was parsed
        /// successfully; otherwise <see langword="false"/>.
        /// </returns>
        protected internal static bool TryParseInt32(string s, out int value)
        {
            return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        /// <summary>
        /// Parses the specified <see cref="string"/> as an <see cref="uint"/>.
        /// </summary>
        /// <param name="s">The value to parse.</param>
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
        /// Returns the <see cref="Stream"/> associated with the resource for the puzzle.
        /// </summary>
        /// <returns>
        /// A <see cref="Stream"/> containing the resource associated with the puzzle.
        /// </returns>
        protected Stream ReadResource()
        {
            var thisType = GetType().GetTypeInfo();
            string name = FormattableString.Invariant($"{thisType.Assembly.GetName().Name}.Input.Y{Year}.{thisType.Name}.input.txt");

            return thisType.Assembly.GetManifestResourceStream(name);
        }

        /// <summary>
        /// Returns the lines associated with the resource for the puzzle as a <see cref="string"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="IList{T}"/> containing the lines of the resource associated with the puzzle.
        /// </returns>
        protected IList<string> ReadResourceAsLines()
        {
            List<string> lines = new List<string>();

            using (Stream stream = ReadResource())
            {
                using (TextReader reader = new StreamReader(stream))
                {
                    string value = null;

                    while ((value = reader.ReadLine()) != null)
                    {
                        lines.Add(value);
                    }
                }
            }

            return lines;
        }

        /// <summary>
        /// Returns a <see cref="string"/> containing the content of the resource associated with the puzzle..
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> containing the content of the resource associated with the puzzle.
        /// </returns>
        protected string ReadResourceAsString()
        {
            using (Stream stream = ReadResource())
            {
                using (TextReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Solves the puzzle given the specified arguments.
        /// </summary>
        /// <param name="args">The input arguments to the puzzle.</param>
        /// <returns>The exit code the application should return.</returns>
        protected abstract int SolveCore(string[] args);
    }
}
