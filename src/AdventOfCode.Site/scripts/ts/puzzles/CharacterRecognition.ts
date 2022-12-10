// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

export class CharacterRecognition {
    static readString(array: Array<string>[], ink = '*') {
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

    static readBits(array: Array<boolean>[]) {
        const width = array[0].length;
        const height = array.length;

        if (width % 5 !== 0 || height % 6 !== 0) {
            return '';
        }

        let result = '';

        for (let x = 0; x < width - 1; x += 5) {
            const letter = new Set<string>();

            for (let y = 0; y < height; y++) {
                for (let z = 0; z < 5; z++) {
                    if (array[y][x + z]) {
                        letter.add(`${z},${y}`);
                    }
                }
            }

            result += Alphabet.get(letter);
        }

        return result;
    }
}

class Alphabet {
    private static readonly alphabet = Alphabet.createAlphabet();

    public static get(points: Set<string>) {
        for (const [letter, glyph] of Alphabet.alphabet) {
            if (Alphabet.setsAreEqual(points, glyph)) {
                return letter;
            }
        }

        return '?';
    }

    // Based on https://bobbyhadz.com/blog/javascript-check-if-two-sets-are-equal
    private static setsAreEqual<T>(a: Set<T>, b: Set<T>) {
        if (a.size !== b.size) {
            return false;
        }

        return Array.from(a).every((element) => {
            return b.has(element);
        });
    }

    private static createAlphabet(): Map<string, Set<string>> {
        const alphabet = new Map<string, Set<string>>();
        alphabet.set(
            'A',
            new Set<string>(['1,0', '2,0', '0,1', '3,1', '0,2', '3,2', '0,3', '1,3', '2,3', '3,3', '0,4', '3,4', '0,5', '3,5'])
        );

        alphabet.set(
            'B',
            new Set<string>(['0,0', '1,0', '2,0', '0,1', '3,1', '0,2', '1,2', '2,2', '0,3', '3,3', '0,4', '3,4', '0,5', '1,5', '2,5'])
        );

        alphabet.set('C', new Set<string>(['1,0', '2,0', '0,1', '3,1', '0,2', '0,3', '0,4', '3,4', '1,5', '2,5']));

        alphabet.set(
            'E',
            new Set<string>(['0,0', '1,0', '2,0', '3,0', '0,1', '0,2', '1,2', '2,2', '0,3', '0,4', '0,5', '1,5', '2,5', '3,5'])
        );

        alphabet.set('F', new Set<string>(['0,0', '1,0', '2,0', '3,0', '0,1', '0,2', '1,2', '2,2', '0,3', '0,4', '0,5']));
        alphabet.set('G', new Set<string>(['1,0', '2,0', '0,1', '3,1', '0,2', '0,3', '2,3', '3,3', '0,4', '3,4', '1,5', '2,5', '3,5']));

        alphabet.set(
            'H',
            new Set<string>(['0,0', '3,0', '0,1', '3,1', '0,2', '1,2', '2,2', '3,2', '0,3', '3,3', '0,4', '3,4', '0,5', '3,5'])
        );

        alphabet.set('I', new Set<string>(['1,0', '2,0', '3,0', '2,1', '2,2', '2,3', '2,4', '1,5', '2,5', '3,5']));
        alphabet.set('J', new Set<string>(['2,0', '3,0', '3,1', '3,2', '3,3', '0,4', '3,4', '1,5', '2,5']));
        alphabet.set('K', new Set<string>(['0,0', '3,0', '0,1', '2,1', '0,2', '1,2', '0,3', '2,3', '0,4', '2,4', '0,5', '3,5']));
        alphabet.set('L', new Set<string>(['0,0', '0,1', '0,2', '0,3', '0,4', '0,5', '1,5', '2,5', '3,5']));
        alphabet.set('O', new Set<string>(['1,0', '2,0', '0,1', '3,1', '0,2', '3,2', '0,3', '3,3', '0,4', '3,4', '1,5', '2,5']));
        alphabet.set('P', new Set<string>(['0,0', '1,0', '2,0', '0,1', '3,1', '0,2', '3,2', '0,3', '1,3', '2,3', '0,4', '0,5']));

        alphabet.set(
            'R',
            new Set<string>(['0,0', '1,0', '2,0', '0,1', '3,1', '0,2', '3,2', '0,3', '1,3', '2,3', '0,4', '2,4', '0,5', '3,5'])
        );

        alphabet.set('U', new Set<string>(['0,0', '3,0', '0,1', '3,1', '0,2', '3,2', '0,3', '3,3', '0,4', '3,4', '1,5', '2,5']));
        alphabet.set('Y', new Set<string>(['0,0', '4,0', '0,1', '4,1', '1,2', '3,2', '2,3', '2,4', '2,5']));
        alphabet.set('Z', new Set<string>(['0,0', '1,0', '2,0', '3,0', '3,1', '2,2', '1,3', '0,4', '0,5', '1,5', '2,5', '3,5']));

        return alphabet;
    }
}
