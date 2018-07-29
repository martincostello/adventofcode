// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// A class containing tests for the <see cref="Day22"/> class. This class cannot be inherited.
    /// </summary>
    public class Day22Tests
    {
        /// <summary>
        /// The <see cref="ITestOutputHelper"/> to use.
        /// </summary>
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Initializes a new instance of the <see cref="Day22Tests"/> class.
        /// </summary>
        /// <param name="output">The <see cref="ITestOutputHelper"/> to use.</param>
        public Day22Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public static void Y2015_Day22_Solve_Returns_Correct_Solution_Easy()
        {
            // Arrange
            string[] args = Array.Empty<string>();

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day22>(args);

            // Assert
            Assert.Equal(953, puzzle.MinimumCostToWin);
        }

        [Fact]
        public static void Y2015_Day22_Solve_Returns_Correct_Solution_Hard()
        {
            // Arrange
            string[] args = new string[] { "hard" };

            // Act
            var puzzle = PuzzleTestHelpers.SolvePuzzle<Day22>(args);

            // Assert
            Assert.Equal(1289, puzzle.MinimumCostToWin);
        }

        [Fact]
        public void Y2015_Day22_Fight_Win_Scenario_1()
        {
            // Arrange
            IList<string> spellsToConjure = new[] { "Poison", "MagicMissile" };
            int index = 0;

            string SpellSelector(Day22.Wizard w, ICollection<string> s) => spellsToConjure[index++];

            var wizard = new Day22.Wizard(10, 250, SpellSelector);
            var opponent = new Day22.Boss(13, 8);
            string difficulty = "easy";

            // Act
            Day22.Player actual = Day22.Fight(wizard, opponent, difficulty, Output);

            // Assert
            Assert.Same(wizard, actual);
            Assert.Equal(0, opponent.Armor);
            Assert.Equal(8, opponent.Damage);
            Assert.Equal(0, opponent.HitPoints);
            Assert.Equal(0, wizard.Armor);
            Assert.Equal(0, wizard.Damage);
            Assert.Equal(2, wizard.HitPoints);
            Assert.Equal(24, wizard.Mana);
            Assert.Equal(new[] { "Poison" }, wizard.ActiveSpells);
            Assert.Equal(173 + 53, wizard.ManaSpent);
            Assert.Equal(spellsToConjure, wizard.SpellsCast);
        }

        [Fact]
        public void Y2015_Day22_Fight_Win_Scenario_2()
        {
            // Arrange
            IList<string> spellsToConjure = new[] { "Recharge", "Shield", "Drain", "Poison", "MagicMissile" };
            int index = 0;

            string SpellSelector(Day22.Wizard w, ICollection<string> s) => spellsToConjure[index++];

            var wizard = new Day22.Wizard(10, 250, SpellSelector);
            var opponent = new Day22.Boss(14, 8);
            string difficulty = "easy";

            // Act
            Day22.Player actual = Day22.Fight(wizard, opponent, difficulty, Output);

            // Assert
            Assert.Same(wizard, actual);
            Assert.Equal(0, opponent.Armor);
            Assert.Equal(8, opponent.Damage);
            Assert.Equal(-1, opponent.HitPoints);
            Assert.Equal(0, wizard.Damage);
            Assert.Equal(1, wizard.HitPoints);
            Assert.Equal(114, wizard.Mana);
            Assert.Equal(new[] { "Poison" }, wizard.ActiveSpells);
            Assert.Equal(229 + 113 + 73 + 173 + 53, wizard.ManaSpent);
            Assert.Equal(spellsToConjure, wizard.SpellsCast);
        }

        /// <summary>
        /// Outputs the specified message.
        /// </summary>
        /// <param name="format">
        /// A format string that contains zero or more format items, which
        /// correspond to objects in the <paramref name="args"/> array.
        /// </param>
        /// <param name="args">
        /// An object array containing zero or more objects to format.
        /// </param>
        private void Output(string format, object[] args)
        {
            _output.WriteLine(format, args);
        }
    }
}
