// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day11 } from './index';

describe('2022', () => {
    describe.skip('Day 11', () => {
        test.each([
            [20, false, 10605],
            [10000, true, 2713310158],
        ])('returns correct value for %s and %s', (rounds: number, highAnxiety: boolean, expected: number) => {
            // Arrange
            const observations = [
                'Monkey 0:',
                '  Starting items: 79, 98',
                '  Operation: new = old * 19',
                '  Test: divisible by 23',
                '    If true: throw to monkey 2',
                '    If false: throw to monkey 3',
                '',
                'Monkey 1:',
                '  Starting items: 54, 65, 75, 74',
                '  Operation: new = old + 6',
                '  Test: divisible by 19',
                '    If true: throw to monkey 2',
                '    If false: throw to monkey 0',
                '',
                'Monkey 2:',
                '  Starting items: 79, 60, 97',
                '  Operation: new = old * old',
                '  Test: divisible by 13',
                '    If true: throw to monkey 1',
                '    If false: throw to monkey 3',
                '',
                'Monkey 3:',
                '  Starting items: 74',
                '  Operation: new = old + 3',
                '  Test: divisible by 17',
                '    If true: throw to monkey 0',
                '    If false: throw to monkey 1',
            ];

            // Act
            const actual = Day11.getMonkeyBusiness(observations, rounds, highAnxiety);

            // Assert
            expect(actual).toBe(expected);
        });
    });
});
