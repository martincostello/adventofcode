// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle2025 } from './Puzzle2025';

export class Day01 extends Puzzle2025 {
    password1: number;
    password2: number;

    override get solved() {
        return true;
    }

    override get name() {
        return 'Secret Entrance';
    }

    override get day() {
        return 1;
    }

    static solve(values: string[], useMethod0x434C49434B: boolean): number {
        const steps = 100;

        let previous: number;
        let current = 50;
        let zeroes = 0;

        for (const rotation of values) {
            const sign = rotation[0] === 'R' ? 1 : -1;
            const clicks = parseInt(rotation.substring(1), 10);

            const cycles = Math.floor(clicks / steps);
            const distance = clicks % steps;

            previous = current;
            current = Math.abs(current + sign * distance + steps) % steps;

            if (useMethod0x434C49434B) {
                zeroes += cycles;

                if (previous !== current && (current === 0 || (previous !== 0 && sign * (current - previous) < 0))) {
                    zeroes++;
                }
            } else if (current === 0) {
                zeroes++;
            }
        }

        return zeroes;
    }

    override solveCore(_: string[]) {
        const values = this.readResourceAsLines();

        this.password1 = Day01.solve(values, false);
        this.password2 = Day01.solve(values, true);

        console.info(`The password to open the door is ${this.password1}`);
        console.info(`The password to open the door using method 0x434C49434B is ${this.password2}`);

        return this.createResult([this.password1, this.password2]);
    }
}
