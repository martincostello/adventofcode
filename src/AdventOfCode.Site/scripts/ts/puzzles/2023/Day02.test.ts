// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day02 } from './index';

describe('2023', () => {
    describe('Day 02', () => {
        test('returns correct values', () => {
            // Arrange
            const games = [
                'Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green',
                'Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue',
                'Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red',
                'Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red',
                'Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green',
            ];

            // Act
            const [actualIdsSum, actualPowersSum] = Day02.play(games);

            // Assert
            expect(actualIdsSum).toBe(8);
            expect(actualPowersSum).toBe(2286);
        });
    });
});
