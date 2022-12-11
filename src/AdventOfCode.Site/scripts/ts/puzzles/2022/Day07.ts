// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Puzzle } from '../Puzzle';
import { Puzzle2022 } from './Puzzle2022';

export class Day07 extends Puzzle2022 {
    totalSizeOfDirectoriesLargerThanLimit: number;
    sizeOfSmallestDirectoryToDelete: number;

    override get name() {
        return 'No Space Left On Device';
    }

    override get day() {
        return 7;
    }

    static getDirectorySizes(
        terminalOutput: string[]
    ): [totalSizeOfDirectoriesLargerThanLimit: number, sizeOfSmallestDirectoryToDelete: number] {
        const parseEntries = (current: Directory, terminalOutput: string[]) => {
            let count = 0;

            for (let i = 0; i < terminalOutput.length; i++) {
                const item = terminalOutput[i];

                if (item.startsWith('$')) {
                    count = i;
                    break;
                } else if (item.startsWith('dir ')) {
                    current.children.push(new Directory(item.slice(4), current));
                } else {
                    const split = item.split(' ');
                    current.children.push(new File(split[1], current, Puzzle.parse(split[0])));
                }
            }

            return count;
        };

        const getDirectories = (terminalOutput: string[]) => {
            const root = new Directory('/');
            let current = root;

            const fileSystem = new Map<string, Directory>();
            fileSystem.set(root.path, root);

            for (let i = 0; i < terminalOutput.length; i++) {
                const line = terminalOutput[i];

                if (line.startsWith('$ cd ')) {
                    const name = line.slice(5);

                    if (name === '..') {
                        current = current.parent;
                    } else {
                        let path;

                        if (name === '/') {
                            path = name;
                        } else {
                            path = current.path.slice(0, -1) + '/' + name;
                        }

                        let directory: Directory;

                        if (fileSystem.has(path)) {
                            directory = fileSystem.get(path);
                        } else {
                            directory = new Directory(name, current);
                            fileSystem.set(directory.path, directory);
                        }

                        current = directory;
                    }
                } else if (line.startsWith('$ ls')) {
                    i += parseEntries(current, terminalOutput.slice(i + 1));
                    for (const child of current.children) {
                        if (child instanceof Directory) {
                            fileSystem.set(child.path, child);
                        }
                    }
                }
            }

            const result: Directory[] = [];

            for (const directory of fileSystem.values()) {
                result.push(directory);
            }

            return result;
        };

        const diskSize = 70000000;
        const sizeLimit = 100000;
        const updateSize = 30000000;

        const directories = getDirectories(terminalOutput);

        const totalSizeOfDirectoriesLargerThanLimit = directories
            .filter((p) => p.totalSize <= sizeLimit)
            .map((p) => p.totalSize)
            .reduce((x, y) => x + y, 0);

        const consumed = directories.find((p) => p.container === null).totalSize;
        const freeSpace = diskSize - consumed;
        const required = updateSize - freeSpace;

        const sizeOfSmallestDirectoryToDelete = directories
            .filter((p) => p.totalSize >= required)
            .map((p) => p.totalSize)
            .sort((x, y) => x - y)[0];

        return [totalSizeOfDirectoriesLargerThanLimit, sizeOfSmallestDirectoryToDelete];
    }

    override solveCore(_: string[]) {
        const terminalOutput = this.readResourceAsLines();

        [this.totalSizeOfDirectoriesLargerThanLimit, this.sizeOfSmallestDirectoryToDelete] = Day07.getDirectorySizes(terminalOutput);

        console.info(
            `The sum of the total sizes of the directories with a total size of at most 100,000 is ${this.totalSizeOfDirectoriesLargerThanLimit}.`
        );
        console.info(
            `The sizes of the directory would free up enough space on the filesystem to run the update is ${this.sizeOfSmallestDirectoryToDelete}.`
        );

        return this.createResult([this.totalSizeOfDirectoriesLargerThanLimit, this.sizeOfSmallestDirectoryToDelete]);
    }
}

abstract class FileSystemEntry {
    readonly path: string;

    constructor(readonly name: string, readonly container: FileSystemEntry | null) {
        if (this.container === null) {
            this.path = this.name;
        } else {
            this.path = this.container.path.slice(0, -1) + '/' + this.name;
        }
    }
}

class Directory extends FileSystemEntry {
    constructor(name: string, readonly parent: Directory | null = null, readonly children: FileSystemEntry[] = []) {
        super(name, parent);
    }

    get totalSize() {
        let size = 0;

        for (const child of this.children) {
            if (child instanceof Directory) {
                size += child.totalSize;
            } else if (child instanceof File) {
                size += child.size;
            }
        }

        return size;
    }
}

class File extends FileSystemEntry {
    constructor(name: string, directory: Directory, readonly size: number) {
        super(name, directory);
    }
}
