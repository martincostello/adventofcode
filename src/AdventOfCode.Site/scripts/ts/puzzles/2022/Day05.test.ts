// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day05 } from './index';

describe('2022', () => {
    describe('Day 05', () => {
        test.each([
            [false, 'CMZ'],
            [true, 'MCD'],
        ])('returns correct arrangement with can move multiple crates %s', (canMoveMultipleCrates: boolean, expected: string) => {
            // Arrange
            const instructions = [
                '    [D]     ',
                '[N] [C]     ',
                '[Z] [M] [P] ',
                ' 1   2   3  ',
                '',
                'move 1 from 2 to 1',
                'move 3 from 1 to 3',
                'move 2 from 2 to 1',
                'move 1 from 1 to 2',
            ];

            // Act
            const actual = Day05.rearrangeCrates(instructions, canMoveMultipleCrates);

            // Assert
            expect(actual).toBe(expected);
        });
    });
});
