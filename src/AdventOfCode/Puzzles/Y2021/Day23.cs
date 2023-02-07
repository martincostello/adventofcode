// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace MartinCostello.AdventOfCode.Puzzles.Y2021;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2021/day/23</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2021, 23, "Amphipod", RequiresData = true, IsSlow = true)]
public sealed class Day23 : Puzzle
{
    /// <summary>
    /// Gets the least energy required to organize the amphipods with the diagram folded.
    /// </summary>
    public int MinimumEnergyFolded { get; private set; }

    /// <summary>
    /// Gets the least energy required to organize the amphipods with the diagram unfolded.
    /// </summary>
    public int MinimumEnergyUnfolded { get; private set; }

    /// <summary>
    /// Organizes the specified amphipods into the correct burrows.
    /// </summary>
    /// <param name="diagram">The diagram of the burrows occupied by the amphipods.</param>
    /// <param name="unfoldDiagram">Whether to unfold the diagram to reveal the missing lines.</param>
    /// <param name="cancellationToken">The optional <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The least energy required to organize the amphipods.
    /// </returns>
    public static int Organize(
        IList<string> diagram,
        bool unfoldDiagram,
        CancellationToken cancellationToken = default)
    {
        string amber = $"{diagram[2][3]}{diagram[3][3]}";
        string bronze = $"{diagram[2][5]}{diagram[3][5]}";
        string copper = $"{diagram[2][7]}{diagram[3][7]}";
        string desert = $"{diagram[2][9]}{diagram[3][9]}";

        if (unfoldDiagram)
        {
            amber = amber.Insert(1, "DD");
            bronze = bronze.Insert(1, "CB");
            copper = copper.Insert(1, "BA");
            desert = desert.Insert(1, "AC");
        }

        string emptyHallway = new(' ', 11);
        int depth = unfoldDiagram ? 4 : 2;

        var start = new State(emptyHallway, amber, bronze, copper, desert);
        var goal = new State(emptyHallway, new('A', depth), new('B', depth), new('C', depth), new('D', depth));

        return (int)PathFinding.AStar(new Burrow(), start, goal, cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> diagram = await ReadResourceAsLinesAsync(cancellationToken);

        MinimumEnergyFolded = Organize(diagram, unfoldDiagram: false, cancellationToken);
        MinimumEnergyUnfolded = Organize(diagram, unfoldDiagram: true, cancellationToken);

        if (Verbose)
        {
            Logger.WriteLine("The least energy required to organize the amphipods with the diagram folded is {0:N0}.", MinimumEnergyFolded);
            Logger.WriteLine("The least energy required to organize the amphipods with the diagram unfolded is {0:N0}.", MinimumEnergyUnfolded);
        }

        return PuzzleResult.Create(MinimumEnergyFolded, MinimumEnergyUnfolded);
    }

    private record struct State(string Hallway, string Amber, string Bronze, string Copper, string Desert)
    {
        public static int Entrance(char burrow) => burrow switch
        {
            'A' => 2,
            'B' => 4,
            'C' => 6,
            'D' => 8,
            _ => throw new InvalidOperationException(),
        };

        public string Burrow(char burrow) => burrow switch
        {
            'A' => Amber,
            'B' => Bronze,
            'C' => Copper,
            'D' => Desert,
            _ => throw new InvalidOperationException(),
        };

        public bool HasSpace(char burrow) => Burrow(burrow).Contains(' ', StringComparison.Ordinal);

        public bool IsEmpty(char burrow) => string.IsNullOrWhiteSpace(Burrow(burrow));

        public bool IsOrganized(char burrow) => Burrow(burrow).TrimStart().All((p) => p == burrow);

        public bool IsPathClear(char sourceBurrow, int destinationBurrow, bool fromHallway)
            => IsPathClear(Entrance(sourceBurrow), destinationBurrow, fromHallway);

        public bool IsPathClear(int sourceBurrow, int destinationBurrow, bool fromHallway)
        {
            int startIndex = Math.Min(sourceBurrow, destinationBurrow);
            int endIndex = Math.Max(sourceBurrow, destinationBurrow);

            if (fromHallway)
            {
                bool leftToRight = sourceBurrow < destinationBurrow;

                if (leftToRight)
                {
                    startIndex++;
                }
                else
                {
                    endIndex--;
                }
            }

            int length = endIndex - startIndex + 1;

            return string.IsNullOrWhiteSpace(Hallway.Substring(startIndex, length));
        }
    }

    private sealed class Burrow : IWeightedGraph<State>
    {
        private static readonly char[] Amphipods = { 'D', 'C', 'B', 'A' };
        private static readonly int[] HallwaySpaces = { 0, 1, 3, 5, 7, 9, 10 };

        public long Cost(State a, State b)
        {
            int differences = !string.Equals(a.Hallway, b.Hallway, StringComparison.Ordinal) ? 1 : 0;

            char burrowChanged = default;

            if (!string.Equals(a.Amber, b.Amber, StringComparison.Ordinal))
            {
                burrowChanged = 'A';
                differences++;
            }

            if (differences < 2 && !string.Equals(a.Bronze, b.Bronze, StringComparison.Ordinal))
            {
                burrowChanged = 'B';
                differences++;
            }

            if (differences < 2 && !string.Equals(a.Copper, b.Copper, StringComparison.Ordinal))
            {
                burrowChanged = 'C';
                differences++;
            }

            if (differences < 2 && !string.Equals(a.Desert, b.Desert, StringComparison.Ordinal))
            {
                burrowChanged = 'D';
                differences++;
            }

            if (differences != 2)
            {
                // If more than two parts (the hallway and one burrow) are different,
                // then there is more than one move between the A and B states. If there
                // are no differences, then they are equal and the cost is genuinely zero.
                return 0;
            }

            char amphipod = default;
            int index = -1;

            for (int i = 0; i < a.Hallway.Length; i++)
            {
                char hallwayA = a.Hallway[i];
                char hallwayB = b.Hallway[i];

                if (hallwayA != hallwayB)
                {
                    index = i;
                    amphipod = hallwayA == ' ' ? hallwayB : hallwayA;
                    break;
                }
            }

            string before = a.Burrow(burrowChanged);
            string after = b.Burrow(burrowChanged);

            int steps = Math.Abs(State.Entrance(burrowChanged) - index);
            steps += Math.Max(before.Count(' '), after.Count(' '));

            int multiplier = (int)Math.Pow(10, amphipod - 'A');

            return steps * multiplier;
        }

        public IEnumerable<State> Neighbors(State id)
        {
            var result = new List<State>();

            // Move any amphipods from their burrow to the hallway
            foreach (char amphipod in Amphipods)
            {
                if (id.IsEmpty(amphipod) || id.IsOrganized(amphipod))
                {
                    continue;
                }

                string burrow = id.Burrow(amphipod);

                if (!TryVacate(burrow, out char moved, out string? vacated))
                {
                    continue;
                }

                int burrowEntrance = State.Entrance(amphipod);

                foreach (int space in HallwaySpaces.OrderBy((p) => Math.Abs(burrowEntrance - p)))
                {
                    if (!id.IsPathClear(amphipod, space, fromHallway: false))
                    {
                        // Something is blocking the hallway
                        continue;
                    }

                    string newHallway = Replace(id.Hallway, moved, space);

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
                    !id.IsPathClear(space, State.Entrance(amphipod), fromHallway: true))
                {
                    continue;
                }

                string burrow = id.Burrow(amphipod);

                if (!id.IsOrganized(amphipod))
                {
                    continue;
                }

                int firstSpace = burrow.LastIndexOf(' ');
                string populated = Replace(burrow, amphipod, firstSpace);
                string newHallway = Replace(id.Hallway, ' ', space);

                result.Add(amphipod switch
                {
                    'A' => new(newHallway, populated, id.Bronze, id.Copper, id.Desert),
                    'B' => new(newHallway, id.Amber, populated, id.Copper, id.Desert),
                    'C' => new(newHallway, id.Amber, id.Bronze, populated, id.Desert),
                    'D' => new(newHallway, id.Amber, id.Bronze, id.Copper, populated),
                    _ => throw new InvalidOperationException(),
                });
            }

            return result;
        }

        private static string Replace(string value, char moved, int index)
        {
            string result = value.Remove(index, 1);
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

            int toMove = burrow.LastIndexOf(' ') + 1;

            moved = burrow[toMove];
            vacated = string.Concat(new string(' ', toMove + 1), burrow.AsSpan(toMove + 1));

            return true;
        }
    }
}
