// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Point } from './Point';

export class CharacterRecognition {
    static read(array: Array<string>[], ink = '*') {
        const width = array[0].length;
        const height = array.length;

        const bits: Array<boolean>[] = [];

        for (let y = 0; y < height; y++) {
            const row = new Array<boolean>(width);

            for (let x = 0; x < width; x++) {
                row[x] = array[y][x] === ink;
            }

            bits.push(row);
        }

        return CharacterRecognition.readBits(bits);
    }

    private static readBits(array: Array<boolean>[]) {
        const width = array[0].length;
        const height = array.length;

        if (width % 5 !== 0 || height % 6 !== 0) {
            return '';
        }

        let result = '';

        for (let x = 0; x < width - 1; x += 5) {
            const letter = new Set<Point>();

            for (let y = 0; y < height; y++) {
                if (array[y][x]) {
                    letter.add(new Point(0, y));
                }

                if (array[y][x + 1]) {
                    letter.add(new Point(1, y));
                }

                if (array[y][x + 2]) {
                    letter.add(new Point(2, y));
                }

                if (array[y][x + 3]) {
                    letter.add(new Point(3, y));
                }

                if (array[y][x + 4]) {
                    letter.add(new Point(4, y));
                }
            }

            result += Alphabet.get(letter);
        }

        return result;
    }
}

class Alphabet {
    private static readonly alphabet = Alphabet.createAlphabet();

    public static get(points: Set<Point>) {
        const set = new Set<string>();
        for (const point of points) {
            set.add(point.toString());
        }

        for (const [letter, glyph] of Alphabet.alphabet) {
            if (Alphabet.setsAreEqual(set, glyph)) {
                return letter;
            }
        }

        return '?';
    }

    // Based on https://bobbyhadz.com/blog/javascript-check-if-two-sets-are-equal
    private static setsAreEqual(a: Set<string>, b: Set<string>) {
        if (a.size !== b.size) {
            return false;
        }

        return Array.from(a).every((element) => {
            return b.has(element);
        });
    }

