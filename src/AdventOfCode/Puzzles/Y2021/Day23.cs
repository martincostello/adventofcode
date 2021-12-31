// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/23</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 23, "Amphipod", RequiresData = true)]
public sealed class Day23 : Puzzle
{
    private static readonly string EmptyBurrow = new(' ', 2);

    /// <summary>
    /// Gets the least energy required to organize the amphipods.
    /// </summary>
    public int MinimumEnergy { get; private set; }

    /// <summary>
    /// Organizes the specified amphipods into the correct burrows.
    /// </summary>
    /// <param name="diagram">The diagram of the burrows occupied by the amphipods.</param>
    /// <returns>
    /// The least energy required to organize the amphipods.
    /// </returns>
    public static int Organize(IList<string> diagram)
    {
        string amber = $"{diagram[2][3]}{diagram[3][3]}";
        string bronze = $"{diagram[2][5]}{diagram[3][5]}";
        string copper = $"{diagram[2][7]}{diagram[3][7]}";
        string desert = $"{diagram[2][9]}{diagram[3][9]}";

        var start = new State(new(' ', 11), amber, bronze, copper, desert);
        var goal = new State(new(' ', 11), "AA", "BB", "CC", "DD");
        var burrow = new Burrow();

        return (int)PathFinding.AStar(burrow, start, goal);
    }

    /// <summary>
    /// Gets the energy required to move between two states.
    /// </summary>
    /// <param name="x">The first state.</param>
    /// <param name="y">The second state.</param>
    /// <returns>
    /// The amount of energy required to move between the two states.
    /// </returns>
    internal static int Cost(
        (string Hallway, string Amber, string Bronze, string Copper, string Desert) x,
        (string Hallway, string Amber, string Bronze, string Copper, string Desert) y)
    {
        var a = new State(x.Hallway, x.Amber, x.Bronze, x.Copper, x.Desert);
        var b = new State(y.Hallway, y.Amber, y.Bronze, y.Copper, y.Desert);
        var burrow = new Burrow();

        return (int)burrow.Cost(a, b);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> diagram = await ReadResourceAsLinesAsync();

        MinimumEnergy = Organize(diagram);

        if (Verbose)
        {
            Logger.WriteLine("The least energy required to organize the amphipods is {0:N0}.", MinimumEnergy);
        }

        return PuzzleResult.Create(MinimumEnergy);
    }

    private record struct State(string Hallway, string Amber, string Bronze, string Copper, string Desert)
    {
        public static int Entrance(char burrow) => burrow switch
        {
            'A' => 2,
            'B' => 4,
            'C' => 6,
            'D' => 8,
            _ => throw new NotImplementedException(),
        };

        public string Burrow(char burrow) => burrow switch
        {
            'A' => Amber,
            'B' => Bronze,
            'C' => Copper,
            'D' => Desert,
            _ => throw new NotImplementedException(),
        };

        public bool HasSpace(char burrow) => Burrow(burrow).Contains(' ', StringComparison.Ordinal);

        public bool IsEmpty(char burrow) => string.IsNullOrWhiteSpace(Burrow(burrow));

        public bool IsOrganized(char burrow) => Burrow(burrow) == new string(burrow, 2);

        public bool IsPathClear(char sourceBurrow, int destinationBurrow, bool occupied)
            => IsPathClear(Entrance(sourceBurrow), destinationBurrow, occupied);

        public bool IsPathClear(int sourceBurrow, int destinationBurrow, bool occupied)
        {
            int startIndex = Math.Min(sourceBurrow, destinationBurrow);
            int endIndex = Math.Max(sourceBurrow, destinationBurrow);
            int length = Math.Abs(endIndex - startIndex);

            if (occupied)
            {
                length--;
            }

            return string.IsNullOrWhiteSpace(Hallway.Substring(startIndex, length));
        }

        public override string ToString()
        {
            var builder = new StringBuilder()
                .AppendLine("#############")
                .Append('#')
                .Append(Hallway.Replace(' ', '.'))
                .Append('#')
                .AppendLine()
                .Append("###")
                .Append(Amber[0])
                .Append('#')
                .Append(Bronze[0])
                .Append('#')
                .Append(Copper[0])
                .Append('#')
                .Append(Desert[0])
                .AppendLine("###")
                .Append("  #")
                .Append(Amber[1])
                .Append('#')
                .Append(Bronze[1])
                .Append('#')
                .Append(Copper[1])
                .Append('#')
                .Append(Desert[1])
                .AppendLine("#  ")
                .AppendLine("  #########  ");

            return builder.ToString();
        }
    }

