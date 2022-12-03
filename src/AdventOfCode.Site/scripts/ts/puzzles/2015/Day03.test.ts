// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day03 } from './Day03';

describe('2015', () => {
    describe('Day 03', () => {
        test.each([
            ['>', 1],
            ['^>v<', 4],
            ['^v^v^v^v^v', 2],
        ])('returns correct count for Santa with route "%s"', (instructions: string, expected: number) => {
            // Act
            const actual = Day03.getUniqueHousesVisitedBySanta(instructions);

            // Assert
            expect(actual).toEqual(expected);
        });
        test.each([
            ['^v', 3],
            ['^>v<', 3],
            ['^v^v^v^v^v', 11],
        ])('returns correct count for Santa and Robo Santa route "%s"', (instructions: string, expected: number) => {
            // Act
            const actual = Day03.getUniqueHousesVisitedBySantaAndRoboSanta(instructions);

            // Assert
            expect(actual).toEqual(expected);
        });
    });
});
