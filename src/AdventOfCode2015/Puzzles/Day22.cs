// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode2015.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// A class representing the puzzle for <c>http://adventofcode.com/day/22</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Day22 : IPuzzle
    {
        /// <summary>
        /// Gets the minimum amount of mana that can be spent to win.
        /// </summary>
        internal int MinimumCostToWin { get; private set; }

        /// <inheritdoc />
        public int Solve(string[] args)
        {
            List<Tuple<bool, int>> solutions = new List<Tuple<bool, int>>();
            Random random = new Random();

            // Play the game 10,000 times with random choices of spells
            while (solutions.Count < 100000)
            {
                Tuple<bool, int> result = Fight((wizard, spells) => spells.ElementAt(random.Next(0, spells.Count)));
                solutions.Add(Tuple.Create(result.Item1, result.Item2));
            }

            MinimumCostToWin = solutions
                .Where((p) => p.Item1)
                .Min((p) => p.Item2);

            Console.WriteLine("The minimum amount of mana that can be spent to win is {0:N0}.", MinimumCostToWin);

            return 0;
        }

        /// <summary>
        /// Simulates a fight between the wizard and the boss.
        /// </summary>
        /// <param name="spellSelector">A delegate to a method to use to select the next spell to conjure.</param>
        /// <returns>
        /// A <see cref="Tuple{T1, T2}"/> that returns whether the wizard won and the amount of mana spent by the wizard.
        /// </returns>
        internal static Tuple<bool, int> Fight(Func<Wizard, ICollection<string>, string> spellSelector)
        {
            Wizard wizard = new Wizard(spellSelector);
            Boss boss = new Boss();

            Player winner = Fight(wizard, boss);

            return Tuple.Create(winner == wizard, wizard.ManaSpent);
        }

        /// <summary>
        /// Simulates a fight between the specified players.
        /// </summary>
        /// <param name="player1">The first player.</param>
        /// <param name="player2">The second player.</param>
        /// <returns>
        /// The player which won the fight.
        /// </returns>
        internal static Player Fight(Wizard player1, Player player2)
        {
            Player attacker = player1;
            Player defender = player2;

            do
            {
                if (attacker != player1)
                {
                    player1.CastSpells(attacker);
                }

                if (attacker.HitPoints > 0)
                {
                    attacker.Attack(defender);
                }

                var player = attacker;
                attacker = defender;
                defender = player;
            }
            while (player1.HitPoints > 0 && player2.HitPoints > 0);

            return player1.HitPoints > 0 ? player1 : player2;
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
                : base(hitPoints, damage)
            {
            }

            /// <summary>
            /// Attacks the specified player.
            /// </summary>
            /// <param name="other">The other player to attack with this player.</param>
            public override void Attack(Player other)
            {
                int damage = Damage - other.Armor;

                if (damage < 1)
                {
                    damage = 1;
                }

                other.HitPoints -= damage;
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
                : base(hitPoints, 0)
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
            public override void Attack(Player other)
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
                var info = availableSpells
                    .Where((p) => string.Equals(spellToConjure, p.Key))
                    .Select((p) => p.Value)
                    .Single();

                // Equip it
                Equip(info);

                // Cast any available spells to apply their effects
                CastSpells(other);
            }

            /// <summary>
            /// Casts the wizard's active spells on the specified player.
            /// </summary>
            /// <param name="other">The other player to cast the spells on.</param>
            internal void CastSpells(Player other)
            {
                foreach (var spell in _spells.Where((p) => p.TurnsLeft > 0))
                {
                    spell.Cast(this, other);
                }
            }

            /// <summary>
            /// Equips the wizard with the specified spell.
            /// </summary>
            /// <param name="info">Information about the spell to equip the wizard with.</param>
            /// <returns>
            /// The spell that was equipped.
            /// </returns>
            internal Spell Equip(SpellInfo info)
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
            /// <param name="hitPoints">The number of hit points the player has.</param>
            /// <param name="damage">The initial damage the player inflicts on others.</param>
            internal Player(int hitPoints, int damage)
            {
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
            /// Attacks the specified player.
            /// </summary>
            /// <param name="other">The other player to attack with this player.</param>
            public abstract void Attack(Player other);
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
                { "MagicMissile", new SpellInfo("MagicMissile", 53, () => new MagicMissile()) },
                { "Drain", new SpellInfo("Drain", 73, () => new Drain()) },
                { "Shield", new SpellInfo("Shield", 113, () => new Shield()) },
                { "Poison", new SpellInfo("Poison", 173, () => new Poison()) },
                { "Recharge", new SpellInfo("Recharge", 229, () => new Recharge()) },
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
            /// Casts the spell on the specified player.
            /// </summary>
            /// <param name="wizard">The wizard casting the spell.</param>
            /// <param name="other">The player to cast the spell on.</param>
            internal virtual void Cast(Wizard wizard, Player other)
            {
                if (TurnsLeft > 0)
                {
                    Affect(wizard);
                    Affect(other);

                    TurnsLeft--;
                }
                else
                {
                    Reset(wizard);
                }
            }

            /// <summary>
            /// Applies the effect(s) of the spell on the specified player.
            /// </summary>
            /// <param name="player">The player to apply the effects of the spell on.</param>
            protected virtual void Affect(Player player)
            {
            }

            /// <summary>
            /// Applies the effect(s) of the spell on the specified wizard.
            /// </summary>
            /// <param name="wizard">The wizard to apply the effects of the spell on.</param>
            protected virtual void Affect(Wizard wizard)
            {
            }

            /// <summary>
            /// Resets any effect(s) of the spell on the specified wizard.
            /// </summary>
            /// <param name="wizard">The wizard to reset the effects of the spell on.</param>
            protected virtual void Reset(Wizard wizard)
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
                : base(1)
            {
            }

            /// <inheritdoc />
            protected override void Affect(Player player)
            {
                player.HitPoints -= 2;
            }

            /// <inheritdoc />
            protected override void Affect(Wizard wizard)
            {
                wizard.HitPoints += 2;
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
                : base(1)
            {
            }

            /// <inheritdoc />
            protected override void Affect(Player player)
            {
                player.HitPoints -= 4;
            }
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

            /// <inheritdoc />
            protected override void Affect(Player player)
            {
                player.HitPoints -= 3;
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
            protected override void Affect(Wizard wizard)
            {
                wizard.Mana += 101;
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

            /// <inheritdoc />
            protected override void Affect(Wizard wizard)
            {
                if (Turns == TurnsLeft)
                {
                    wizard.Armor += 7;
                }
            }

            /// <inheritdoc />
            protected override void Reset(Wizard wizard)
            {
                wizard.Armor -= 7;

                if (wizard.Armor < 0)
                {
                    wizard.Armor = 0;
                }
            }
        }
    }
}
