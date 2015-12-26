// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Puzzle"/> class. This class cannot be inherited.
    /// </summary>
    public static class PuzzleTests
    {
        [Fact]
        public static void Puzzle_Solve_Returns_Correct_Value_Based_On_Args_Length()
        {
            // Arrange
            string[] args = new[] { "1" };
            var target = new MyPuzzle(2);

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(-1, actual);

            // Arrange
            target = new MyPuzzle(1, isFirstArgumentFilePath: true);

            // Act
            actual = target.Solve(args);

            // Assert
            Assert.Equal(-1, actual);

            // Arrange
            args = new string[0];
            target = new MyPuzzle(1);

            // Act
            actual = target.Solve(args);

            // Assert
            Assert.Equal(-1, actual);

            // Arrange
            target = new MyPuzzle(0);

            // Act
            actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(42, target.Answer);
        }

        /// <summary>
        /// A class representing a test implementation of <see cref="Puzzle"/>. This class cannot be inherited.
        /// </summary>
        private sealed class MyPuzzle : Puzzle
        {
            /// <summary>
            /// The value to use for <see cref="IsFirstArgumentFilePath"/>.
            /// </summary>
            private readonly bool _isFirstArgumentFilePath;

            /// <summary>
            /// The value to use for <see cref="MinimumArguments"/>.
            /// </summary>
            private readonly int _minimumArguments;

            /// <summary>
            /// Initializes a new instance of the <see cref="MyPuzzle"/> class.
            /// </summary>
            /// <param name="minimumArguments">The value to use for <see cref="MinimumArguments"/>.</param>
            /// <param name="isFirstArgumentFilePath">The optional value to use for <see cref="IsFirstArgumentFilePath"/>.</param>
            internal MyPuzzle(int minimumArguments, bool isFirstArgumentFilePath = false)
            {
                _isFirstArgumentFilePath = isFirstArgumentFilePath;
                _minimumArguments = minimumArguments;
            }

            /// <summary>
            /// Gets the answer to the puzzle.
            /// </summary>
            internal int Answer { get; private set; }

            /// <inheritdoc />
            protected override bool IsFirstArgumentFilePath => _isFirstArgumentFilePath;

            /// <inheritdoc />
            protected override int MinimumArguments => _minimumArguments;

            /// <inheritdoc />
            protected override int SolveCore(string[] args)
            {
                Answer = 42;
                return 0;
            }
        }
    }
}
