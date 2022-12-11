// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle2017 } from './Puzzle2017';

export class Day01 extends Puzzle2017 {
    captchaSolutionNext: number;
    captchaSolutionOpposite: number;

    override get name() {
        return 'Inverse Captcha';
    }

    override get day() {
        return 1;
    }

    static calculateSum(digits: string, useOppositeDigit: boolean) {
        let sum = 0;
        let offset = useOppositeDigit ? digits.length / 2 : 1;

        for (let i = 0; i < digits.length; i++) {
            let j = i + offset;

            if (j >= digits.length) {
                j -= digits.length;
            }

            const first = digits[i];
            const second = digits[j];

            if (first === second) {
                sum += first.charCodeAt(0) - '0'.charCodeAt(0);
            }
        }

        return sum;
    }

    override solveCore(_: string[]) {
        const digits = this.readResourceAsString().trimEnd();

        this.captchaSolutionNext = Day01.calculateSum(digits, false);
        this.captchaSolutionOpposite = Day01.calculateSum(digits, true);

        console.info(`The solution to the first captcha is ${this.captchaSolutionNext}.`);
        console.info(`The solution to the second captcha is ${this.captchaSolutionOpposite}.`);

        return this.createResult([this.captchaSolutionNext, this.captchaSolutionOpposite]);
    }
}
