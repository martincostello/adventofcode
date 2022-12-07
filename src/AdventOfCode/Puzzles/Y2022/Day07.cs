// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.AdventOfCode.Puzzles.Y2022;

/// <summary>
/// A class representing the puzzle for <c>https://adventofcode.com/2022/day/7</c>. This class cannot be inherited.
/// </summary>
[Puzzle(2022, 07, "No Space Left On Device", RequiresData = true)]
public sealed class Day07 : Puzzle
{
    /// <summary>
    /// Gets the total size of all directories with a total size of at least 100000.
    /// </summary>
    public long TotalSizeOfDirectoriesLargerThan100000 { get; private set; }

    /// <summary>
    /// Finds the total size of the directories with a total size of at least the specified limit.
    /// </summary>
    /// <param name="terminalOutput">The terminal output to parse.</param>
    /// <param name="limit">The size limit to return the sum for.</param>
    /// <returns>
    /// The total size of all directories with a total size of at least <paramref name="limit"/>.
    /// </returns>
    public static long GetTotalSize(IList<string> terminalOutput, int limit)
    {
        var directories = GetDirectories(terminalOutput.ToArray());

        return directories
            .Where((p) => p.TotalSize <= limit)
            .Sum((p) => p.TotalSize);

        static List<Directory> GetDirectories(ReadOnlySpan<string> terminalOutput)
        {
            var root = new Directory("/", null, new());
            var current = root;

            var fileSystem = new Dictionary<string, Directory>()
            {
                [root.Path] = root,
            };

            for (int i = 0; i < terminalOutput.Length; i++)
            {
                string line = terminalOutput[i];

                if (line.StartsWith("$ cd "))
                {
                    string name = line[5..];

                    if (name == "..")
                    {
                        current = current.Parent!;
                    }
                    else
                    {
                        string path = name == "/" ? name : current.Path + "/" + name;

                        if (!fileSystem.TryGetValue(path, out var directory))
                        {
                            directory = new Directory(name, current, new());
                            fileSystem[directory.Path] = directory;
                        }

                        current = directory;
                    }
                }
                else if (line.StartsWith("$ ls"))
                {
                    i += ParseEntries(current, terminalOutput[(i + 1)..]);

                    foreach (var child in current.Children)
                    {
                        if (child is Directory directory)
                        {
                            fileSystem[directory.Path] = directory;
                        }
                    }
                }
            }

            return fileSystem.Values.ToList();
        }

        static int ParseEntries(Directory current, ReadOnlySpan<string> terminalOutput)
        {
            int count = 0;

            for (int i = 0; i < terminalOutput.Length; i++)
            {
                string item = terminalOutput[i];

                if (item.StartsWith('$'))
                {
                    count = i;
                    break;
                }
                else if (item.StartsWith("dir "))
                {
                    current.Children.Add(new Directory(item[4..], current, new()));
                }
                else
                {
                    string[] split = item.Split(' ');
                    current.Children.Add(new File(split[1], current, Parse<long>(split[0])));
                }
            }

            return count;
        }
    }

    /// <inheritdoc />
    protected override async Task<PuzzleResult> SolveCoreAsync(string[] args, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(args);

        var terminalOutput = await ReadResourceAsLinesAsync();

        TotalSizeOfDirectoriesLargerThan100000 = GetTotalSize(terminalOutput, 100000);

        if (Verbose)
        {
            Logger.WriteLine(
                "The sum of the total sizes of the directories with a total size of at most 100,000 is {0}.",
                TotalSizeOfDirectoriesLargerThan100000);
        }

        return PuzzleResult.Create(TotalSizeOfDirectoriesLargerThan100000);
    }

    private abstract record FileSystemEntry(string Name, FileSystemEntry? Container)
    {
        public string Path =>
            Container is null ?
            Name :
            Container.Path.TrimEnd('/') + "/" + Name;
    }

    private sealed record Directory(string Name, Directory? Parent, List<FileSystemEntry> Children)
        : FileSystemEntry(Name, Parent)
    {
        public long TotalSize
        {
            get
            {
                long size = 0;

                foreach (var child in Children)
                {
                    size += child switch
                    {
                        Directory d => d.TotalSize,
                        File f => f.Size,
                        _ => throw new InvalidOperationException(),
                    };
                }

                return size;
            }
        }
    }

    private sealed record File(string Name, Directory Directory, long Size)
        : FileSystemEntry(Name, Directory);
}
