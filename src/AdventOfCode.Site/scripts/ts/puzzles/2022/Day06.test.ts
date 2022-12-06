// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { Day06 } from './index';

describe('2022', () => {
    describe('Day 06', () => {
        test.each([
            ['mjqjpqmgbljsphdztnvjfqwrcgsmlb', 4, 7],
            ['bvwbjplbgvbhsrlpgdmjqwftvncz', 4, 5],
            ['nppdvjthqldpwncqszvftbrmjlhg', 4, 6],
            ['nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg', 4, 10],
            ['zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw', 4, 11],
            ['mjqjpqmgbljsphdztnvjfqwrcgsmlb', 14, 19],
            ['bvwbjplbgvbhsrlpgdmjqwftvncz', 14, 23],
            ['nppdvjthqldpwncqszvftbrmjlhg', 14, 23],
            ['nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg', 14, 29],
            ['zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw', 14, 26],
        ])('returns correct index for datastream %s and count %s', (datastream: string, distinctCharacters: number, expected: number) => {
            // Act
            const actual = Day06.findFirstPacket(datastream, distinctCharacters);

            // Assert
            expect(actual).toBe(expected);
        });
    });
});