    private static createAlphabet(): Map<string, Set<string>> {
        const alphabet = new Map<string, Point[]>();
        alphabet.set('A', [
            new Point(1, 0),
            new Point(2, 0),
            new Point(0, 1),
            new Point(3, 1),
            new Point(0, 2),
            new Point(3, 2),
            new Point(0, 3),
            new Point(1, 3),
            new Point(2, 3),
            new Point(3, 3),
            new Point(0, 4),
            new Point(3, 4),
            new Point(0, 5),
            new Point(3, 5),
        ]);

        alphabet.set('B', [
            new Point(0, 0),
            new Point(1, 0),
            new Point(2, 0),
            new Point(0, 1),
            new Point(3, 1),
            new Point(0, 2),
            new Point(1, 2),
            new Point(2, 2),
            new Point(0, 3),
            new Point(3, 3),
            new Point(0, 4),
            new Point(3, 4),
            new Point(0, 5),
            new Point(1, 5),
            new Point(2, 5),
        ]);

        alphabet.set('C', [
            new Point(1, 0),
            new Point(2, 0),
            new Point(0, 1),
            new Point(3, 1),
            new Point(0, 2),
            new Point(0, 3),
            new Point(0, 4),
            new Point(3, 4),
            new Point(1, 5),
            new Point(2, 5),
        ]);

        alphabet.set('E', [
            new Point(0, 0),
            new Point(1, 0),
            new Point(2, 0),
            new Point(3, 0),
            new Point(0, 1),
            new Point(0, 2),
            new Point(1, 2),
            new Point(2, 2),
            new Point(0, 3),
            new Point(0, 4),
            new Point(0, 5),
            new Point(1, 5),
            new Point(2, 5),
            new Point(3, 5),
        ]);

        alphabet.set('F', [
            new Point(0, 0),
            new Point(1, 0),
            new Point(2, 0),
            new Point(3, 0),
            new Point(0, 1),
            new Point(0, 2),
            new Point(1, 2),
            new Point(2, 2),
            new Point(0, 3),
            new Point(0, 4),
            new Point(0, 5),
        ]);

        alphabet.set('G', [
            new Point(1, 0),
            new Point(2, 0),
            new Point(0, 1),
            new Point(3, 1),
            new Point(0, 2),
            new Point(0, 3),
            new Point(2, 3),
            new Point(3, 3),
            new Point(0, 4),
            new Point(3, 4),
            new Point(1, 5),
            new Point(2, 5),
            new Point(3, 5),
        ]);

        alphabet.set('H', [
            new Point(0, 0),
            new Point(3, 0),
            new Point(0, 1),
            new Point(3, 1),
            new Point(0, 2),
            new Point(1, 2),
            new Point(2, 2),
            new Point(3, 2),
            new Point(0, 3),
            new Point(3, 3),
            new Point(0, 4),
            new Point(3, 4),
            new Point(0, 5),
            new Point(3, 5),
        ]);

        alphabet.set('I', [
            new Point(1, 0),
            new Point(2, 0),
            new Point(3, 0),
            new Point(2, 1),
            new Point(2, 2),
            new Point(2, 3),
            new Point(2, 4),
            new Point(1, 5),
            new Point(2, 5),
            new Point(3, 5),
        ]);

        alphabet.set('J', [
            new Point(2, 0),
            new Point(3, 0),
            new Point(3, 1),
            new Point(3, 2),
            new Point(3, 3),
            new Point(0, 4),
            new Point(3, 4),
            new Point(1, 5),
            new Point(2, 5),
        ]);

        alphabet.set('K', [
            new Point(0, 0),
            new Point(3, 0),
            new Point(0, 1),
            new Point(2, 1),
            new Point(0, 2),
            new Point(1, 2),
            new Point(0, 3),
            new Point(2, 3),
            new Point(0, 4),
            new Point(2, 4),
            new Point(0, 5),
            new Point(3, 5),
        ]);

        alphabet.set('L', [
            new Point(0, 0),
            new Point(0, 1),
            new Point(0, 2),
            new Point(0, 3),
            new Point(0, 4),
            new Point(0, 5),
            new Point(1, 5),
            new Point(2, 5),
            new Point(3, 5),
        ]);

        alphabet.set('O', [
            new Point(1, 0),
            new Point(2, 0),
            new Point(0, 1),
            new Point(3, 1),
            new Point(0, 2),
            new Point(3, 2),
            new Point(0, 3),
            new Point(3, 3),
            new Point(0, 4),
            new Point(3, 4),
            new Point(1, 5),
            new Point(2, 5),
        ]);

        alphabet.set('P', [
            new Point(0, 0),
            new Point(1, 0),
            new Point(2, 0),
            new Point(0, 1),
            new Point(3, 1),
            new Point(0, 2),
            new Point(3, 2),
            new Point(0, 3),
            new Point(1, 3),
            new Point(2, 3),
            new Point(0, 4),
            new Point(0, 5),
        ]);

        alphabet.set('R', [
            new Point(0, 0),
            new Point(1, 0),
            new Point(2, 0),
            new Point(0, 1),
            new Point(3, 1),
            new Point(0, 2),
            new Point(3, 2),
            new Point(0, 3),
            new Point(1, 3),
            new Point(2, 3),
            new Point(0, 4),
            new Point(2, 4),
            new Point(0, 5),
            new Point(3, 5),
        ]);

        alphabet.set('U', [
            new Point(0, 0),
            new Point(3, 0),
            new Point(0, 1),
            new Point(3, 1),
            new Point(0, 2),
            new Point(3, 2),
            new Point(0, 3),
            new Point(3, 3),
            new Point(0, 4),
            new Point(3, 4),
            new Point(1, 5),
            new Point(2, 5),
        ]);

        alphabet.set('Y', [
            new Point(0, 0),
            new Point(4, 0),
            new Point(0, 1),
            new Point(4, 1),
            new Point(1, 2),
            new Point(3, 2),
            new Point(2, 3),
            new Point(2, 4),
            new Point(2, 5),
        ]);

        alphabet.set('Z', [
            new Point(0, 0),
            new Point(1, 0),
            new Point(2, 0),
            new Point(3, 0),
            new Point(3, 1),
            new Point(2, 2),
            new Point(1, 3),
            new Point(0, 4),
            new Point(0, 5),
            new Point(1, 5),
            new Point(2, 5),
            new Point(3, 5),
        ]);

        const result = new Map<string, Set<string>>();

        for (const [letter, glyph] of alphabet) {
            const set = new Set<string>();
            for (const point of glyph) {
                set.add(point.toString());
            }
            result.set(letter, set);
        }

        return result;
    }
}
