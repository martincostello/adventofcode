// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Puzzle2018 } from './Puzzle2018';

export class Day01 extends Puzzle2018 {
    frequency: number;
    firstRepeatedFrequency: number;

    override get name() {
        return 'Chronal Calibration';
    }

    override get day() {
        return 1;
    }

    static calculateFrequency(sequence: number[]) {
        const [frequency] = Day01.calculateFrequencyWithRepetition(sequence);
        return frequency;
    }

    static calculateFrequencyWithRepetition(sequence: number[]): [frequency: number, firstRepeat: number] {
        let current = 0;
        let frequency: number | null = null;
        let firstRepeat: number | null = null;

        const history: number[] = [];
        history.push(current);

        const tendsToInfinity = (sequence: number[]): boolean => {
            return Math.abs(from(sequence).sum((p: number) => Math.sign(p))) === from(sequence).count((p: number) => p !== 0);
        };

        const isInfinite = tendsToInfinity(sequence);

        do {
            for (const shift of sequence) {
                let previous = current;
                current += shift;

                if (firstRepeat === null && history.includes(current) && history[history.length - 2] !== previous) {
                    firstRepeat = current;
                }

                history.push(current);
            }

            if (frequency === null) {
                frequency = current;
            }
        } while (firstRepeat === null && !isInfinite);

        return [frequency, firstRepeat !== null ? firstRepeat : frequency];
    }

    override solveCore(_: string[]) {
        const sequence = this.readResourceAsNumbers();

        [this.frequency, this.firstRepeatedFrequency] = Day01.calculateFrequencyWithRepetition(sequence);

        console.info(`The resulting frequency is ${this.frequency}.`);
        console.info(`The first repeated frequency is ${this.firstRepeatedFrequency}.`);

        return this.createResult([this.frequency, this.firstRepeatedFrequency]);
    }
}
