// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day08 } from './index';

describe('2022', () => {
    describe('Day 08', () => {
        test('returns correct values', () => {
            // Arrange
            const grid = ['30373', '25512', '65332', '33549', '35390'];

            // Act
            const [actualCount, actualScore] = Day08.countVisibleTrees(grid);

            // Assert
            expect(actualCount).toBe(21);
            expect(actualScore).toBe(8);
        });
    });
});
