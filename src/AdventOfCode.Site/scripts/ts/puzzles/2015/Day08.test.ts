// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day08 } from './index';

describe('2015', () => {
    describe('Day 08', () => {
        test.each([
            [['""'], 0],
            [['"abc"'], 3],
            [['"aaa\\"aaa"'], 7],
            [['"\\x27"'], 1],
            [['"v\\xfb"lgs"kvjfywmut\\x9cr"'], 18],
            [['"d\\\\gkbqo\\\\fwukyxab"u"'], 18],
            [['""', '"abc"', '"aaa\\"aaa"', '"\\x27"'], 11],
        ])('returns correct value for literal values %s', (values: string[], expected: number) => {
            // Act
            const actual = Day08.getLiteralCharacterCount(...values);

            // Assert
            expect(actual).toEqual(expected);
        });
        test.each([
            [['""'], 6],
            [['"abc"'], 9],
            [['"aaa\\"aaa"'], 16],
            [['"\\x27"'], 11],
            [['""', '"abc"', '"aaa\\"aaa"', '"\\x27"'], 42],
        ])('returns correct value for encoded values %s', (values: string[], expected: number) => {
            // Act
            const actual = Day08.getEncodedCharacterCount(...values);

            // Assert
            expect(actual).toEqual(expected);
        });
    });
});
