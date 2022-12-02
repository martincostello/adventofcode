// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { PuzzleSolver } from '../PuzzleSolver';
import { Day01 } from './Day01';

describe('2022', () => {
    describe('Day 01', () => {
        test('returns correct calorie inventory', () => {
            // Arrange
            const inventory = ['1000', '2000', '3000', '', '4000', '', '5000', '6000', '', '7000', '8000', '9000', '', '10000'];

            // Act
            const actual = Day01.getCalorieInventories(inventory);

            // Assert
            expect(actual).toEqual([6000, 4000, 11000, 24000, 10000]);
        });
        test('returns the correct solution', async () => {
            // Act
            const puzzle = await PuzzleSolver.solve(Day01);

            // Assert
            expect(puzzle).not.toBeNull();
            expect(puzzle.maximumCalories).toBe(68775);
            expect(puzzle.maximumCaloriesForTop3).toBe(202585);
        });
    });
});
