// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles
{
    using System;
    using System.Text;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// The class class for puzzle tests.
    /// </summary>
    public abstract class PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PuzzleTest"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        protected PuzzleTest(ITestOutputHelper outputHelper)
        {
            Logger = new TestLogger(outputHelper);
        }

        /// <summary>
        /// Gets the <see cref="ILogger"/> to use.
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Solves the specified puzzle type.
        /// </summary>
        /// <typeparam name="T">The type of the puzzle to solve.</typeparam>
        /// <returns>
        /// The solved puzzle of the type specified by <typeparamref name="T"/>.
        /// </returns>
        protected T SolvePuzzle<T>()
            where T : Puzzle, new()
        {
            return SolvePuzzle<T>(Array.Empty<string>());
        }

        /// <summary>
        /// Solves the specified puzzle type with the specified arguments.
        /// </summary>
        /// <typeparam name="T">The type of the puzzle to solve.</typeparam>
        /// <param name="args">The arguments to pass to the puzzle.</param>
        /// <returns>
        /// The solved puzzle of the type specified by <typeparamref name="T"/>.
        /// </returns>
        protected T SolvePuzzle<T>(params string[] args)
            where T : Puzzle, new()
        {
            // Arrange
            var puzzle = new T()
            {
                Logger = Logger,
                Verbose = true,
            };

            // Act
            int result = puzzle.Solve(args);

            // Assert
            Assert.Equal(0, result);

            return puzzle;
        }

        /// <summary>
        /// A class representing an implementation of <see cref="ILogger"/> that
        /// logs to the XUnit output. This class cannot be inherited.
        /// </summary>
        private sealed class TestLogger : ILogger
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TestLogger"/> class.
            /// </summary>
            /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
            internal TestLogger(ITestOutputHelper outputHelper)
            {
                OutputHelper = outputHelper;
            }

            /// <summary>
            /// Gets the <see cref="ITestOutputHelper"/> to use.
            /// </summary>
            private ITestOutputHelper OutputHelper { get; }

            /// <inheritdoc />
            public void WriteGrid(bool[,] array, char falseChar, char trueChar)
            {
                var builder = new StringBuilder().AppendLine();

                for (int x = 0; x < array.GetLength(0); x++)
                {
                    for (int y = 0; y < array.GetLength(1); y++)
                    {
                        builder.Append(array[x, y] ? trueChar : falseChar);
                    }

                    builder.AppendLine();
                }

                builder.AppendLine();

                OutputHelper.WriteLine(builder.ToString());
            }

            /// <inheritdoc />
            public void WriteLine(string format, params object[] args)
                => OutputHelper.WriteLine(format, args);
        }
    }
}
