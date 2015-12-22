// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Day22"/> class. This class cannot be inherited.
    /// </summary>
    public static class Day22Tests
    {
        [Fact]
        public static void Day22_Fight_Win_Scenario_1()
        {
            // Arrange
            IList<string> spellsToConjure = new[] { "Poison", "MagicMissile" };
            int index = 0;

            Func<Day22.Wizard, ICollection<string>, string> spellSelector = (w, s) => spellsToConjure[index++];

            var wizard = new Day22.Wizard(10, 250, spellSelector);
            var boss = new Day22.Boss(13, 8);

            // Act
            Day22.Player actual = Day22.Fight(wizard, boss);

            // Assert
            Assert.Same(wizard, actual);
            Assert.Equal(0, boss.Armor);
            Assert.Equal(8, boss.Damage);
            Assert.Equal(0, boss.HitPoints);
            Assert.Equal(0, wizard.Armor);
            Assert.Equal(0, wizard.Damage);
            Assert.Equal(2, wizard.HitPoints);
            Assert.Equal(24, wizard.Mana);
            Assert.Equal(new[] { "Poison" }, wizard.ActiveSpells);
            Assert.Equal(173 + 53, wizard.ManaSpent);
            Assert.Equal(spellsToConjure, wizard.SpellsCast);
        }

        [Fact]
        public static void Day22_Fight_Win_Scenario_2()
        {
            // Arrange
            IList<string> spellsToConjure = new[] { "Recharge", "Shield", "Drain", "Poison", "MagicMissile" };
            int index = 0;

            Func<Day22.Wizard, ICollection<string>, string> spellSelector = (w, s) => spellsToConjure[index++];

            var wizard = new Day22.Wizard(10, 250, spellSelector);
            var boss = new Day22.Boss(14, 8);

            // Act
            Day22.Player actual = Day22.Fight(wizard, boss);

            // Assert
            Assert.Same(wizard, actual);
            Assert.Equal(0, boss.Armor);
            Assert.Equal(8, boss.Damage);
            Assert.Equal(-1, boss.HitPoints);
            Assert.Equal(0, wizard.Damage);
            Assert.Equal(1, wizard.HitPoints);
            Assert.Equal(114, wizard.Mana);
            Assert.Equal(new[] { "Poison" }, wizard.ActiveSpells);
            Assert.Equal(229 + 113 + 73 + 173 + 53, wizard.ManaSpent);
            Assert.Equal(spellsToConjure, wizard.SpellsCast);
        }

        [Fact]
        public static void Day22_Solve_Returns_Correct_Solution()
        {
            // Arrange
            string[] args = new string[0];
            var target = new Day22();

            // Act
            int actual = target.Solve(args);

            // Assert
            Assert.Equal(0, actual);
            Assert.Equal(953, target.MinimumCostToWin);
        }
    }
}
