// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day01 } from './index';

describe('2018', () => {
    describe('Day 01', () => {
        test.each([
            [[1, -2, 3, 1], 3],
            [[1, 1, 1], 3],
            [[1, 1, -2], 0],
            [[-1, -2, -3], -6],
        ])('returns correct frequency for %s', (sequence: number[], expected: number) => {
            // Act
            const actual = Day01.calculateFrequency(sequence);

            // Assert
            expect(actual).toEqual(expected);
        });
        test.each([
            [[1, -2, 3, 1, 1, -2], 2],
            [[1, -1], 0],
            [[3, 3, 4, -2, -4], 10],
            [[-6, 3, 8, 5, -6], 5],
            [[7, 7, -2, -7, -4], 14],
        ])('returns correct frequency with repetition for %s', (sequence: number[], expected: number) => {
            // Act
            const [_, actual] = Day01.calculateFrequencyWithRepetition(sequence);

            // Assert
            expect(actual).toEqual(expected);
        });
    });
});
