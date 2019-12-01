// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/2015/day/22</c>. This class cannot be inherited.
    /// </summary>
    public sealed class Day22 : Puzzle2015
    {
        /// <summary>
        /// Gets the minimum amount of mana that can be spent to win.
        /// </summary>
        internal int MinimumCostToWin { get; private set; }

        /// <summary>
        /// Simulates a fight between the wizard and the boss.
        /// </summary>
        /// <param name="spellSelector">A delegate to a method to use to select the next spell to conjure.</param>
        /// <param name="difficulty">The difficulty to play with.</param>
        /// <returns>
        /// A named tuple that returns whether the wizard won and the amount of mana spent by the wizard.
        /// </returns>
        internal static (bool didWizardWin, int manaSpent) Fight(Func<Wizard, ICollection<string>, string> spellSelector, string difficulty)
        {
            var wizard = new Wizard(spellSelector);
            var boss = new Boss();

            Player winner = Fight(wizard, boss, difficulty, null);

            return (winner == wizard, wizard.ManaSpent);
        }

        /// <summary>
        /// Simulates a fight between the specified wizard and opponent.
        /// </summary>
        /// <param name="wizard">The wizard.</param>
        /// <param name="opponent">The opponent.</param>
        /// <param name="difficulty">The difficulty to play with.</param>
        /// <param name="output">A delegate to a method to use to output messages.</param>
        /// <returns>
        /// The player which won the fight.
        /// </returns>
        internal static Player Fight(Wizard wizard, Player opponent, string difficulty, Action<string, object[]>? output)
        {
            Player attacker = wizard;
            Player defender = opponent;

            bool isHardDifficulty = string.Equals("HARD", difficulty, StringComparison.OrdinalIgnoreCase);

            do
            {
                output?.Invoke("-- {0} turn --", new object[] { attacker.Name });
                output?.Invoke("- {0} has {1} hit points, {2} armor, {3} mana", new object[] { wizard.Name, wizard.HitPoints, wizard.Armor, wizard.Mana });
                output?.Invoke("- {0} has {1} hit points", new object[] { opponent.Name, opponent.HitPoints });

                if (isHardDifficulty && attacker == wizard)
                {
                    wizard.HitPoints--;

                    if (wizard.HitPoints < 1)
                    {
                        break;
                    }
                }

                wizard.CastSpells(opponent, output);

                if (attacker.HitPoints > 0)
                {
                    attacker.Attack(defender, output);
                }

                Player player = attacker;
                attacker = defender;
                defender = player;

                output?.Invoke(string.Empty, Array.Empty<object>());
            }
            while (wizard.HitPoints > 0 && opponent.HitPoints > 0);

            return wizard.HitPoints > 0 ? wizard : opponent;
        }

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            string difficulty = args.Length == 1 ? args[0] : "easy";

            var solutions = new List<(bool didWizardWin, int manaSpent)>();
            var random = new Random();

            // Play the game 100,000 times with random choices of spells
            while (solutions.Count < 100000)
            {
                var result = Fight((wizard, spells) => spells.ElementAt(random.Next(0, spells.Count)), difficulty);
                solutions.Add(result);
            }

            MinimumCostToWin = solutions
                .Where((p) => p.didWizardWin)
                .Min((p) => p.manaSpent);

            if (Verbose)
            {
                Logger.WriteLine(
                    "The minimum amount of mana that can be spent to win on {0} difficulty is {1:N0}.",
                    difficulty,
                    MinimumCostToWin);
            }

            return 0;
        }

        /// <summary>
        /// A class representing the boss. This class cannot be inherited.
        /// </summary>
        internal sealed class Boss : Player
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Boss"/> class.
            /// </summary>
            internal Boss()
                : this(55, 8)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Boss"/> class.
            /// </summary>
            /// <param name="hitPoints">The number of hit points the boss has.</param>
            /// <param name="damage">The initial damage the boss inflicts on others.</param>
            internal Boss(int hitPoints, int damage)
                : base("Boss", hitPoints, damage)
            {
            }

            /// <inheritdoc />
            public override void Attack(Player other, Action<string, object[]>? output)
            {
                int damage = Damage - other.Armor;

                if (damage < 1)
                {
                    damage = 1;
                }

                other.HitPoints -= damage;

                if (other.Armor == 0)
                {
                    output?.Invoke("{0} attacks for {1} damage!", new object[] { Name, damage });
                }
                else
                {
                    output?.Invoke("{0} attacks for {1} - {2} = {3} damage!", new object[] { Name, Damage, other.Armor, damage });
                }
            }
        }

        /// <summary>
        /// A class representing the wizard. This class cannot be inherited.
        /// </summary>
        internal sealed class Wizard : Player
        {
            /// <summary>
            /// A delegate to a method to use to select the next spell to conjure. This field is read-only.
            /// </summary>
            private readonly Func<Wizard, ICollection<string>, string> _spellSelector;

            /// <summary>
            /// The spells the wizard currently has equipped, including any that have lost their power.
            /// </summary>
            private readonly ICollection<Spell> _spells = new List<Spell>();

            /// <summary>
            /// Initializes a new instance of the <see cref="Wizard"/> class.
            /// </summary>
            /// <param name="spellSelector">A delegate to a method to use to select the next spell to conjure.</param>
            internal Wizard(Func<Wizard, ICollection<string>, string> spellSelector)
                : this(50, 500, spellSelector)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Wizard"/> class.
            /// </summary>
            /// <param name="hitPoints">The number of hit points the wizard has.</param>
            /// <param name="mana">The amount of mana the wizard has.</param>
            /// <param name="spellSelector">A delegate to a method to use to select the next spell to conjure.</param>
            internal Wizard(int hitPoints, int mana, Func<Wizard, ICollection<string>, string> spellSelector)
                : base("Player", hitPoints, 0)
            {
                Mana = mana;
                _spellSelector = spellSelector;
            }

            /// <summary>
            /// Gets the names of the active spells cast by the wizard.
            /// </summary>
            internal ICollection<string> ActiveSpells => _spells.Where((p) => p.TurnsLeft > 0).Select((p) => p.Name).ToArray();

            /// <summary>
            /// Gets the names of all the spells cast by the wizard.
            /// </summary>
            internal ICollection<string> SpellsCast => _spells.Select((p) => p.Name).ToArray();

            /// <summary>
            /// Gets or sets the amount of mana the wizard has.
            /// </summary>
            internal int Mana { get; set; }

            /// <summary>
            /// Gets the amount of mana the wizard has spent.
            /// </summary>
            internal int ManaSpent { get; private set; }

            /// <summary>
            /// Attacks the specified player.
            /// </summary>
            /// <param name="other">The other player to attack with this player.</param>
            /// <param name="output">A delegate to a method to use to output messages.</param>
            public override void Attack(Player other, Action<string, object[]>? output)
            {
                var availableSpells = SpellInfo.Spells
                    .Where((p) => p.Value.Cost <= Mana)
                    .Where((p) => !ActiveSpells.Contains(p.Key))
                    .ToList();

                if (availableSpells.Count < 1)
                {
                    HitPoints = 0;
                    return;
                }

                // Determine which spell the wizard should cast
                string spellToConjure = _spellSelector(this, availableSpells.Select((p) => p.Key).ToList());

                // Get the spell
                SpellInfo info = availableSpells
                    .Where((p) => string.Equals(spellToConjure, p.Key, StringComparison.Ordinal))
                    .Select((p) => p.Value)
                    .Single();

                // Conjure the new spell and then cast it
                Conjure(info).Cast(this, other, output);
            }

            /// <summary>
            /// Casts the wizard's active spells on the specified player.
            /// </summary>
            /// <param name="other">The other player to cast the spells on.</param>
            /// <param name="output">A delegate to a method to use to output messages.</param>
            internal void CastSpells(Player other, Action<string, object[]>? output)
            {
                foreach (Spell spell in _spells.Where((p) => p.TurnsLeft > 0))
                {
                    spell.Cast(this, other, output);
                }
            }

            /// <summary>
            /// Conjures the specified spell.
            /// </summary>
            /// <param name="info">Information about the spell to conjure.</param>
            /// <returns>
            /// The spell that was conjured.
            /// </returns>
            internal Spell Conjure(SpellInfo info)
            {
                Mana -= info.Cost;
                ManaSpent += info.Cost;

                Spell spell = info.Conjure();

                _spells.Add(spell);

                return spell;
            }
        }

        /// <summary>
        /// A class representing a player.
        /// </summary>
        internal abstract class Player
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Player"/> class.
            /// </summary>
            /// <param name="name">The name of the player.</param>
            /// <param name="hitPoints">The number of hit points the player has.</param>
            /// <param name="damage">The initial damage the player inflicts on others.</param>
            internal Player(string name, int hitPoints, int damage)
            {
                Name = name;
                HitPoints = hitPoints;
                Damage = damage;
            }

            /// <summary>
            /// Gets or sets the armor of the player.
            /// </summary>
            public int Armor { get; set; }

            /// <summary>
            /// Gets or sets the damage the player inflicts on others.
            /// </summary>
            public int Damage { get; set; }

            /// <summary>
            /// Gets or sets the number of hit points the player has.
            /// </summary>
            public int HitPoints { get; set; }

            /// <summary>
            /// Gets the name of the player.
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Attacks the specified player.
            /// </summary>
            /// <param name="other">The other player to attack with this player.</param>
            /// <param name="output">A delegate to a method to use to output messages.</param>
            public abstract void Attack(Player other, Action<string, object[]>? output);
        }

        /// <summary>
        /// A class representing information about a spell. This class cannot be inherited.
        /// </summary>
        [DebuggerDisplay("{" + nameof(Name) + "} (" + nameof(Cost) + ")")]
        internal sealed class SpellInfo
        {
            /// <summary>
            /// Information about the spells that can be cast, their cost, and a method to conjure one. This field is read-only.
            /// </summary>
            internal static readonly IDictionary<string, SpellInfo> Spells = new Dictionary<string, SpellInfo>()
            {
                ["MagicMissile"] = new SpellInfo("MagicMissile", 53, () => new MagicMissile()),
                ["Drain"] = new SpellInfo("Drain", 73, () => new Drain()),
                ["Shield"] = new SpellInfo("Shield", 113, () => new Shield()),
                ["Poison"] = new SpellInfo("Poison", 173, () => new Poison()),
                ["Recharge"] = new SpellInfo("Recharge", 229, () => new Recharge()),
            };

            /// <summary>
            /// A delegate to a method to use to conjure the spell. This field is read-only.
            /// </summary>
            private readonly Func<Spell> _factory;

            /// <summary>
            /// Initializes a new instance of the <see cref="SpellInfo"/> class.
            /// </summary>
            /// <param name="name">The name of the spell.</param>
            /// <param name="cost">The cost of the spell.</param>
            /// <param name="conjure">A delegate to a method to use to conjure the spell.</param>
            private SpellInfo(string name, int cost, Func<Spell> conjure)
            {
                Name = name;
                Cost = cost;
                _factory = conjure;
            }

            /// <summary>
            /// Gets the cost of the spell in mana.
            /// </summary>
            internal int Cost { get; }

            /// <summary>
            /// Gets the name of the spell.
            /// </summary>
            internal string Name { get; }

            /// <summary>
            /// Conjures a new instance of the spell.
            /// </summary>
            /// <returns>
            /// The spell that was conjured.
            /// </returns>
            internal Spell Conjure() => _factory();
        }

        /// <summary>
        /// The base class for spells.
        /// </summary>
        internal abstract class Spell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Spell"/> class.
            /// </summary>
            /// <param name="turns">The number of turns the spell lasts for.</param>
            protected Spell(int turns)
            {
                Turns = TurnsLeft = turns;
            }

            /// <summary>
            /// Gets the name of the spell.
            /// </summary>
            internal string Name => GetType().Name;

            /// <summary>
            /// Gets the number of turns the spell can have an effect for.
            /// </summary>
            internal int Turns { get; }

            /// <summary>
            /// Gets the number of turns left where the spell can have an effect.
            /// </summary>
            internal int TurnsLeft { get; private set; }

            /// <summary>
            /// Gets or sets a value indicating whether the spell has been cast.
            /// </summary>
            private bool HasBeenCast { get; set; }

            /// <summary>
            /// Casts the spell on the specified player.
            /// </summary>
            /// <param name="wizard">The wizard casting the spell.</param>
            /// <param name="other">The player to cast the spell on.</param>
            /// <param name="output">A delegate to a method to use to output messages.</param>
            internal virtual void Cast(Wizard wizard, Player other, Action<string, object[]>? output)
            {
                if (!HasBeenCast)
                {
                    Cast(wizard, output);
                    Cast(other, output);

                    HasBeenCast = true;
                }
                else if (TurnsLeft > 0)
                {
                    Affect(wizard, output);
                    Affect(other, output);

                    TurnsLeft--;

                    if (TurnsLeft == 0)
                    {
                        Reverse(wizard, output);
                        Reverse(other, output);
                    }
                }
            }

            /// <summary>
            /// Applies the effect(s) of the spell on the specified player.
            /// </summary>
            /// <param name="other">The player to apply the effects of the spell on.</param>
            /// <param name="output">A delegate to a method to use to output messages.</param>
            protected virtual void Affect(Player other, Action<string, object[]>? output)
            {
            }

            /// <summary>
            /// Applies the effect(s) of the spell on the specified wizard.
            /// </summary>
            /// <param name="wizard">The wizard to apply the effects of the spell on.</param>
            /// <param name="output">A delegate to a method to use to output messages.</param>
            protected virtual void Affect(Wizard wizard, Action<string, object[]>? output)
            {
            }

            /// <summary>
            /// Casts the spell on the specified player.
            /// </summary>
            /// <param name="other">The player to cast the spell on.</param>
            /// <param name="output">A delegate to a method to use to output messages.</param>
            protected virtual void Cast(Player other, Action<string, object[]>? output)
            {
            }

            /// <summary>
            /// Casts the spell on the specified wizard.
            /// </summary>
            /// <param name="wizard">The wizard to cast the spell on.</param>
            /// <param name="output">A delegate to a method to use to output messages.</param>
            protected virtual void Cast(Wizard wizard, Action<string, object[]>? output)
            {
            }

            /// <summary>
            /// Reverses any effect(s) of the spell on the specified player.
            /// </summary>
            /// <param name="other">The player to reverse the effects of the spell on.</param>
            /// <param name="output">A delegate to a method to use to output messages.</param>
            protected virtual void Reverse(Player other, Action<string, object[]>? output)
            {
            }

            /// <summary>
            /// Reverses any effect(s) of the spell on the specified wizard.
            /// </summary>
            /// <param name="wizard">The wizard to reverse the effects of the spell on.</param>
            /// <param name="output">A delegate to a method to use to output messages.</param>
            protected virtual void Reverse(Wizard wizard, Action<string, object[]>? output)
            {
            }
        }

        /// <summary>
        /// A class representing the drain spell. This class cannot be inherited.
        /// </summary>
        private sealed class Drain : Spell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Drain"/> class.
            /// </summary>
            internal Drain()
                : base(0)
            {
            }

            /// <summary>
            /// Gets the amount of damage/healing the spell provides.
            /// </summary>
            private static int Delta => 2;

            /// <inheritdoc />
            protected override void Cast(Player other, Action<string, object[]>? output)
                => other.HitPoints -= Delta;

            /// <inheritdoc />
            protected override void Cast(Wizard wizard, Action<string, object[]>? output)
            {
                wizard.HitPoints += Delta;
                output?.Invoke("{0} casts Drain, dealing {1} damage, and healing {1} hit points.", new object[] { wizard.Name, Delta });
            }
        }

        /// <summary>
        /// A class representing the magic missile spell. This class cannot be inherited.
        /// </summary>
        private sealed class MagicMissile : Spell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MagicMissile"/> class.
            /// </summary>
            internal MagicMissile()
                : base(0)
            {
            }

            /// <summary>
            /// Gets the amount of damage the magic missile causes.
            /// </summary>
            private static int Damage => 4;

            /// <inheritdoc />
            protected override void Cast(Wizard wizard, Action<string, object[]>? output)
                => output?.Invoke("{0} casts {1}, dealing {2} damage.", new object[] { wizard.Name, Name, Damage });

            /// <inheritdoc />
            protected override void Cast(Player other, Action<string, object[]>? output)
                => other.HitPoints -= Damage;
        }

        /// <summary>
        /// A class representing the poison spell. This class cannot be inherited.
        /// </summary>
        private sealed class Poison : Spell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Poison"/> class.
            /// </summary>
            internal Poison()
                : base(6)
            {
            }

            /// <summary>
            /// Gets the amount of damage the poison causes.
            /// </summary>
            private static int Damage => 3;

            /// <inheritdoc />
            protected override void Cast(Wizard wizard, Action<string, object[]>? output)
                => output?.Invoke("{0} casts {1}.", new object[] { wizard.Name, Name });

            /// <inheritdoc />
            protected override void Affect(Player other, Action<string, object[]>? output)
            {
                other.HitPoints -= Damage;

                if (other.HitPoints < 1)
                {
#pragma warning disable CA1308
                    output?.Invoke("{0} deals {1} damage. This kills the {2}, and the player wins.", new object[] { Name, Damage, other.Name.ToLowerInvariant() });
#pragma warning restore CA1308
                }
                else
                {
                    output?.Invoke("{0} deals {1} damage; its timer is now {2}.", new object[] { Name, Damage, TurnsLeft - 1 });
                }
            }
        }

        /// <summary>
        /// A class representing the recharge spell. This class cannot be inherited.
        /// </summary>
        private sealed class Recharge : Spell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Recharge"/> class.
            /// </summary>
            internal Recharge()
                : base(5)
            {
            }

            /// <inheritdoc />
            protected override void Cast(Wizard wizard, Action<string, object[]>? output)
                => output?.Invoke("{0} casts {1}.", new object[] { wizard.Name, Name });

            /// <inheritdoc />
            protected override void Affect(Wizard wizard, Action<string, object[]>? output)
            {
                wizard.Mana += 101;
                output?.Invoke("{0} provides 101 mana; its timer is now {1}.", new object[] { Name, TurnsLeft - 1 });

                if (TurnsLeft - 1 == 0)
                {
                    output?.Invoke("{0} wears off.", new object[] { Name });
                }
            }
        }

        /// <summary>
        /// A class representing the shield spell. This class cannot be inherited.
        /// </summary>
        private sealed class Shield : Spell
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Shield"/> class.
            /// </summary>
            internal Shield()
                : base(6)
            {
            }

            /// <summary>
            /// Gets the amount of armor the shield provides.
            /// </summary>
            private static int ArmorIncrease => 7;

            /// <inheritdoc />
            protected override void Cast(Wizard wizard, Action<string, object[]>? output)
            {
                wizard.Armor += ArmorIncrease;
                output?.Invoke("{0} casts {1}, increasing armor by {2}.", new object[] { wizard.Name, Name, ArmorIncrease });
            }

            /// <inheritdoc />
            protected override void Affect(Wizard wizard, Action<string, object[]>? output)
                => output?.Invoke("{0}'s timer is now {1}.", new object[] { Name, TurnsLeft - 1 });

            /// <inheritdoc />
            protected override void Reverse(Wizard wizard, Action<string, object[]>? output)
            {
                wizard.Armor -= ArmorIncrease;

                if (wizard.Armor < 0)
                {
                    wizard.Armor = 0;
                }

                output?.Invoke("{0} wears off, decreasing armor by {1}.", new object[] { Name, ArmorIncrease });
            }
        }
    }
}
