// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.AdventOfCode
{
    /// <summary>
    /// A class containing tests for the <see cref="Puzzle"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class PuzzleTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PuzzleTests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public PuzzleTests(ITestOutputHelper outputHelper)
        {
            Logger = new TestLogger(outputHelper);
        }

        /// <summary>
        /// Gets the <see cref="ILogger"/> to use.
        /// </summary>
        private ILogger Logger { get; }

        [Fact]
        public async Task Puzzle_Solve_Returns_Correct_Value_Based_On_Args_Length()
        {
            // Arrange
            string[] args = new[] { "1" };
            var cancellationToken = CancellationToken.None;

            var target = new MyPuzzle(2)
            {
                Logger = Logger,
            };

            // Act and Assert
            await Assert.ThrowsAsync<PuzzleException>(() => target.SolveAsync(args, cancellationToken));

            // Arrange
            args = Array.Empty<string>();
            target = new MyPuzzle(1);

            // Act and Assert
            await Assert.ThrowsAsync<PuzzleException>(() => target.SolveAsync(args, cancellationToken));

            // Arrange
            target = new MyPuzzle(0);

            // Act
            PuzzleResult actual = await target.SolveAsync(args, cancellationToken);

            // Assert
            actual.ShouldNotBeNull();
            actual.Solutions.ShouldNotBeNull();
            actual.Solutions.Count.ShouldBe(1);
            actual.Solutions[0].ShouldBe(42);
            target.Answer.ShouldBe(42);
        }

        /// <summary>
        /// A class representing a test implementation of <see cref="Puzzle"/>. This class cannot be inherited.
        /// </summary>
        private sealed class MyPuzzle : Puzzle
        {
            /// <summary>
            /// The value to use for <see cref="MinimumArguments"/>.
            /// </summary>
            private readonly int _minimumArguments;

            /// <summary>
            /// Initializes a new instance of the <see cref="MyPuzzle"/> class.
            /// </summary>
            /// <param name="minimumArguments">The value to use for <see cref="MinimumArguments"/>.</param>
            internal MyPuzzle(int minimumArguments)
            {
                _minimumArguments = minimumArguments;
            }

            /// <summary>
            /// Gets the answer to the puzzle.
            /// </summary>
            internal int Answer { get; private set; }

            /// <inheritdoc />
            protected override int MinimumArguments => _minimumArguments;

            /// <inheritdoc />
            protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
            {
                Answer = 42;
                return PuzzleResult.Create(Answer);
            }
        }
    }
}
