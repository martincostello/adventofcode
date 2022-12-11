// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

export class CharacterRecognition {
    static readString(text: string, ink = '*') {
        const lines = text.split('\n');

        const width = lines[0].length;
        const height = lines.length;

        const chars: Array<string>[] = [];

        for (let y = 0; y < height; y++) {
            const row = new Array<string>(width);

            for (let x = 0; x < width; x++) {
                row[x] = lines[y][x];
            }

            chars.push(row);
        }

        return CharacterRecognition.readCharacters(chars, ink);
    }

    static readCharacters(array: Array<string>[], ink = '*') {
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

        for (let offset = 0; offset < width - 1; offset += 5) {
            let letter = 0;
            let bit = 30;

            for (let x = 0; x < 5; x++) {
                for (let y = 0; y < height; y++) {
                    if (array[y][offset + x]) {
                        letter |= 1 << bit;
                    }
                    bit--;
                }
            }

            result += Alphabet.get(letter);
        }

        return result;
    }
}

class Alphabet {
    private static readonly alphabet = Alphabet.createAlphabet();

    public static get(value: number) {
        if (this.alphabet.has(value)) {
            return this.alphabet.get(value);
        }

        return '?';
    }

    private static createAlphabet() {
        const alphabet = new Map<number, string>();

        // Letters are encoded as a 32-bit number where the top-left pixel
        // is the 31st bit and the bottom - right pixel is the 2nd bit, then
        // one zero. The bits are encoded top-to-bottom, left-to-right.
        alphabet.set(parseInt('00111111001001001000111110000000', 2), 'A');
        alphabet.set(parseInt('01111111010011010010101100000000', 2), 'B');
        alphabet.set(parseInt('00111101000011000010100100000000', 2), 'C');
        alphabet.set(parseInt('01111111010011010011000010000000', 2), 'E');
        alphabet.set(parseInt('01111111010001010001000000000000', 2), 'F');
        alphabet.set(parseInt('00111101000011001010101110000000', 2), 'G');
        alphabet.set(parseInt('01111110010000010001111110000000', 2), 'H');
        alphabet.set(parseInt('00000001000011111111000010000000', 2), 'I');
        alphabet.set(parseInt('00000100000011000011111100000000', 2), 'J');
        alphabet.set(parseInt('01111110010000101101000010000000', 2), 'K');
        alphabet.set(parseInt('01111110000010000010000010000000', 2), 'L');
        alphabet.set(parseInt('00111101000011000010111100000000', 2), 'O');
        alphabet.set(parseInt('01111111001001001000110000000000', 2), 'P');
        alphabet.set(parseInt('01111111001001001100110010000000', 2), 'R');
        alphabet.set(parseInt('01111100000010000011111100000000', 2), 'U');
        alphabet.set(parseInt('01100000010000001110010001100000', 2), 'Y');
        alphabet.set(parseInt('01000111001011010011100010000000', 2), 'Z');

        return alphabet;
    }
}
