// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day09 } from './index';

describe('2022', () => {
    describe('Day 09', () => {
        test.each([
            [['R 4', 'U 4', 'L 3', 'D 1', 'R 4', 'D 1', 'L 5', 'R 2'], 2, 13],
            [['R 5', 'U 8', 'L 8', 'D 3', 'R 17', 'D 10', 'L 25', 'U 20'], 10, 36],
        ])('returns correct value for %s knots', (moves: string[], knots: number, expected: number) => {
            // Act
            const actual = Day09.move(moves, knots);

            // Assert
            expect(actual).toBe(expected);
        });
    });
});
