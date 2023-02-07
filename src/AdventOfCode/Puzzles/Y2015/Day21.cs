// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/21</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 21, "RPG Simulator 20XX", RequiresData = true)]
public sealed class Day21 : Puzzle
{
    /// <summary>
    /// Gets the maximum cost of items that can be purchased for the human to lost to the boss.
    /// </summary>
    internal int MaximumCostToLose { get; private set; }

    /// <summary>
    /// Gets the minimum cost of items that can be purchased for the human to beat the boss.
    /// </summary>
    internal int MinimumCostToWin { get; private set; }

    /// <summary>
    /// Simulates a fight between the player and the boss using the specified upgrades.
    /// </summary>
    /// <param name="weapon">The weapon to purchase.</param>
    /// <param name="armor">The armor to purchase, if any.</param>
    /// <param name="rings">The rings to purchase, if any.</param>
    /// <param name="bossStats">The boss' stats.</param>
    /// <returns>
    /// A named tuple that returns whether the human player won and the amount of gold spent.
    /// </returns>
    internal static (bool DidHumanWin, int GoldSpent) Fight(
        string weapon,
        string? armor,
        ICollection<string>? rings,
        (int HitPoints, int Damage, int Armor) bossStats)
    {
        var shop = new Shop();
        var human = new Human();

        int goldSpent = human.Upgrade(shop.PurchaseWeapon(weapon));

        if (!string.IsNullOrEmpty(armor))
        {
            goldSpent += human.Upgrade(shop.PurchaseArmor(armor));
        }

        foreach (string ring in rings!.Distinct().Take(2))
        {
            goldSpent += human.Upgrade(shop.PurchaseRing(ring));
        }

        var boss = new Player(bossStats.HitPoints, bossStats.Damage, bossStats.Armor);

        Player winner = Fight(human, boss);

        return (winner == human, goldSpent);
    }

    /// <summary>
    /// Simulates a fight between the specified players.
    /// </summary>
    /// <param name="player1">The first player.</param>
    /// <param name="player2">The second player.</param>
    /// <returns>
    /// The player which won the fight.
    /// </returns>
    internal static Player Fight(Player player1, Player player2)
    {
        Player attacker = player1;
        Player defender = player2;

        while (attacker.HitPoints > 0 && defender.HitPoints > 0)
        {
            defender.AcceptAttack(attacker);
            (attacker, defender) = (defender, attacker);
        }

        return player1.HitPoints > 0 ? player1 : player2;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> stats = await ReadResourceAsLinesAsync(cancellationToken);

        int bossHitPoints = Parse<int>(stats[0].Split(':')[1]);
        int bossDamage = Parse<int>(stats[1].Split(':')[1]);
        int bossArmor = Parse<int>(stats[2].Split(':')[1]);

        var bossStats = (bossHitPoints, bossDamage, bossArmor);

        string[] potentialWeapons = Shop.PotentialWeapons.Keys.ToArray();
        string?[] potentialArmor = Shop.PotentialArmor.Keys.Append(null!).ToArray();

        ICollection<string> keys = Shop.PotentialRings.Keys.ToArray();

        var potentialRings = new List<IList<string>>((keys.Count * 2) + 1)
        {
            Array.Empty<string>(),
        };

        foreach (string ring in keys)
        {
            potentialRings.Add(new[] { ring });
        }

        var permutations = Maths.GetPermutations(keys, 2);

        foreach (var permutation in permutations)
        {
            potentialRings.Add(permutation.ToArray());
        }

        var costsToLose = new List<int>((potentialArmor.Length + potentialRings.Count + potentialWeapons.Length) / 2);
        var costsToWin = new List<int>(costsToLose.Capacity);

        foreach (string weapon in potentialWeapons)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            foreach (string? armor in potentialArmor)
            {
                foreach (var rings in potentialRings)
                {
                    (bool didHumanWin, int goldSpent) = Fight(weapon, armor, rings, bossStats);

                    if (didHumanWin)
                    {
                        costsToWin.Add(goldSpent);
                    }
                    else
                    {
                        costsToLose.Add(goldSpent);
                    }
                }
            }
        }

        MaximumCostToLose = costsToLose.Max();
        MinimumCostToWin = costsToWin.Min();

        if (Verbose)
        {
            Logger.WriteLine(
                "The minimum amount of gold spent for the human to beat the boss is {0:N0}.",
                MinimumCostToWin);

            Logger.WriteLine(
                "The maximum amount of gold spent for the human to lose to the boss is {0:N0}.",
                MaximumCostToLose);
        }

