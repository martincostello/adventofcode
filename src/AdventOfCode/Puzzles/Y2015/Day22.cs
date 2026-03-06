// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2015;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2015/day/22</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2015, 22, "Wizard Simulator 20XX")]
public sealed class Day22 : Puzzle<int, int>
{
    /// <summary>
    /// Simulates a fight between the wizard and the boss.
    /// </summary>
    /// <param name="difficulty">The difficulty to play with.</param>
    /// <returns>
    /// The minimum amount of mana that can be spent for the wizard to win.
    /// </returns>
    public static int Fight(string difficulty)
    {
        bool isHard = string.Equals("HARD", difficulty, StringComparison.OrdinalIgnoreCase);

        // Use Dijkstra to find the minimum mana that can be spent for the wizard to win.
        // State: (wizardHP, wizardMana, bossHP, shieldTimer, poisonTimer, rechargeTimer, isWizardsTurn)
        var queue = new PriorityQueue<(int WizardHP, int WizardMana, int BossHP, int ShieldT, int PoisonT, int RechargeT, bool IsWizardsTurn), int>();
        var visited = new HashSet<(int, int, int, int, int, int, bool)>();

        const int InitialWizardHP = 50;
        const int InitialWizardMana = 500;
        const int InitialBossHP = 55;

        const int BossDamage = 8;

        const int WizardArmor = 7;
        const int WizardPenaltyHP = 1;
        const int WizardManaRecharge = 101;

        const int MagicMissileCost = 53;
        const int MagicMissileDamage = 4;

        const int DrainCost = 73;
        const int DrainDamage = 2;
        const int DrainHeal = 2;

        const int ShieldCost = 113;
        const int ShieldTurns = 6;

        const int PoisonCost = 173;
        const int PoisonDamage = 3;
        const int PoisonTurns = 6;

        const int RechargeCost = 229;
        const int RechargeTurns = 5;

        queue.Enqueue((InitialWizardHP, InitialWizardMana, InitialBossHP, 0, 0, 0, true), 0);

        while (queue.TryDequeue(out var state, out int manaSpent))
        {
            var (wizardHP, wizardMana, bossHP, shieldT, poisonT, rechargeT, isWizardsTurn) = state;

            if (!visited.Add((wizardHP, wizardMana, bossHP, shieldT, poisonT, rechargeT, isWizardsTurn)))
            {
                continue;
            }

            // Hard mode: wizard loses HP at the start of their turn
            if (isHard && isWizardsTurn)
            {
                wizardHP -= WizardPenaltyHP;

                if (wizardHP < 1)
                {
                    continue;
                }
            }

            // Apply effects and update timers at the start of this turn
            int wizardArmor = shieldT > 0 ? WizardArmor : 0;

            if (shieldT > 0)
            {
                shieldT--;
            }

            if (poisonT > 0)
            {
                bossHP -= PoisonDamage;
                poisonT--;
            }

            if (rechargeT > 0)
            {
                wizardMana += WizardManaRecharge;
                rechargeT--;
            }

            // Check if boss died from effects
            if (bossHP <= 0)
            {
                return manaSpent;
            }

            // If wizard's turn, try casting each available spell
            if (isWizardsTurn)
            {
                // Magic Missile
                if (wizardMana >= MagicMissileCost)
                {
                    int newBossHP = bossHP - MagicMissileDamage;

                    if (newBossHP <= 0)
                    {
                        return manaSpent + MagicMissileCost;
                    }

                    queue.Enqueue((wizardHP, wizardMana - MagicMissileCost, newBossHP, shieldT, poisonT, rechargeT, false), manaSpent + MagicMissileCost);
                }

                // Drain
                if (wizardMana >= DrainCost)
                {
                    int newBossHP = bossHP - DrainDamage;

                    if (newBossHP <= 0)
                    {
                        return manaSpent + DrainCost;
                    }

                    queue.Enqueue((wizardHP + DrainHeal, wizardMana - DrainCost, newBossHP, shieldT, poisonT, rechargeT, false), manaSpent + DrainCost);
                }

                // Shield
                if (wizardMana >= ShieldCost && shieldT == 0)
                {
                    queue.Enqueue((wizardHP, wizardMana - ShieldCost, bossHP, ShieldTurns, poisonT, rechargeT, false), manaSpent + ShieldCost);
                }

                // Poison
                if (wizardMana >= PoisonCost && poisonT == 0)
                {
                    queue.Enqueue((wizardHP, wizardMana - PoisonCost, bossHP, shieldT, PoisonTurns, rechargeT, false), manaSpent + PoisonCost);
                }

                // Recharge
                if (wizardMana >= RechargeCost && rechargeT == 0)
                {
                    queue.Enqueue((wizardHP, wizardMana - RechargeCost, bossHP, shieldT, poisonT, RechargeTurns, false), manaSpent + RechargeCost);
                }
            }
            else
            {
                // Boss's turn: boss attacks wizard
                int damage = Math.Max(1, BossDamage - wizardArmor);
                int newWizHP = wizardHP - damage;

                if (newWizHP > 0)
                {
                    queue.Enqueue((newWizHP, wizardMana, bossHP, shieldT, poisonT, rechargeT, true), manaSpent);
                }
            }
        }

        return int.MaxValue; // No winning path found
    }

    /// <inheritdoc />
    protected override Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        Solution1 = Fight("easy");
        Solution2 = Fight("hard");

        if (Verbose)
        {
            Logger.WriteLine("The minimum amount of mana that can be spent to win on easy difficulty is {0:N0}.", Solution1);
            Logger.WriteLine("The minimum amount of mana that can be spent to win on hard difficulty is {0:N0}.", Solution2);
        }

        return PuzzleResult.Create(Solution1, Solution2);
    }
}
