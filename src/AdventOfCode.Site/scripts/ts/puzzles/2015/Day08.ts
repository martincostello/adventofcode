// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Puzzle2015 } from './Puzzle2015';

export class Day08 extends Puzzle2015 {
    firstSolution: number;
    secondSolution: number;

    override get name() {
        return 'Matchsticks';
    }

    override get day() {
        return 8;
    }

    static getEncodedCharacterCount(...collection: string[]) {
        let count = 0;

        for (const value of collection) {
            const encoded: Array<string> = ['"'];

            for (let i = 0; i < value.length; i++) {
                let current = value[i];

                switch (current) {
                    case '"':
                    case '\\':
                    case "'":
                        encoded.push('\\');
                        break;

                    default:
                        break;
                }

                encoded.push(current);
            }

            encoded.push('"');

            count += encoded.length;
        }

        return count;
    }

    static getLiteralCharacterCount(...collection: string[]) {
        let result = 0;

        for (let value of collection) {
            let count = 0;

            // Remove quotes if present as first/last characters
            let removeFirstQuote = value.length > 0 && value[0] === '"';
            let removeLastQuote = value.length > 1 && value[value.length - 1] === '"';

            if (removeFirstQuote) {
                value = value.slice(1);
            }

            if (removeLastQuote) {
                value = value.slice(0, -1);
            }

            if (value.length > 0) {
                const characters: Array<string> = [];

                for (let i = 0; i < value.length; i++) {
                    characters.push(value[i]);
                }

                while (characters.length > 0) {
                    let current = characters.shift();

                    if (characters.length > 0) {
                        switch (current) {
                            case '\\':
                                const next = characters[0];

                                if (next === '"' || next === "'" || next === '\\') {
                                    characters.shift();
                                } else if (next === 'x' && characters.length > 2) {
                                    characters.shift();
                                    characters.shift();
                                    characters.shift();
                                }

                                break;

                            default:
                                break;
                        }
                    }

                    count++;
                }
            }

            result += count;
        }

        return result;
    }

    override solveCore(_: string[]) {
        const input = this.readResourceAsLines();

        const countForCode = from(input).sum((p: string) => p.length);
        const countInMemory = Day08.getLiteralCharacterCount(...input);
        const countEncoded = Day08.getEncodedCharacterCount(...input);

        this.firstSolution = countForCode - countInMemory;
        this.secondSolution = countEncoded - countForCode;

        console.info(
            `The number of characters of code for string literals minus the number of characters in memory for the values of the strings is ${this.firstSolution}.`
        );
        console.info(
            `The total number of characters to represent the newly encoded strings minus the number of characters of code in each original string literal is ${this.secondSolution}.`
        );

        return this.createResult([this.firstSolution, this.secondSolution]);
    }
}
