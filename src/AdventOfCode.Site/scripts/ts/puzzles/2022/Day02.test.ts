// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { readFileSync } from 'fs';
import { join, resolve } from 'path';
import { Puzzle } from '../Puzzle';
import { Day02 } from './Day02';

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
    describe('Day 02', () => {
        test.each([
            [false, 15],
            [true, 12],
        ])('returns correct total score', (containsDesiredOutcome: boolean, expected: number) => {
            // Arrange
            const moves = ['A Y', 'B X', 'C Z'];

            // Act
            const actual = Day02.getTotalScore(moves, containsDesiredOutcome);

            // Assert
            expect(actual).toBe(expected);
        });
        test('returns the correct solution', async () => {
            // Act
            const puzzle = await solvePuzzle(Day02);

            // Assert
            expect(puzzle.totalScoreForMoves).toBe(13675);
            expect(puzzle.totalScoreForOutcomes).toBe(14184);
        });
    });
});