    private sealed class Burrow : IWeightedGraph<State>
    {
        private static readonly char[] Amphipods = { 'A', 'B', 'C', 'D' };
        private static readonly int[] HallwaySpaces = { 0, 1, 3, 5, 7, 9, 10 };

        public long Cost(State a, State b)
        {
            if (a.Equals(b))
            {
                return 0;
            }

            int differences = !string.Equals(a.Hallway, b.Hallway, StringComparison.Ordinal) ? 1 : 0;
            differences += !string.Equals(a.Amber, b.Amber, StringComparison.Ordinal) ? 1 : 0;
            differences += !string.Equals(a.Bronze, b.Bronze, StringComparison.Ordinal) ? 1 : 0;
            differences += !string.Equals(a.Copper, b.Copper, StringComparison.Ordinal) ? 1 : 0;
            differences += !string.Equals(a.Desert, b.Desert, StringComparison.Ordinal) ? 1 : 0;

#pragma warning disable CA1508
            if (differences != 2)
#pragma warning restore CA1508
            {
                return int.MaxValue;
            }

            char moved = default;
            int index = -1;

            for (int i = 0; i < a.Hallway.Length; i++)
            {
                char hallwayA = a.Hallway[i];
                char hallwayB = b.Hallway[i];

                if (hallwayA != hallwayB)
                {
                    index = i;
                    moved = hallwayA == ' ' ? hallwayB : hallwayA;
                    break;
                }
            }

            char burrowChanged;
            string burrowBefore;
            string burrowAfter;

            if (moved == default)
            {
                char from;
                char to;

                if (!string.Equals(a.Amber, b.Amber, StringComparison.Ordinal))
                {
                    from = 'A';

                    if (!string.Equals(a.Bronze, b.Bronze, StringComparison.Ordinal))
                    {
                        to = 'B';
                    }
                    else if (!string.Equals(a.Copper, b.Copper, StringComparison.Ordinal))
                    {
                        to = 'C';
                    }
                    else
                    {
                        to = 'D';
                    }
                }
                else if (!string.Equals(a.Bronze, b.Bronze, StringComparison.Ordinal))
                {
                    from = 'B';

                    if (!string.Equals(a.Amber, b.Amber, StringComparison.Ordinal))
                    {
                        to = 'A';
                    }
                    else if (!string.Equals(a.Copper, b.Copper, StringComparison.Ordinal))
                    {
                        to = 'C';
                    }
                    else
                    {
                        to = 'D';
                    }
                }
                else if (!string.Equals(a.Copper, b.Copper, StringComparison.Ordinal))
                {
                    from = 'C';

                    if (!string.Equals(a.Amber, b.Amber, StringComparison.Ordinal))
                    {
                        to = 'A';
                    }
                    else if (!string.Equals(a.Bronze, b.Bronze, StringComparison.Ordinal))
                    {
                        to = 'B';
                    }
                    else
                    {
                        to = 'D';
                    }
                }
                else
                {
                    from = 'D';

                    if (!string.Equals(a.Amber, b.Amber, StringComparison.Ordinal))
                    {
                        to = 'A';
                    }
                    else if (!string.Equals(a.Bronze, b.Bronze, StringComparison.Ordinal))
                    {
                        to = 'B';
                    }
                    else
                    {
                        to = 'C';
                    }
                }

                index = State.Entrance(from);
                burrowChanged = to;
                moved = to;

                burrowBefore = a.Burrow(to);
                burrowAfter = b.Burrow(to);

                int steps = Math.Abs(State.Entrance(burrowChanged) - index);

                if (burrowAfter[0] == ' ')
                {
                    steps += burrowAfter[1] != moved ? 1 : 2;
                }
                else
                {
                    steps += burrowBefore[1] == moved ? 1 : 2;
                }

                burrowBefore = a.Burrow(from);
                burrowAfter = b.Burrow(from);

                if (burrowBefore[0] == ' ')
                {
                    steps += burrowAfter[1] != moved ? 1 : 2;
                }
                else
                {
                    steps += burrowBefore[0] == moved ? 1 : 2;
                }

                int multiplier = Multiplier(moved);

                return steps * multiplier;
            }
            else
            {
                burrowBefore = a.Burrow(moved);
                burrowAfter = b.Burrow(moved);

                burrowChanged = moved;

                if (burrowBefore == burrowAfter)
                {
                    if (!string.Equals(a.Amber, b.Amber, StringComparison.Ordinal))
                    {
                        burrowChanged = 'A';
                        burrowBefore = a.Amber;
                        burrowAfter = b.Amber;
                    }
                    else if (!string.Equals(a.Bronze, b.Bronze, StringComparison.Ordinal))
                    {
                        burrowChanged = 'B';
                        burrowBefore = a.Bronze;
                        burrowAfter = b.Bronze;
                    }
                    else if (!string.Equals(a.Copper, b.Copper, StringComparison.Ordinal))
                    {
                        burrowChanged = 'C';
                        burrowBefore = a.Copper;
                        burrowAfter = b.Copper;
                    }
                    else
                    {
                        burrowChanged = 'D';
                        burrowBefore = a.Desert;
                        burrowAfter = b.Desert;
                    }
                }

                int steps = Math.Abs(State.Entrance(burrowChanged) - index);

                steps += burrowAfter.Count((p) => p == ' ') switch
                {
                    0 => 1,
                    1 => burrowBefore.Count((p) => p == ' ') switch
                    {
                        0 => 1,
                        1 => 2,
                        2 => 2,
                        _ => throw new InvalidOperationException(),
                    },
                    2 => 2,
                    _ => throw new InvalidOperationException(),
                };

                int multiplier = Multiplier(moved);

                return steps * multiplier;
            }

            static int Multiplier(char amphipod) => (int)Math.Pow(10, amphipod - 'A');
        }