        return PuzzleResult.Create(MaximumCostToLose, MinimumCostToWin);
    }

    /// <summary>
    /// A class representing a player.
    /// </summary>
    internal class Player
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="hitPoints">The initial number of hit points.</param>
        /// <param name="damage">The amount of damage.</param>
        /// <param name="armor">The amount of armor.</param>
        internal Player(int hitPoints, int damage, int armor)
        {
            HitPoints = hitPoints;
            Damage = damage;
            Armor = armor;
        }

        /// <summary>
        /// Gets or sets the entity's current number of hit points.
        /// </summary>
        public int HitPoints { get; protected set; }

        /// <summary>
        /// Gets or sets the damage the entity inflicts on others.
        /// </summary>
        public int Damage { get; protected set; }

        /// <summary>
        /// Gets or sets the armor of the entity.
        /// </summary>
        public int Armor { get; protected set; }

        /// <summary>
        /// Accepts an attack from the specified entity.
        /// </summary>
        /// <param name="other">The other entity that attacks this entity.</param>
        public void AcceptAttack(Player other)
        {
            int damage = other.Damage - Armor;

            if (damage < 1)
            {
                damage = 1;
            }

            HitPoints -= damage;
        }
    }

    /// <summary>
    /// A class representing the human player. This class cannot be inherited.
    /// </summary>
    private sealed class Human : Player
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Human"/> class.
        /// </summary>
        internal Human()
            : base(100, 0, 0)
        {
        }

        /// <summary>
        /// Upgrades the human using the specified item.
        /// </summary>
        /// <param name="item">The item to upgrade the human with.</param>
        /// <returns>
        /// The cost of the upgrade.
        /// </returns>
        public int Upgrade(Item item)
        {
            Damage += item.Damage;
            Armor += item.Armor;
            return item.Cost;
        }
    }

    /// <summary>
    /// A class representing an item. This class cannot be inherited.
    /// </summary>
    private sealed class Item
    {
        /// <summary>
        /// Gets or sets the cost of the item.
        /// </summary>
        internal int Cost { get; set; }

        /// <summary>
        /// Gets or sets the amount of extra damage an entity can inflict with the item.
        /// </summary>
        internal int Damage { get; set; }

        /// <summary>
        /// Gets or sets the amount of extra armor an entity can have with the item.
        /// </summary>
        internal int Armor { get; set; }
    }

    /// <summary>
    /// A class representing the shop. This class cannot be inherited.
    /// </summary>
    private sealed class Shop
    {
        /// <summary>
        /// The armor inventory. This field is read-only.
        /// </summary>
        internal static readonly Dictionary<string, Item> PotentialArmor = new(5)
        {
            ["Leather"] = new() { Cost = 13, Armor = 1 },
            ["Chainmail"] = new() { Cost = 31, Armor = 2 },
            ["Splintmail"] = new() { Cost = 53, Armor = 3 },
            ["Bandedmail"] = new() { Cost = 75, Armor = 4 },
            ["Platemail"] = new() { Cost = 102, Armor = 5 },
        };

        /// <summary>
        /// The ring inventory. This field is read-only.
        /// </summary>
        internal static readonly Dictionary<string, Item> PotentialRings = new(6)
        {
            ["Damage +1"] = new() { Cost = 25, Damage = 1 },
            ["Damage +2"] = new() { Cost = 50, Damage = 2 },
            ["Damage +3"] = new() { Cost = 100, Damage = 3 },
            ["Defense +1"] = new() { Cost = 20, Armor = 1 },
            ["Defense +2"] = new() { Cost = 40, Armor = 2 },
            ["Defense +3"] = new() { Cost = 80, Armor = 3 },
        };

        /// <summary>
        /// The weapon inventory. This field is read-only.
        /// </summary>
        internal static readonly Dictionary<string, Item> PotentialWeapons = new(5)
        {
            ["Dagger"] = new() { Cost = 8, Damage = 4 },
            ["Shortsword"] = new() { Cost = 10, Damage = 5 },
            ["Warhammer"] = new() { Cost = 25, Damage = 6 },
            ["Longsword"] = new() { Cost = 40, Damage = 7 },
            ["Greataxe"] = new() { Cost = 74, Damage = 8 },
        };

        /// <summary>
        /// The shop's current armor inventory. This field is read-only.
        /// </summary>
        private readonly Dictionary<string, Item> _armor = new(PotentialArmor);

        /// <summary>
        /// The shop's current ring inventory. This field is read-only.
        /// </summary>
        private readonly Dictionary<string, Item> _rings = new(PotentialRings);

        /// <summary>
        /// The shop's current weapon inventory. This field is read-only.
        /// </summary>
        private readonly Dictionary<string, Item> _weapons = new(PotentialWeapons);

        /// <summary>
        /// Purchases the specified armor.
        /// </summary>
        /// <param name="name">The name of the armor to purchase.</param>
        /// <returns>
        /// The purchased armor.
        /// </returns>
        public Item PurchaseArmor(string name)
        {
            Item item = _armor[name];
            _armor.Clear();
            return item;
        }

        /// <summary>
        /// Purchases the specified ring.
        /// </summary>
        /// <param name="name">The name of the ring to purchase.</param>
        /// <returns>
        /// The purchased ring.
        /// </returns>
        public Item PurchaseRing(string name)
        {
            Item item = _rings[name];

            if (_rings.Count < 5)
            {
                _rings.Clear();
            }

            return item;
        }

        /// <summary>
        /// Purchases the specified weapon.
        /// </summary>
        /// <param name="name">The name of the weapon to purchase.</param>
        /// <returns>
        /// The purchased armor.
        /// </returns>
        public Item PurchaseWeapon(string name)
        {
            Item item = _weapons[name];
            _weapons.Clear();
            return item;
        }
    }
}
