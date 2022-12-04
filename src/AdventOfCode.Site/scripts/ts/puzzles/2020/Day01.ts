// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { IEnumerable } from 'linq-to-typescript';
import { Maths } from '../Maths';
import { Puzzle2020 } from './Puzzle2020';

export class Day01 extends Puzzle2020 {
    productOf2020SumFrom2: number;
    productOf2020SumFrom3: number;

    override get name() {
        return 'Report Repair';
    }

    override get day() {
        return 1;
    }

    protected override get requiresData() {
        return true;
    }

    static get2020Product(expenses: number[], take: number) {
        const result = Maths.getPermutationsWithCount(expenses, take);

        return result
            .where((p: IEnumerable<number>) => p.sum() === 2020)
            .select((p: IEnumerable<number>) => p.aggregate((x: number, y: number) => x * y))
            .first();
    }

    override solveCore(_: string[]) {
        const values = this.readResourceAsNumbers();

        this.productOf2020SumFrom2 = Day01.get2020Product(values, 2);
        this.productOf2020SumFrom3 = Day01.get2020Product(values, 3);

        console.info(`The product of the two entries that sum to 2020 is ${this.productOf2020SumFrom2}.`);
        console.info(`The product of the three entries that sum to 2020 is ${this.productOf2020SumFrom3}.`);

        return this.createResult([this.productOf2020SumFrom2, this.productOf2020SumFrom3]);
    }
}
