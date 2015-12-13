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
        /// <summary>
        /// Gets the argument test cases to pass to the application to test it.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1819:PropertiesShouldNotReturnArrays",
            Justification = "Required for xunit data-driven tests.")]
        public static object[][] Arguments
        {
            get
            {
                return new[]
                {
                    new[] { new[] { "1", @".\Input\Day01\input.txt" } },
                    new[] { new[] { "2", @".\Input\Day02\input.txt" } },
                    new[] { new[] { "3", @".\Input\Day03\input.txt" } },
                    new[] { new[] { "4", "iwrupvqb", "5" } },
                    new[] { new[] { "4", "iwrupvqb", "6" } },
                    new[] { new[] { "5", @".\Input\Day05\input.txt", "1" } },
                    new[] { new[] { "5", @".\Input\Day05\input.txt", "2" } },
                    new[] { new[] { "6", @".\Input\Day06\input.txt", "1" } },
                    new[] { new[] { "6", @".\Input\Day06\input.txt", "2" } },
                    new[] { new[] { "7", @".\Input\Day07\input.txt" } },
                    new[] { new[] { "8", @".\Input\Day08\input.txt" } },
                    new[] { new[] { "9", @".\Input\Day09\input.txt" } },
                    new[] { new[] { "9", @".\Input\Day09\input.txt", "true" } },
                    new[] { new[] { "10", "1321131112", "40" } },
                    new[] { new[] { "10", "1321131112", "50" } },
                    new[] { new[] { "11", "cqjxjnds" } },
                    new[] { new[] { "11", "cqjxxyzz" } },
                    new[] { new[] { "12", @".\Input\Day12\input.txt" } },
                    new[] { new[] { "12", @".\Input\Day12\input.txt", "red" } },
                };
            }
        }

        [Theory]
        [MemberData(nameof(Arguments))]
        public static void Program_Returns_Zero_If_Input_Valid(string[] args)
        {
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
