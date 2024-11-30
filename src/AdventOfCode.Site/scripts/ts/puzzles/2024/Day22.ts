// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle2024 } from './Puzzle2024';

export class Day22 extends Puzzle2024 {
    solution1: number;
    solution2: number;

    override get solved() {
        return false;
    }

    override get name() {
        return '';
    }

    override get day() {
        return 22;
    }

    static solve(values: string[]): number {
        return -1;
    }

    override solveCore(_: string[]) {
        const values = this.readResourceAsLines();

        this.solution1 = Day22.solve(values);
        this.solution2 = Day22.solve(values);

        console.info(`${this.solution1}`);
        console.info(`${this.solution2}`);

        return this.createResult([this.solution1, this.solution2]);
    }
}
