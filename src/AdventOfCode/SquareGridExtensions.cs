// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode;

/// <summary>
/// A class containing extension methods for the <see cref="SquareGrid"/> class. This class cannot be inherited.
/// </summary>
public static class SquareGridExtensions
{
    /// <summary>
    /// Iterates over each character in the specified layout and invokes the visitor action for each cell in the grid.
    /// </summary>
    /// <typeparam name="T">The type of the grid.</typeparam>
    /// <param name="grid">The grid to traverse.</param>
    /// <param name="layout">A list of strings representing the layout to traverse. Each string corresponds to a row in the grid.</param>
    /// <param name="visitor">An action to invoke for each cell in the grid.</param>
    public static void Visit<T>(this T grid, IReadOnlyList<string> layout, Action<T, Point, char> visitor)
        where T : SquareGrid =>
        Visit(grid, layout, visitor, static (grid, location, cell, visitor) =>
        {
            visitor(grid, location, cell);
            return visitor;
        });

    /// <summary>
    /// Iterates over each character in the specified layout and invokes the visitor action for each cell in the grid, accumulating state across all cells.
    /// </summary>
    /// <typeparam name="TGrid">The type of the grid.</typeparam>
    /// <typeparam name="TState">The type of the state that is passed and optionally updated during the visit.</typeparam>
    /// <param name="grid">The grid to traverse.</param>
    /// <param name="layout">A list of strings representing the layout to traverse. Each string corresponds to a row in the grid.</param>
    /// <param name="state">The initial state value to be passed to the visitor function and optionally updated as each cell is visited.</param>
    /// <param name="visitor">An action to invoke for each cell in the grid.</param>
    /// <returns>The final state value after all cells in the grid have been visited.</returns>
    public static TState Visit<TGrid, TState>(this TGrid grid, IReadOnlyList<string> layout, TState state, Func<TGrid, Point, char, TState, TState> visitor)
        where TGrid : SquareGrid
    {
        for (int y = 0; y < layout.Count; y++)
        {
            string row = layout[y];

            for (int x = 0; x < row.Length; x++)
            {
                state = visitor(grid, new(x, y), row[x], state);
            }
        }

        return state;
    }

    /// <summary>
    /// Iterates over each cell in the grid and invokes the specified visitor action for every position.
    /// </summary>
    /// <typeparam name="T">The type of the grid.</typeparam>
    /// <param name="grid">The grid to traverse.</param>
    /// <param name="visitor">An action to invoke for each cell in the grid.</param>
    public static void Visit<T>(this T grid, Action<T, Point> visitor)
        where T : SquareGrid
        => Visit(grid, visitor, static (grid, location, visitor) => visitor(grid, location));

    /// <summary>
    /// Iterates over each cell in the grid and invokes the specified visitor action for every position.
    /// </summary>
    /// <typeparam name="TGrid">The type of the grid.</typeparam>
    /// <typeparam name="TState">The type of the state object to pass to the visitor action.</typeparam>
    /// <param name="grid">The grid to traverse.</param>
    /// <param name="state">The state to pass to each invocation of the visitor action.</param>
    /// <param name="visitor">An action to invoke for each cell in the grid.</param>
    public static void Visit<TGrid, TState>(this TGrid grid, TState state, Action<TGrid, Point, TState> visitor)
        where TGrid : SquareGrid
    {
        var bounds = grid.Bounds;

        for (int y = 0; y < bounds.Height; y++)
        {
            for (int x = 0; x < bounds.Width; x++)
            {
                visitor(grid, new(x, y), state);
            }
        }
    }

    /// <summary>
    /// Iterates over each cell in the grid and applies the specified visitor function, accumulating state across all locations.
    /// </summary>
    /// <typeparam name="TGrid">The type of the grid.</typeparam>
    /// <typeparam name="TState">The type of the state that is passed and optionally updated during the visit.</typeparam>
    /// <param name="grid">The grid to traverse.</param>
    /// <param name="state">The initial state value to be passed to the visitor function and optionally updated as each location is visited.</param>
    /// <param name="visitor">A function that is invoked for each location in the grid.</param>
    /// <returns>The final state value after all locations in the grid have been visited.</returns>
    public static TState Visit<TGrid, TState>(this TGrid grid, TState state, Func<TGrid, Point, TState, TState> visitor)
        where TGrid : SquareGrid
    {
        var bounds = grid.Bounds;

        for (int y = 0; y < bounds.Height; y++)
        {
            for (int x = 0; x < bounds.Width; x++)
            {
                state = visitor(grid, new(x, y), state);
            }
        }

        return state;
    }

    /// <summary>
    /// Visits each location in the grid and applies the specified visitor function.
    /// </summary>
    /// <typeparam name="TGrid">The type of the grid.</typeparam>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <param name="grid">The grid whose locations will be visited.</param>
    /// <param name="state">The state value to be passed to the visitor function.</param>
    /// <param name="visitor">A function that is invoked for each location in the grid.</param>
    public static void VisitLocations<TGrid, TState>(this TGrid grid, TState state, Action<TGrid, Point, TState> visitor)
        where TGrid : SquareGrid =>
        VisitLocations(grid, (visitor, state), static (grid, location, state) =>
        {
            state.visitor(grid, location, state.state);
            return state;
        });

    /// <summary>
    /// Visits each location in the grid and applies the specified visitor function, accumulating state across all locations.
    /// </summary>
    /// <typeparam name="TGrid">The type of the grid.</typeparam>
    /// <typeparam name="TState">The type of the state that is passed and optionally updated during the visit.</typeparam>
    /// <param name="grid">The grid whose locations will be visited.</param>
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
