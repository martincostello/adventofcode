// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day04 } from './index';

describe('2022', () => {
    describe('Day 04', () => {
        test.each([
            [false, 2],
            [true, 4],
        ])('returns correct number of pairs that overlap with partial %s', (partial: boolean, expected: number) => {
            // Arrange
            const assignments = ['2-4,6-8', '2-3,4-5', '5-7,7-9', '2-8,3-7', '6-6,4-6', '2-6,4-8'];

            // Act
            const actual = Day04.getOverlappingAssignments(assignments, partial);

            // Assert
            expect(actual).toBe(expected);
        });
    });
});
