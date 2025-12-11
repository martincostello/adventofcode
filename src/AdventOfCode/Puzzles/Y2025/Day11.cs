// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2025;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2025/day/11</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2025, 11, "Reactor", RequiresData = true, IsSlow = true)]
public sealed class Day11 : Puzzle<int, int>
{
    /// <summary>
    /// Counts the number of different paths that lead from <c>you</c> and <c>srv</c> to <c>out</c>.
    /// </summary>
    /// <param name="devices">The devices to solve the puzzle from.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
    /// <returns>
    /// The number of paths from <c>you</c> to <c>out</c> and the number of paths from
    /// <c>svr</c> to <c>out</c> via <c>dac</c> and <c>fft</c>.
    /// </returns>
    public static (int YouPaths, int ServerPaths) CountPaths(IReadOnlyList<string> devices, CancellationToken cancellationToken)
    {
        var connections = new Dictionary<string, Device>();

        foreach (string line in devices)
        {
            int index = line.IndexOf(':', StringComparison.Ordinal);
            string name = line[..index];
            string rest = line[(index + 2)..];

            var outputs = new List<string>();

            foreach (var range in rest.AsSpan().Split(' '))
            {
                outputs.Add(rest[range]);
            }

            connections[name] = new(name, outputs);
        }

        var path = new Stack<string>();

        const string Dac = "dac";
        const string Fft = "fft";
        const string Out = "out";
        const string Svr = "svr";

        int youPaths = CountPaths("you", Out, path, [], connections, cancellationToken);

        int srvPaths = 0;

        int svrdac = CountPaths(Svr, Dac, path, [Fft, Out], connections, cancellationToken);

        if (svrdac > 0)
        {
            int dacfft = CountPaths(Dac, Fft, path, [Out], connections, cancellationToken);

            if (dacfft > 0)
            {
                int fftout = CountPaths(Fft, Out, path, [Dac], connections, cancellationToken);

                if (fftout > 0)
                {
                    srvPaths += svrdac * dacfft * fftout;
                }
            }
        }

        int svrfft = CountPaths(Svr, Fft, path, [Dac, Out], connections, cancellationToken);

        if (svrfft > 0)
        {
            int fftdac = CountPaths(Fft, Dac, path, [], connections, cancellationToken);

            if (fftdac > 0)
            {
                int dacout = CountPaths(Dac, Out, path, [Fft], connections, cancellationToken);

                if (svrfft > 0 && fftdac > 0 && dacout > 0)
                {
                    srvPaths += svrfft * fftdac * dacout;
                }
            }
        }

        return (youPaths, srvPaths);

        static int CountPaths(
            string origin,
            string destination,
            Stack<string> path,
            List<string> except,
            Dictionary<string, Device> connections,
            CancellationToken cancellationToken)
        {
            if (except.Any(path.Contains))
            {
                return 0;
            }

            if (origin.SequenceEqual(destination))
            {
                return 1;
            }

            int total = 0;

            if (connections.TryGetValue(origin, out var device))
            {
                path.Push(origin);
                connections.Remove(origin);

                foreach (string to in device.Connections)
                {
                    total += CountPaths(to, destination, path, except, connections, cancellationToken);
                }

                path.Pop();
                connections.Add(origin, device);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return total;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        return await SolveWithLinesAsync(
            static (values, logger, cancellationToken) =>
            {
                (int youPaths, int serverPaths) = CountPaths(values, cancellationToken);

                if (logger is { })
                {
                    logger.WriteLine("{0} different paths lead from you to out.", youPaths);
                    logger.WriteLine("{0} different paths lead from srv to out via dac and fft.", serverPaths);
                }

                return (youPaths, serverPaths);
            },
            cancellationToken);
    }

    private sealed record Device(string Name, IReadOnlyList<string> Connections);
}
