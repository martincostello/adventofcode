// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    /// <summary>
    /// A class containing tests for the <see cref="Day07"/> class. This class cannot be inherited.
    /// </summary>
    public sealed class Day07Tests : PuzzleTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Day07Tests"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day07Tests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public static void Y2015_Day07_InstructionsAreInterpretedCorrectly()
        {
            // Arrange
            string[] instructions = new string[]
            {
                "j -> k",
                "123 -> x",
                "456 -> y",
                "x AND y -> d",
                "x OR y -> e",
                "x LSHIFT 2 -> f",
                "y RSHIFT 2 -> g",
                "NOT x -> h",
                "NOT y -> i",
                "i -> j",
            };

            // Act
            IDictionary<string, ushort> actual = Day07.GetWireValues(instructions);

            // Assert
            actual.ShouldNotBeNull();
            actual.ShouldContainKeyAndValue<string, ushort>("d", 72);
            actual.ShouldContainKeyAndValue<string, ushort>("e", 507);
            actual.ShouldContainKeyAndValue<string, ushort>("f", 492);
            actual.ShouldContainKeyAndValue<string, ushort>("g", 114);
            actual.ShouldContainKeyAndValue<string, ushort>("h", 65412);
            actual.ShouldContainKeyAndValue<string, ushort>("i", 65079);
            actual.ShouldContainKeyAndValue<string, ushort>("j", 65079);
            actual.ShouldContainKeyAndValue<string, ushort>("k", 65079);
            actual.ShouldContainKeyAndValue<string, ushort>("x", 123);
            actual.ShouldContainKeyAndValue<string, ushort>("y", 456);
        }

        [Fact]
        public async Task Y2015_Day07_Solve_Returns_Correct_Solution()
        {
            // Act
            var puzzle = await SolvePuzzleAsync<Day07>();

            // Assert
            puzzle.FirstSignal.ShouldBe(3176);
            puzzle.SecondSignal.ShouldBe(14710);
        }
    }
}
