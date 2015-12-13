// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Program"/> class. This class cannot be inherited.
    /// </summary>
    public static class ProgramTests
    {
        [Fact(Skip = "Needs input file.")]
        public static void Day01_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "1", "input.txt" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact(Skip = "Needs input file.")]
        public static void Day02_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "2", "input.txt" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact(Skip = "Needs input file.")]
        public static void Day03_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "3", "input.txt" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact(Skip = "Too slow.")]
        public static void Day04_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "4", "iwrupvqb" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact(Skip = "Needs input file.")]
        public static void Day05_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "5", "input.txt", "1" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);

            // Arrange
            args = new[] { "5", "input.txt", "2" };

            // Act
            actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact(Skip = "Needs input file.")]
        public static void Day06_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "6", "input.txt", "1" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);

            // Arrange
            args = new[] { "6", "input.txt", "2" };

            // Act
            actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact(Skip = "Needs input file.")]
        public static void Day07_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "7", "input.txt" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact(Skip = "Needs input file.")]
        public static void Day08_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "8", "input.txt" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact(Skip = "Needs input file.")]
        public static void Day09_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "9", "input.txt" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);

            // Arrange
            args = new[] { "9", "input.txt", "true" };

            // Act
            actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public static void Day10_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "10", "1321131112", "40" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);

            // Arrange
            args = new[] { "10", "1321131112", "50" };

            // Act
            actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public static void Day11_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "11", "cqjxjnds" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);

            args = new[] { "11", "cqjxxyzz" };

            // Act
            actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact(Skip = "Needs input file.")]
        public static void Day12_Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { "12", "input.txt" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public static void Program_Exits_If_Null_Arguments()
        {
            // Arrange
            string[] args = null;

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(-1, actual);
        }

        [Fact]
        public static void Program_Exits_If_No_Arguments()
        {
            // Arrange
            string[] args = new string[0];

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(-1, actual);
        }

        [Fact]
        public static void Program_Exits_If_Invalid_Day()
        {
            // Arrange
            string[] args = new[] { "a" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(-1, actual);
        }

        [Fact]
        public static void Program_Exits_If_Day_Too_Small()
        {
            // Arrange
            string[] args = new[] { "0" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(-1, actual);
        }

        [Fact]
        public static void Program_Exits_If_Day_Too_Large()
        {
            // Arrange
            string[] args = new[] { "26" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(-1, actual);
        }
    }
}
