// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { PuzzleSolver } from '../PuzzleSolver';
import { Day02 } from './Day02';

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
            const puzzle = await PuzzleSolver.solve(Day02);

            // Assert
            expect(puzzle.totalScoreForMoves).toBe(13675);
            expect(puzzle.totalScoreForOutcomes).toBe(14184);
        });
    });
});
