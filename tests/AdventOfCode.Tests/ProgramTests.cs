// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode
{
    using Shouldly;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Program"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class ProgramTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramTests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public ProgramTests(ITestOutputHelper outputHelper)
        {
            Logger = new TestLogger(outputHelper);
        }

        /// <summary>
        /// Gets the <see cref="ILogger"/> to use.
        /// </summary>
        private ILogger Logger { get; }

        [Fact]
        public void Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "1", "2016" };

            // Act
            int actual = Program.Run(args, Logger);

            // Assert
            actual.ShouldBe(0);
        }

        [Fact]
        public void Program_Returns_Zero_If_Input_Invalid()
        {
            // Arrange
            string[] args = new[] { "26", "a" };

            // Act
            int actual = Program.Run(args, Logger);

            // Assert
            actual.ShouldBe(-1);
        }

        [Fact]
        public void Program_Exits_If_Invalid_Day()
        {
            // Arrange
            string[] args = new[] { "a" };

            // Act
            int actual = Program.Run(args, Logger);

            // Assert
            actual.ShouldBe(-1);
        }

        [Fact]
        public void Program_Exits_If_Day_Too_Small()
        {
            // Arrange
            string[] args = new[] { "0" };

            // Act
            int actual = Program.Run(args, Logger);

            // Assert
            actual.ShouldBe(-1);
        }

        [Fact]
        public void Program_Exits_If_Day_Too_Large()
        {
            // Arrange
            string[] args = new[] { "26" };

            // Act
            int actual = Program.Run(args, Logger);

            // Assert
            actual.ShouldBe(-1);
        }
    }
}
