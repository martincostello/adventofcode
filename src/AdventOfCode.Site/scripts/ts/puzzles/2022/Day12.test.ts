// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day12 } from './index';

describe('2022', () => {
    describe('Day 12', () => {
        test('returns correct values', () => {
            // Arrange
            const heightmap = ['Sabqponm', 'abcryxxl', 'accszExk', 'acctuvwj', 'abdefghi'];

            // Act
            const [actualFromStart, actualFromGround] = Day12.getMinimumSteps(heightmap);

            // Assert
            expect(actualFromStart).toBe(31);
            expect(actualFromGround).toBe(29);
        });
    });
});
