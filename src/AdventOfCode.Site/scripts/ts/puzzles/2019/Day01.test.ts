// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day01 } from './index';

describe('2019', () => {
    describe('Day 01', () => {
        test.each([
            [12, false, 2],
            [14, false, 2],
            [1969, false, 654],
            [100756, false, 33583],
            [12, true, 2],
            [1969, true, 966],
            [100756, true, 50346],
        ])('returns correct requirement for mass %s with include fuel %s', (mass: number, includeFuel: boolean, expected: number) => {
            // Act
            const actual = Day01.getFuelRequirementsForMass(mass, includeFuel);

            // Assert
            expect(actual).toEqual(expected);
        });
    });
});
