// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day07 } from './index';

describe('2015', () => {
    describe('Day 07', () => {
        test.each([
            ['d', 72],
            ['e', 507],
            ['f', 492],
            ['g', 114],
            ['h', 65412],
            ['i', 65079],
            ['j', 65079],
            ['k', 65079],
            ['x', 123],
            ['y', 456],
        ])('returns correct value for wire %s', (wire: string, expected: number) => {
            // Arrange
            const instructions = [
                'j -> k',
                '123 -> x',
                '456 -> y',
                'x AND y -> d',
                'x OR y -> e',
                'x LSHIFT 2 -> f',
                'y RSHIFT 2 -> g',
                'NOT x -> h',
                'NOT y -> i',
                'i -> j',
            ];

            // Act
            const actual = Day07.getWireValues(instructions);

            // Assert
            expect(actual.has(wire)).toBe(true);
            expect(actual.get(wire)).toEqual(expected);
        });
    });
});
