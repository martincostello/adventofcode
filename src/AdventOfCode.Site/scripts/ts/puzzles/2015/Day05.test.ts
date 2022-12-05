// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day05 } from './index';

describe('2015', () => {
    describe('Day 05', () => {
        test.each([
            ['ugknbfddgicrmopn', '1', true],
            ['aaa', '1', true],
            ['jchzalrnumimnmhp', '1', false],
            ['haegwjzuvuyypxyu', '1', false],
            ['dvszwmarrgswjxmb', '1', false],
            ['haegwjzuvuyypabu', '1', false],
            ['haegwjzuvuyypcdu', '1', false],
            ['haegwjzuvuyyppqu', '1', false],
            ['qjhvhtzxzqqjkmpb', '2', true],
            ['xxyxx', '2', true],
            ['uurcxstgmygtbstg', '2', false],
            ['ieodomkazucvgmuy', '2', false],
        ])('returns correct value for value %s and version %s', (value: string, version: '1' | '2', expected: boolean) => {
            // Act
            const actual = Day05.isNice(value, version);

            // Assert
            expect(actual).toEqual(expected);
        });
    });
});
