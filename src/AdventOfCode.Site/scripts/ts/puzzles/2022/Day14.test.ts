// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day14 } from './index';

describe('2022', () => {
    describe('Day 14', () => {
        test.each([
            [false, 24],
            [true, 93],
        ])('returns correct value for has floor %s', (hasFloor: boolean, expected: number) => {
            // Arrange
            const paths = ['498,4 -> 498,6 -> 496,6', '503,4 -> 502,4 -> 502,9 -> 494,9'];

            // Act
            const [actual] = Day14.simulate(paths, hasFloor);

            // Assert
            expect(actual).toBe(expected);
        });
    });
});
