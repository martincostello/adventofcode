// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { describe, expect, test } from '@jest/globals';
import { CharacterRecognition } from './index';

describe('Character Recognition', () => {
    test('recognizes all the letters', () => {
        // Arrange
        const letters = [
            '.**..***...**..****.****..**..*..*..***...**.*..*.*.....**..***..***..*..*.*...*****.',
            '*..*.*..*.*..*.*....*....*..*.*..*...*.....*.*.*..*....*..*.*..*.*..*.*..*.*...*...*.',
            '*..*.***..*....***..***..*....****...*.....*.**...*....*..*.*..*.*..*.*..*..*.*...*..',
            '****.*..*.*....*....*....*.**.*..*...*.....*.*.*..*....*..*.***..***..*..*...*...*...',
            '*..*.*..*.*..*.*....*....*..*.*..*...*..*..*.*.*..*....*..*.*....*.*..*..*...*..*....',
            '*..*.***...**..****.*.....***.*..*..***..**..*..*.****..**..*....*..*..**....*..****.',
        ];

        // Act
        const actual = CharacterRecognition.readString(letters.join('\n'));

        // Assert
        expect(actual).toBe('ABCEFGHIJKLOPRUYZ');
    });
});
