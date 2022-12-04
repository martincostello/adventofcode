// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Md5 as MD5 } from 'ts-md5';
import { Solution } from '../../models/Solution';
import { Puzzle } from '../Puzzle';
import { Puzzle2015 } from './Puzzle2015';

export class Day04 extends Puzzle2015 {
    lowestZeroHash: number;

    override get name(): string {
        return 'The Ideal Stocking Stuffer';
    }

    override get day(): number {
        return 4;
    }

    protected override get minimumArguments(): number {
        return 2;
    }

    static getLowestPositiveNumberWithStartingZeroesAsync(secretKey: string, zeroes: number): number {
        const isSolution = (value: number, secretKey: string, zeroes: number): boolean => {
            const hash = MD5.hashStr(`${secretKey}${value}`, true);
            const wholeBytes = zeroes / 2;
            const remainder = zeroes % 2;
            const hasHalfByte = remainder === 1;

            let sum = hash[0];

            // Are the whole bytes all zero?
            for (let j = 1; sum === 0 && j < wholeBytes; j++) {
                sum += hash[j];
            }

            if (sum === 0) {
                // The current value is a solution if there is an even number
                // of zeroes or if the low bits of the odd byte are zero.
                if (!hasHalfByte || hash[wholeBytes] < 0x10) {
                    return true;
                }
            }

            return false;
        };

        for (let i = 1; i < 2147483647; i++) {
            if (isSolution(i, secretKey, zeroes)) {
                return i;
            }
        }

        return 0;
    }

    override solveCore(inputs: string[]): Promise<Solution> {
        const secretKey = inputs[0];
        const zeroes = Puzzle.parse(inputs[1]);

        this.lowestZeroHash = Day04.getLowestPositiveNumberWithStartingZeroesAsync(secretKey, zeroes);

        console.info(`The lowest positive number for a hash starting with ${zeroes} zeroes is ${this.lowestZeroHash}.`);

        return this.createResult([this.lowestZeroHash]);
    }
}
