// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles;

/// <summary>
/// A class containing extension methods for the <see cref="SquareGrid"/> class. This class cannot be inherited.
/// </summary>
public static class SquareGridExtensions
{
    /// <summary>
    /// Iterates over each character in the specified layout and invokes the visitor action for each cell in the grid.
    /// </summary>
    /// <typeparam name="T">The type of the grid.</typeparam>
    /// <param name="grid">The grid instance to visit.</param>
    /// <param name="layout">A list of strings representing the layout to traverse. Each string corresponds to a row in the grid.</param>
    /// <param name="visitor">An action to invoke for each cell in the grid.</param>
    public static void Visit<T>(this T grid, IReadOnlyList<string> layout, Action<T, Point, char> visitor)
        where T : SquareGrid
    {
        for (int y = 0; y < layout.Count; y++)
        {
            for (int x = 0; x < layout[y].Length; x++)
            {
                visitor(grid, new(x, y), layout[y][x]);
            }
        }
    }

    /// <summary>
    /// Iterates over each cell in the grid and invokes the specified visitor action for every position.
    /// </summary>
    /// <typeparam name="TGrid">The type of the grid.</typeparam>
    /// <typeparam name="TState">The type of the state object to pass to the visitor action.</typeparam>
    /// <param name="grid">The grid to traverse. Must not be null.</param>
    /// <param name="state">The state to pass to each invocation of the visitor action.</param>
    /// <param name="visitor">An action to invoke for each cell in the grid.</param>
    public static void Visit<TGrid, TState>(this TGrid grid, TState state, Action<TGrid, Point, TState> visitor)
        where TGrid : SquareGrid
    {
        for (int y = 0; y < grid.Bounds.Height; y++)
        {
            for (int x = 0; x < grid.Bounds.Width; x++)
            {
                visitor(grid, new(x, y), state);
            }
        }
    }

    /// <summary>
    /// Visits each location in the grid and applies the specified visitor function, accumulating state across all locations.
    /// </summary>
    /// <typeparam name="TGrid">The type of the grid.</typeparam>
    /// <typeparam name="TState">The type of the state that is passed and optionally updated during the visit.</typeparam>
    /// <param name="grid">The grid whose locations will be visited. Must not be null.</param>
    /// <param name="state">The initial state value to be passed to the visitor function and optionally updated as each location is visited.</param>
    /// <param name="visitor">A function that is invoked for each location in the grid.</param>
    /// <returns>The final state value after all locations in the grid have been visited.</returns>
    public static TState VisitLocations<TGrid, TState>(this TGrid grid, TState state, Func<TGrid, Point, TState, TState> visitor)
        where TGrid : SquareGrid
    {
        foreach (var location in grid.Locations)
        {
            state = visitor(grid, location, state);
        }

        return state;
    }
}
