// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { PuzzleSolver } from '../PuzzleSolver';
import { Day03 } from './Day03';

describe('2022', () => {
    describe('Day 03', () => {
        test.each([
            [false, 157],
            [true, 70],
        ])('returns correct sum of priorities with using groups %s', (useGroups: boolean, expected: number) => {
            // Arrange
            const inventories = [
                'vJrwpWtwJgWrhcsFMMfFFhFp',
                'jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL',
                'PmmdzqPrVvPwwTWBwg',
                'wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn',
                'ttgJtRGJQctTZtZT',
                'CrZsJsPPZsGzwwsLwLmpwMDw',
            ];

            // Act
            const actual = Day03.getSumOfCommonItemTypes(inventories, useGroups);

            // Assert
            expect(actual).toBe(expected);
        });
        test('returns the correct solution', async () => {
            // Act
            const puzzle = await PuzzleSolver.solve(Day03);

            // Assert
            expect(puzzle.sumOfPriorities).toBe(7568);
            expect(puzzle.sumOfPrioritiesOfGroups).toBe(2780);
        });
    });
});
