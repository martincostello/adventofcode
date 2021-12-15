// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2016;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2016/day/11</c>. This class cannot be inherited.
/// </summary>
[Puzzle("Radioisotope Thermoelectric Generators", 2016, 11, RequiresData = true, IsHidden = true)]
public sealed class Day11 : Puzzle
{
    /// <summary>
    /// Gets the minimum number of steps required to assemble all the generators.
    /// </summary>
    public int MinimumStepsForAssembly { get; private set; }

    /// <summary>
    /// Returns the minimum number of steps required to assemble all the generators
    /// on the fourth floor of the building given the specified initial state.
    /// </summary>
    /// <param name="initialState">The initial state of the generators and microchips.</param>
    /// <returns>
    /// The minimum number of steps required to assemble all the generators given <paramref name="initialState"/>.
    /// </returns>
    internal static int GetMinimumStepsForAssembly(IList<string> initialState)
    {
        IDictionary<int, ICollection<Element>> state = ParseInitialState(initialState);

        var facility = new Facility(state);

        int steps = 0;

        using (var source = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
        {
            while (facility.State[facility.AssemblyFloor].Count != facility.TotalItems && !source.IsCancellationRequested)
            {
                // TODO Implement algorithm to move all items to the top floor
                var orphansOnThisFloor = facility.GetOrphansOnFloor(facility.Elevator.Floor);
                var orphanMicrochipsOnThisFloor = orphansOnThisFloor.OfType<Microchip>().ToList();

                if (facility.Elevator.Floor < facility.AssemblyFloor)
                {
                    var orphansUpstairs = facility.GetOrphansOnFloor(facility.Elevator.Floor + 1);
                    var orphanGeneratorUpstairs = orphansUpstairs.OfType<Generator>().ToList();

                    if (orphanMicrochipsOnThisFloor.Any() && orphanGeneratorUpstairs.Any())
                    {
                    }
                }
            }
        }

        return steps;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> initialState = await ReadResourceAsLinesAsync();

        MinimumStepsForAssembly = GetMinimumStepsForAssembly(initialState);

        if (Verbose)
        {
            Logger.WriteLine(
                $"The minimum number of steps required to bring all of the objects to the fourth floor is {0:N0}.",
                MinimumStepsForAssembly);
        }

        return PuzzleResult.Create(MinimumStepsForAssembly);
    }

    /// <summary>
    /// Parses the initial state of the Radioisotope Testing Facility.
    /// </summary>
    /// <param name="initialState">The initial state of the facility.</param>
    /// <returns>
    /// An <see cref="IDictionary{TKey, TValue}"/> containing the items on each
    /// floor of the Radioisotope Testing Facility, keyed by their zero-based floor.
    /// </returns>
    private static IDictionary<int, ICollection<Element>> ParseInitialState(IList<string> initialState)
    {
        IDictionary<int, ICollection<Element>> state = new Dictionary<int, ICollection<Element>>();

        for (int i = 0; i < initialState.Count; i++)
        {
            var items = new List<Element>();

            string[] split = initialState[i]
                .TrimEnd('.')
                .Split(' ');

            for (int j = 0; j < split.Length; j++)
            {
                string value = split[j].TrimEnd(',');

                if (string.Equals(value, "generator", StringComparison.Ordinal))
                {
                    items.Add(new Generator(split[j - 1]));
                }
                else if (string.Equals(value, "microchip", StringComparison.Ordinal))
                {
                    items.Add(new Microchip(split[j - 1].Replace("-compatible", string.Empty, StringComparison.Ordinal)));
                }
            }

            state[i] = items;
        }

        return state;
    }

    /// <summary>
    /// The base class for Radioisotope Thermoelectric elements.
    /// </summary>
    private abstract class Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Element"/> class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        protected Element(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of the element associated with the element.
        /// </summary>
        public string Name { get; }
    }

    /// <summary>
    /// A class representing the elevator. This class cannot be inherited.
    /// </summary>
    private sealed class Elevator
    {
        /// <summary>
        /// The maximum capacity of the elevator.
        /// </summary>
        internal const int Capacity = 2;

