﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

public sealed class Day22Tests(ITestOutputHelper outputHelper) : PuzzleTest(outputHelper)
{
    [Theory]
    [InlineData("easy", 953)]
    [InlineData("hard", 1289)]
    public async Task Y2015_Day22_Solve_Returns_Correct_Solution(string difficulty, int expected)
    {
        // Act
        var puzzle = await SolvePuzzleAsync<Day22>(difficulty);

        // Assert
        puzzle.MinimumCostToWin.ShouldBe(expected);
    }

    [Fact]
    public void Y2015_Day22_Fight_Win_Scenario_1()
    {
        // Arrange
        IList<string> spellsToConjure = ["Poison", "MagicMissile"];
        int index = 0;

        string SpellSelector(Day22.Wizard w, ICollection<string> s) => spellsToConjure[index++];

        var wizard = new Day22.Wizard(10, 250, SpellSelector);
        var opponent = new Day22.Boss(13, 8);
        string difficulty = "easy";

        // Act
        Day22.Player actual = Day22.Fight(wizard, opponent, difficulty, Output);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldBeSameAs(wizard);
        opponent.Armor.ShouldBe(0);
        opponent.Damage.ShouldBe(8);
        opponent.HitPoints.ShouldBe(0);
        wizard.Armor.ShouldBe(0);
        wizard.Damage.ShouldBe(0);
        wizard.HitPoints.ShouldBe(2);
        wizard.Mana.ShouldBe(24);
        wizard.ActiveSpells.ShouldBe(["Poison"]);
        wizard.ManaSpent.ShouldBe(173 + 53);
        wizard.SpellsCast.ShouldBe(spellsToConjure);
    }

    [Fact]
    public void Y2015_Day22_Fight_Win_Scenario_2()
    {
        // Arrange
        IList<string> spellsToConjure = ["Recharge", "Shield", "Drain", "Poison", "MagicMissile"];
        int index = 0;

        string SpellSelector(Day22.Wizard w, ICollection<string> s) => spellsToConjure[index++];

        var wizard = new Day22.Wizard(10, 250, SpellSelector);
        var opponent = new Day22.Boss(14, 8);
        string difficulty = "easy";

        // Act
        Day22.Player actual = Day22.Fight(wizard, opponent, difficulty, Output);

        // Assert
        actual.ShouldNotBeNull();
        actual.ShouldBeSameAs(wizard);
        opponent.Armor.ShouldBe(0);
        opponent.Damage.ShouldBe(8);
        opponent.HitPoints.ShouldBe(-1);
        wizard.Damage.ShouldBe(0);
        wizard.HitPoints.ShouldBe(1);
        wizard.Mana.ShouldBe(114);
        wizard.ActiveSpells.ShouldBe(["Poison"]);
        wizard.ManaSpent.ShouldBe(229 + 113 + 73 + 173 + 53);
        wizard.SpellsCast.ShouldBe(spellsToConjure);
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
        => OutputHelper.WriteLine(format, args);
}
