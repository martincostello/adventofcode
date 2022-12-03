// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day01 } from './Day01';

describe('2020', () => {
    describe('Day 01', () => {
        test.each([
            [2, 514579],
            [3, 241861950],
        ])('returns correct product for take %s', (take: number, expected: number) => {
            // Arrange
            const values = [1721, 979, 366, 299, 675, 1456];

            // Act
            const actual = Day01.get2020Product(values, take);

            // Assert
            expect(actual).toEqual(expected);
        });
    });
});
