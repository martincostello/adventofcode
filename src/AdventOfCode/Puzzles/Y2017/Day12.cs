// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2017;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2017/day/12</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2017, 12, RequiresData = true)]
public sealed class Day12 : Puzzle
{
    /// <summary>
    /// Gets the number of programs in the group that contains program zero.
    /// </summary>
    public int ProgramsInGroupOfProgram0 { get; private set; }

    /// <summary>
    /// Gets the number of groups of programs.
    /// </summary>
    public int NumberOfGroups { get; private set; }

    /// <summary>
    /// Gets the number of groups in the specified network of pipes.
    /// </summary>
    /// <param name="pipes">A collection of strings describing the pipes connecting the programs.</param>
    /// <returns>
    /// The number of groups of programs in network specified by <paramref name="pipes"/>.
    /// </returns>
    public static int GetGroupsInNetwork(ICollection<string> pipes)
    {
        IDictionary<int, Node> graph = BuildGraph(pipes);

        var groups = new HashSet<string>();

        foreach (Node target in graph.Values)
        {
            ICollection<Node> members = GetMembersOfGroup(target);

            string groupId = string.Join(",", members.Select((p) => p.Id).OrderBy((p) => p));

            if (!groups.Contains(groupId))
            {
                groups.Add(groupId);
            }
        }

        return groups.Count;
    }

    /// <summary>
    /// Gets the number of programs in the same group as the specified program Id.
    /// </summary>
    /// <param name="programId">The Id of the program to find the number of programs in its group.</param>
    /// <param name="pipes">A collection of strings describing the pipes connecting the programs.</param>
    /// <returns>
    /// The number of programs in the same group as the program with the Id specified by <paramref name="programId"/>.
    /// </returns>
    public static int GetProgramsInGroup(int programId, ICollection<string> pipes)
    {
        IDictionary<int, Node> graph = BuildGraph(pipes);
        Node target = graph[programId];

        ICollection<Node> members = GetMembersOfGroup(target);

        return members.Count;
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        IList<string> pipes = await ReadResourceAsLinesAsync();

        ProgramsInGroupOfProgram0 = GetProgramsInGroup(0, pipes);
        NumberOfGroups = GetGroupsInNetwork(pipes);

        if (Verbose)
        {
            Logger.WriteLine($"There are {ProgramsInGroupOfProgram0:N0} programs in the group that contains program ID 0.");
            Logger.WriteLine($"There are {NumberOfGroups:N0} groups in the network of pipes.");
        }

        return PuzzleResult.Create(ProgramsInGroupOfProgram0, NumberOfGroups);
    }

    /// <summary>
    /// Builds a graph from the specified network of pipes.
    /// </summary>
    /// <param name="pipes">A collection of strings describing the pipes connecting the programs.</param>
    /// <returns>
    /// An <see cref="IDictionary{TKey, TValue}"/> containing the network of programs keyed by their Id.
    /// </returns>
    private static IDictionary<int, Node> BuildGraph(IEnumerable<string> pipes)
    {
        var graph = pipes
            .Select((p) => new Node(p))
            .ToDictionary((p) => p.Id, (p) => p);

        foreach (Node node in graph.Values)
        {
            foreach (int edge in node.EdgeIds)
            {
                node.Edges.Add(graph[edge]);
            }
        }

        return graph;
    }

    /// <summary>
    /// Gets the programs that are in the same group as the specified program.
    /// </summary>
    /// <param name="target">The Id of the program to find the number of programs in its group.</param>
    /// <returns>
    /// The programs that in the same group as the program specified by <paramref name="target"/>.
    /// </returns>
    private static ICollection<Node> GetMembersOfGroup(Node target)
    {
        var members = new List<Node>()
        {
            target,
        };

        foreach (Node edge in target.Edges)
        {
            Visit(edge, members);
        }

        return members;
    }

    /// <summary>
    /// Visits the specified node.
    /// </summary>
    /// <param name="node">The node to visit.</param>
    /// <param name="members">The current members of the group of the target node.</param>
    private static void Visit(Node node, ICollection<Node> members)
    {
        if (!members.Contains(node))
        {
            members.Add(node);
        }

        foreach (Node edge in node.Edges)
        {
            if (!members.Contains(edge))
            {
                Visit(edge, members);
            }
        }
    }

    /// <summary>
    /// A class representing a node in the network of programs. This class cannot be inherited.
    /// </summary>
    private sealed class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="pipe">A string describing the pipes connecting the node with its edges.</param>
        internal Node(string pipe)
        {
            string[] split = pipe.Split(' ');

            Id = Parse<int>(split[0]);

            EdgeIds = split
                .Skip(2)
                .Select((p) => Parse<int>(p.TrimEnd(',')))
                .ToList();
        }

        /// <summary>
        /// Gets the Id of the node.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the Ids of the edges of the node.
        /// </summary>
        public ICollection<int> EdgeIds { get; }

        /// <summary>
        /// Gets the edges of the node.
        /// </summary>
        public ICollection<Node> Edges { get; } = new List<Node>();
    }
}