        public IEnumerable<State> Neighbors(State id)
        {
            var result = new List<State>();

            // Move any amphipods from their burrow to the hallway
            foreach (char amphipod in Amphipods)
            {
                if (id.IsOrganized(amphipod) || id.IsEmpty(amphipod))
                {
                    continue;
                }

                string burrow = id.Burrow(amphipod);

                if (burrow[0] == ' ' && burrow[1] == amphipod)
                {
                    // Already in the right place
                    continue;
                }

                if (!TryVacate(burrow, out char moved, out string? vacated))
                {
                    continue;
                }

                int burrowEntrance = State.Entrance(amphipod);

                foreach (int space in HallwaySpaces.OrderBy((p) => Math.Abs(burrowEntrance - p)))
                {
                    if (!id.IsPathClear(amphipod, space, occupied: false))
                    {
                        // Something is blocking the hallway
                        continue;
                    }

                    string newHallway = Move(id.Hallway, moved, space);

                    result.Add(amphipod switch
                    {
                        'A' => new(newHallway, vacated, id.Bronze, id.Copper, id.Desert),
                        'B' => new(newHallway, id.Amber, vacated, id.Copper, id.Desert),
                        'C' => new(newHallway, id.Amber, id.Bronze, vacated, id.Desert),
                        'D' => new(newHallway, id.Amber, id.Bronze, id.Copper, vacated),
                        _ => throw new InvalidOperationException(),
                    });
                }
            }

            // Move any amphipods from the hallway to their burrow
            foreach (int space in HallwaySpaces)
            {
                char amphipod = id.Hallway[space];

                if (amphipod == ' ' ||
                    !id.HasSpace(amphipod) ||
                    !id.IsPathClear(space, State.Entrance(amphipod), occupied: true))
                {
                    continue;
                }

                string burrow = id.Burrow(amphipod);

                if (burrow[0] == ' ' && burrow[1] != ' ' && burrow[1] != amphipod)
                {
                    continue;
                }

                string newHallway = Move(id.Hallway, ' ', space);
                string populated = string.IsNullOrWhiteSpace(burrow) ? " " + amphipod : new string(amphipod, 2);

                result.Add(amphipod switch
                {
                    'A' => new(newHallway, populated, id.Bronze, id.Copper, id.Desert),
                    'B' => new(newHallway, id.Amber, populated, id.Copper, id.Desert),
                    'C' => new(newHallway, id.Amber, id.Bronze, populated, id.Desert),
                    'D' => new(newHallway, id.Amber, id.Bronze, id.Copper, populated),
                    _ => throw new InvalidOperationException(),
                });
            }

            return result.Distinct().ToList();
        }

        private static string Move(string hallway, char moved, int index)
        {
            string result = hallway.Remove(index, 1);
            return result.Insert(index, char.ToString(moved));
        }

        private static bool TryVacate(string burrow, out char moved, [NotNullWhen(true)] out string? vacated)
        {
            moved = default;
            vacated = default;

            if (string.IsNullOrWhiteSpace(burrow))
            {
                return false;
            }

            char top = burrow[0];
            char bottom = burrow[1];

            bool isTopEmpty = top == ' ';

            moved = isTopEmpty ? bottom : top;
            vacated = isTopEmpty ? EmptyBurrow : (" " + bottom);

            return true;
        }
    }
}
