// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { readFileSync } from 'fs';
import { join, resolve } from 'path';
import { Puzzle } from '../Puzzle';
import { Day01 } from './Day01';

async function solvePuzzle<T extends Puzzle>(type: { new (): T }, inputs: string[] = []): Promise<T> {
    // Arrange
    const puzzle = new type();

    const locale = 'en-US';
    const year = puzzle.year.toLocaleString(locale, {
        minimumIntegerDigits: 4,
        useGrouping: false,
    });

    const day = puzzle.day.toLocaleString(locale, {
        minimumIntegerDigits: 2,
        useGrouping: false,
    });

    let input = join(__dirname, `../../../../../AdventOfCode/Input/Y${year}/Day${day}/input.txt`);
    input = resolve(input);

    puzzle.resource = readFileSync(input, 'utf-8');

    // Act
    const result = await puzzle.solve(inputs);

    // Assert
    expect(result).not.toBeNull();
    expect(result.solutions).not.toBeNull();
    expect(result.solutions.length).toBeGreaterThan(0);

    return puzzle;
}

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
            const puzzle = await solvePuzzle(Day01);

            // Assert
            expect(puzzle).not.toBeNull();
            expect(puzzle.maximumCalories).toBe(68775);
            expect(puzzle.maximumCaloriesForTop3).toBe(202585);
        });
    });
});
