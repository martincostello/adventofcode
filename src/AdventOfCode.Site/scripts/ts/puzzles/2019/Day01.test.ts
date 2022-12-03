// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day01 } from './Day01';

describe('2019', () => {
    describe('Day 01', () => {
        test.each([
            [12, 2],
            [14, 2],
            [1969, 654],
            [100756, 33583],
        ])('returns correct requirement for mass %s', (mass: number, expected: number) => {
            // Act
            const actual = Day01.getFuelRequirementsForMass(mass);

            // Assert
            expect(actual).toEqual(expected);
        });
        test.each([
            [12, 2],
            [1969, 966],
            [100756, 50346],
        ])('returns correct requirement with mass %s with fuel', (mass: number, expected: number) => {
            // Act
            const actual = Day01.getFuelRequirementsForMassWithFuel(mass);

            // Assert
            expect(actual).toEqual(expected);
        });
    });
});
