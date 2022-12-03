// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day01 } from './Day01';

describe('2021', () => {
    describe('Day 01', () => {
        test.each([
            [false, 7],
            [true, 5],
        ])('returns correct increases with useSlidingWindow %s', (useSlidingWindow: boolean, expected: number) => {
            // Arrange
            const depthMeasurements = [199, 200, 208, 210, 200, 207, 240, 269, 260, 263];

            // Act
            const actual = Day01.getDepthMeasurementIncreases(depthMeasurements, useSlidingWindow);

            // Assert
            expect(actual).toEqual(expected);
        });
    });
});
