// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle2023 } from './Puzzle2023';

export class Day05 extends Puzzle2023 {
    solution1: number;
    solution2: number;

    override get solved() {
        return false;
    }

    override get name() {
        return 'If You Give A Seed A Fertilizer';
    }

    override get day() {
        return 5;
    }

    static solve(values: string[]): number {
        return -1;
    }

    override solveCore(_: string[]) {
        const values = this.readResourceAsLines();

        this.solution1 = Day05.solve(values);
        this.solution2 = Day05.solve(values);

        console.info(`${this.solution1}`);
        console.info(`${this.solution2}`);

        return this.createResult([this.solution1, this.solution2]);
    }
}