        /// <summary>
        /// The contents of the elevator. This field is read-only.
        /// </summary>
        private readonly IList<Element> _contents = new List<Element>();

        /// <summary>
        /// The maximum floor the elevator can move to. This field is read-only.
        /// </summary>
        private readonly int _maximumFloor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Elevator"/> class.
        /// </summary>
        /// <param name="maximumFloor">The maximum floor the elevator can go it.</param>
        internal Elevator(int maximumFloor)
        {
            _maximumFloor = maximumFloor;
        }

        /// <summary>
        /// Gets the current floor the elevator is on.
        /// </summary>
        internal int Floor { get; private set; }

        /// <summary>
        /// Gets the current number of items in the elevator.
        /// </summary>
        internal int ItemCount => _contents.Count;

        /// <summary>
        /// Loads an element into the elevator.
        /// </summary>
        /// <param name="element">The element to load.</param>
        /// <returns>
        /// <see langword="true"/> if the element was successfully loaded; otherwise <see langword="false"/>.
        /// </returns>
        internal bool Load(Element element)
        {
            bool loaded = false;

            if (ItemCount < Capacity)
            {
                _contents.Add(element);
                loaded = true;
            }

            return loaded;
        }

        /// <summary>
        /// Moves the elevator down, if possible.
        /// </summary>
        /// <returns>
        /// The floor the elevator is now on.
        /// </returns>
        internal int MoveDown()
        {
            if (Floor > 0 && ItemCount > 0 && ItemCount <= Capacity)
            {
                Floor--;
            }

            return Floor;
        }

        /// <summary>
        /// Moves the elevator up, if possible.
        /// </summary>
        /// <returns>
        /// The floor the elevator is now on.
        /// </returns>
        internal int MoveUp()
        {
            if (Floor < _maximumFloor && ItemCount > 0 && ItemCount <= Capacity)
            {
                Floor++;
            }

            return Floor;
        }
    }

    /// <summary>
    /// A class representing the Radioisotope Testing Facility. This class cannot be inherited.
    /// </summary>
    private sealed class Facility
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Facility"/> class.
        /// </summary>
        /// <param name="initialState">The initial state of the facility.</param>
        internal Facility(IDictionary<int, ICollection<Element>> initialState)
        {
            State = initialState;

            AssemblyFloor = State.Max((p) => p.Key);
            TotalItems = State.SelectMany((p) => p.Value).Count();

            Elevator = new Elevator(AssemblyFloor);
        }

        /// <summary>
        /// Gets the zero-based number of the assembly floor.
        /// </summary>
        internal int AssemblyFloor { get; }

        /// <summary>
        /// Gets the facility's elevator.
        /// </summary>
        internal Elevator Elevator { get; }

        /// <summary>
        /// Gets the total number of items in the facility.
        /// </summary>
        internal int TotalItems { get; }

        /// <summary>
        /// Gets the current state of the facility.
        /// </summary>
        internal IDictionary<int, ICollection<Element>> State { get; }

        /// <summary>
        /// Returns the orphan generators and/or microchips on the specified floor.
        /// </summary>
        /// <param name="floor">The floor to get the orphans from.</param>
        /// <returns>
        /// An <see cref="IList{T}"/> containing the orphans on the specified floor, if any.
        /// </returns>
        internal IList<Element> GetOrphansOnFloor(int floor)
        {
            return State[floor]
                .GroupBy((p) => p.Name)
                .Where((p) => p.Count() == 1)
                .SelectMany((p) => p)
                .ToList();
        }
    }

    /// <summary>
    /// A class representing a Radioisotope Thermoelectric Generator. This class cannot be inherited.
    /// </summary>
    private sealed class Generator : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Generator"/> class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        internal Generator(string name)
            : base(name)
        {
        }
    }

    /// <summary>
    /// A class representing a microchip for a Radioisotope Thermoelectric Generator. This class cannot be inherited.
    /// </summary>
    private sealed class Microchip : Element
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Microchip"/> class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        internal Microchip(string name)
            : base(name)
        {
        }
    }
}
