// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from } from 'linq-to-typescript';
import { Solution } from '../../models/Solution';
import { Puzzle2015 } from './Puzzle2015';

export class Day02 extends Puzzle2015 {
    totalAreaOfPaper: number;
    totalLengthOfRibbon: number;

    override get name(): string {
        return 'I Was Told There Would Be No Math';
    }

    override get day(): number {
        return 2;
    }

    protected override get requiresData(): boolean {
        return true;
    }

    static getTotalWrappingPaperAreaAndRibbonLength(dimensions: string[]): [area: number, length: number] {
        const presents = from(dimensions)
            .select((p: string) => Present.parse(p))
            .toArray();

        const totalArea = from(presents).sum((p: Present) => Day02.getWrappingPaperArea(p));
        const length = from(presents).sum((p: Present) => Day02.getRibbonLength(p));

        return [totalArea, length];
    }

    override solveCore(_: string[]): Promise<Solution> {
        const dimensions = this.readResourceAsLines();

        [this.totalAreaOfPaper, this.totalLengthOfRibbon] = Day02.getTotalWrappingPaperAreaAndRibbonLength(dimensions);

        console.info(
            `The elves should order ${this.totalAreaOfPaper} square feet of wrapping paper.\nThey also need ${this.totalLengthOfRibbon} feet of ribbon.`
        );

        return this.createResult([this.totalAreaOfPaper, this.totalLengthOfRibbon]);
    }

    private static getRibbonLength(present: Present): number {
        let smallestPerimeter = Math.min((present.length + present.width) * 2, (present.width + present.height) * 2);
        smallestPerimeter = Math.min(smallestPerimeter, (present.height + present.length) * 2);

        const lengthForBow = present.height * present.length * present.width;

        return smallestPerimeter + lengthForBow;
    }

    private static getWrappingPaperArea(present: Present): number {
        const surfaceArea = 2 * present.length * present.width + 2 * present.width * present.height + 2 * present.height * present.length;

        let extra = Math.min(present.length * present.width, present.width * present.height);
        extra = Math.min(extra, present.height * present.length);

        return surfaceArea + extra;
    }
}

class Present {
    constructor(public readonly length: number, public readonly width: number, public readonly height: number) {}

    static parse(value: string): Present {
        const [length, width, height] = value.split('x');
        return new Present(parseInt(length, 10), parseInt(width, 10), parseInt(height, 10));
    }
}
