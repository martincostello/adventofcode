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
    /// <summary>
    /// Gets the least energy required to organize the amphipods with the diagram unfolded.
    /// </summary>
    public int MinimumEnergyFolded { get; private set; }

    /// <summary>
    /// Gets the least energy required to organize the amphipods with the diagram folded.
    /// </summary>
    public int MinimumEnergyUnfolded { get; private set; }

    /// <summary>
    /// Organizes the specified amphipods into the correct burrows.
    /// </summary>
    /// <param name="diagram">The diagram of the burrows occupied by the amphipods.</param>
    /// <param name="unfoldDiagram">Whether to unfold the diagram to reveal the missing lines.</param>
    /// <returns>
    /// The least energy required to organize the amphipods.
    /// </returns>
    public static int Organize(IList<string> diagram, bool unfoldDiagram)
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

        const int HallwayLength = 11;

        int burrowDepth = unfoldDiagram ? 4 : 2;

        var start = new State(
            new(' ', HallwayLength),
            amber,
            bronze,
            copper,
            desert,
            burrowDepth);

        var goal = new State(
            new(' ', HallwayLength),
            new('A', burrowDepth),
            new('B', burrowDepth),
            new('C', burrowDepth),
            new('D', burrowDepth),
            burrowDepth);

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
        var a = new State(x.Hallway, x.Amber, x.Bronze, x.Copper, x.Desert, 2);
        var b = new State(y.Hallway, y.Amber, y.Bronze, y.Copper, y.Desert, 2);
        var burrow = new Burrow();

        return (int)burrow.Cost(a, b);
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> diagram = await ReadResourceAsLinesAsync();

        MinimumEnergyFolded = Organize(diagram, unfoldDiagram: false);
        MinimumEnergyUnfolded = Organize(diagram, unfoldDiagram: true);

        if (Verbose)
        {
            Logger.WriteLine("The least energy required to organize the amphipods is {0:N0}.", MinimumEnergyFolded);
            Logger.WriteLine("The least energy required to organize the amphipods with the diagram unfolded is {0:N0}.", MinimumEnergyUnfolded);
        }

        return PuzzleResult.Create(MinimumEnergyFolded, MinimumEnergyUnfolded);
    }

    private record struct State(string Hallway, string Amber, string Bronze, string Copper, string Desert, int Depth)
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

        public override string ToString()
        {
            var builder = new StringBuilder()
                .AppendLine("#############")
                .Append('#')
                .Append(Hallway.Replace(' ', '.'))
                .Append('#')
                .AppendLine();

            for (int i = 0; i < Depth; i++)
            {
                builder
                    .Append("###")
                    .Append(Amber[i])
                    .Append('#')
                    .Append(Bronze[i])
                    .Append('#')
                    .Append(Copper[i])
                    .Append('#')
                    .Append(Desert[i])
                    .AppendLine("###");
            }

            builder.AppendLine("  #########  ");

            return builder.ToString();
        }
    }

    private sealed class Burrow : IWeightedGraph<State>
    {
        private static readonly char[] Amphipods = { 'A', 'B', 'C', 'D' };
        private static readonly int[] HallwaySpaces = { 0, 1, 3, 5, 7, 9, 10 };

        public long Cost(State a, State b)
        {
            int differences = !string.Equals(a.Hallway, b.Hallway, StringComparison.Ordinal) ? 1 : 0;

            if (!string.Equals(a.Amber, b.Amber, StringComparison.Ordinal))
            {
                differences++;
            }

            if (differences < 2 && !string.Equals(a.Bronze, b.Bronze, StringComparison.Ordinal))
            {
                differences++;
            }

            if (differences < 2 && !string.Equals(a.Copper, b.Copper, StringComparison.Ordinal))
            {
                differences++;
            }

            if (differences < 2 && !string.Equals(a.Desert, b.Desert, StringComparison.Ordinal))
            {
                differences++;
            }

            if (differences != 2)
            {
                // If more than two parts (the hallway and one burrow) are different,
                // then there is more than one move between the A and B states. If there
                // are no differences, then they are equal and the cost is genuinely zero.
                return 0;
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

            string burrowBefore = a.Burrow(moved);
            string burrowAfter = b.Burrow(moved);

            char burrowChanged = moved;

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

            steps += Math.Max(burrowAfter.Count((p) => p == ' '), burrowBefore.Count((p) => p == ' '));

            int multiplier = Multiplier(moved);

            return steps * multiplier;

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

                    string newHallway = Move(id.Hallway, moved, space);

                    result.Add(amphipod switch
                    {
                        'A' => new(newHallway, vacated, id.Bronze, id.Copper, id.Desert, id.Depth),
                        'B' => new(newHallway, id.Amber, vacated, id.Copper, id.Desert, id.Depth),
                        'C' => new(newHallway, id.Amber, id.Bronze, vacated, id.Desert, id.Depth),
                        'D' => new(newHallway, id.Amber, id.Bronze, id.Copper, vacated, id.Depth),
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
                string populated = Move(burrow, amphipod, firstSpace);
                string newHallway = Move(id.Hallway, ' ', space);

                result.Add(amphipod switch
                {
                    'A' => new(newHallway, populated, id.Bronze, id.Copper, id.Desert, id.Depth),
                    'B' => new(newHallway, id.Amber, populated, id.Copper, id.Desert, id.Depth),
                    'C' => new(newHallway, id.Amber, id.Bronze, populated, id.Desert, id.Depth),
                    'D' => new(newHallway, id.Amber, id.Bronze, id.Copper, populated, id.Depth),
                    _ => throw new InvalidOperationException(),
                });
            }

            return result.ToList();
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

            int toMove = burrow.LastIndexOf(' ');
            toMove++;

            moved = burrow[toMove];
            vacated = string.Concat(new string(' ', toMove + 1), burrow.AsSpan(toMove + 1));

            return true;
        }
    }
}
