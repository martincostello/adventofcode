// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;

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
    public long TotalSizeOfDirectoriesLargerThanLimit { get; private set; }

    /// <summary>
    /// Gets the total size of the smallest directory to delete that frees enough disk space.
    /// </summary>
    public long SizeOfSmallestDirectoryToDelete { get; private set; }

    /// <summary>
    /// Finds the total size of the directories with a total size of at least 100000 and
    /// the total size of the smallest directory to delete that frees enough disk space.
    /// </summary>
    /// <param name="terminalOutput">The terminal output to parse.</param>
    /// <returns>
    /// The total size of all directories with a total size of at least 100000 and
    /// the total size of the smallest directory to delete that frees enough disk space.
    /// </returns>
    public static (long TotalSizeOfDirectoriesLargerThanLimit, long SizeOfSmallestDirectoryToDelete) GetDirectorySizes(
        IList<string> terminalOutput)
    {
        const long DiskSize = 70000000;
        const long SizeLimit = 100000;
        const long UpdateSize = 30000000;

        var directories = GetDirectories(terminalOutput.ToArray());

        long totalSizeOfDirectoriesLargerThanLimit = directories
            .Where((p) => p.TotalSize <= SizeLimit)
            .Sum((p) => p.TotalSize);

        long consumed = directories.Single((p) => p.Container is null).TotalSize;
        long freeSpace = DiskSize - consumed;
        long required = UpdateSize - freeSpace;

        long sizeOfSmallestDirectoryToDelete = directories
            .Where((p) => p.TotalSize >= required)
            .Select((p) => p.TotalSize)
            .Min();

        return (totalSizeOfDirectoriesLargerThanLimit, sizeOfSmallestDirectoryToDelete);

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

                if (line.StartsWith("$ cd ", StringComparison.Ordinal))
                {
                    string name = line[5..];

                    if (name == "..")
                    {
                        current = current.Parent!;
                    }
                    else
                    {
                        string path = name == "/" ? name : current.Path.TrimEnd('/') + "/" + name;

                        if (!fileSystem.TryGetValue(path, out var directory))
                        {
                            directory = new Directory(name, current, new());
                            fileSystem[directory.Path] = directory;
                        }

                        current = directory;
                    }
                }
                else if (line.StartsWith("$ ls", StringComparison.Ordinal))
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
                else if (item.StartsWith("dir ", StringComparison.Ordinal))
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
        var terminalOutput = await ReadResourceAsLinesAsync();

        (TotalSizeOfDirectoriesLargerThanLimit, SizeOfSmallestDirectoryToDelete) = GetDirectorySizes(terminalOutput);

        if (Verbose)
        {
            Logger.WriteLine(
                "The sum of the total sizes of the directories with a total size of at most 100,000 is {0}.",
                TotalSizeOfDirectoriesLargerThanLimit);

            Logger.WriteLine(
                "The sizes of the directory would free up enough space on the filesystem to run the update is {0}.",
                SizeOfSmallestDirectoryToDelete);
        }

        return PuzzleResult.Create(TotalSizeOfDirectoriesLargerThanLimit, SizeOfSmallestDirectoryToDelete);
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
                        Directory directory => directory.TotalSize,
                        File file => file.Size,
                        _ => throw new UnreachableException(),
                    };
                }

                return size;
            }
        }
    }

    private sealed record File(string Name, Directory Directory, long Size)
        : FileSystemEntry(Name, Directory);
}
