// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day02 } from './index';

describe('2022', () => {
    describe('Day 02', () => {
        test.each([
            [false, 15],
            [true, 12],
        ])('returns correct total score with containsDesiredOutcome %s', (containsDesiredOutcome: boolean, expected: number) => {
            // Arrange
            const moves = ['A Y', 'B X', 'C Z'];

            // Act
            const actual = Day02.getTotalScore(moves, containsDesiredOutcome);

            // Assert
            expect(actual).toBe(expected);
        });
    });
});
