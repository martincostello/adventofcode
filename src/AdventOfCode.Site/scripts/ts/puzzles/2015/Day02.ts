// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Puzzle2015 } from './Puzzle2015';

export class Day02 extends Puzzle2015 {
    totalAreaOfPaper: number;
    totalLengthOfRibbon: number;

    override get name() {
        return 'I Was Told There Would Be No Math';
    }

    override get day() {
        return 2;
    }

    static getTotalWrappingPaperAreaAndRibbonLength(dimensions: string[]): [area: number, length: number] {
        const presents = dimensions.map((p) => Present.parse(p));
        const totalArea = presents.reduce((x, y) => x + y.getWrappingPaperArea(), 0);
        const length = presents.reduce((x, y) => x + y.getRibbonLength(), 0);

        return [totalArea, length];
    }

    override solveCore(_: string[]) {
        const dimensions = this.readResourceAsLines();

        [this.totalAreaOfPaper, this.totalLengthOfRibbon] = Day02.getTotalWrappingPaperAreaAndRibbonLength(dimensions);

        console.info(
            `The elves should order ${this.totalAreaOfPaper} square feet of wrapping paper.\n` +
                `They also need ${this.totalLengthOfRibbon} feet of ribbon.`
        );

        return this.createResult([this.totalAreaOfPaper, this.totalLengthOfRibbon]);
    }
}

class Present {
    constructor(
        readonly length: number,
        readonly width: number,
        readonly height: number
    ) {}

    static parse(value: string) {
        const [length, width, height] = value.split('x');
        return new Present(parseInt(length, 10), parseInt(width, 10), parseInt(height, 10));
    }

    get volume() {
        return this.height * this.length * this.width;
    }

    getRibbonLength() {
        return Math.min((this.length + this.width) * 2, (this.width + this.height) * 2, (this.height + this.length) * 2) + this.volume;
    }

    getWrappingPaperArea() {
        const surfaceArea = 2 * this.length * this.width + 2 * this.width * this.height + 2 * this.height * this.length;
        const extra = Math.min(this.length * this.width, this.width * this.height, this.height * this.length);

        return surfaceArea + extra;
    }
}
