// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day07 } from './index';

describe('2022', () => {
    describe('Day 07', () => {
        test('returns correct values', () => {
            // Arrange
            const terminalOutput = [
                '$ cd /',
                '$ ls',
                'dir a',
                '14848514 b.txt',
                '8504156 c.dat',
                'dir d',
                '$ cd a',
                '$ ls',
                'dir e',
                '29116 f',
                '2557 g',
                '62596 h.lst',
                '$ cd e',
                '$ ls',
                '584 i',
                '$ cd ..',
                '$ cd ..',
                '$ cd d',
                '$ ls',
                '4060174 j',
                '8033020 d.log',
                '5626152 d.ext',
                '7214296 k',
            ];

            // Act
            const [actualTotalSizeBelowLimit, actualFreedSpace] = Day07.getDirectorySizes(terminalOutput);

            // Assert
            expect(actualTotalSizeBelowLimit).toBe(95437);
            expect(actualFreedSpace).toBe(24933642);
        });
    });
});
