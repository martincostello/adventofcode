// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Point } from '../Point';
import { Size } from '../Size';
import { Puzzle2022 } from './Puzzle2022';

export class Day08 extends Puzzle2022 {
    visibleTrees: number;
    maximumScenicScore: number;

    override get name() {
        return 'Treetop Tree House';
    }

    override get day() {
        return 8;
    }

    protected override get requiresData() {
        return true;
    }

    static countVisibleTrees(grid: string[]): [visibleTrees: number, maximumScenicScore: number] {
        const up = new Size(0, -1);
        const down = new Size(0, 1);
        const left = new Size(-1, 0);
        const right = new Size(1, 0);

        const parse = (grid: string[]): [trees: Map<string, Tree>, height: number, width: number] => {
            const height = grid.length;
            const width = grid[0].length;

            const trees = new Map<string, Tree>();

            for (let y = 0; y < height; y++) {
                for (let x = 0; x < width; x++) {
                    const location = new Point(x, y);
                    trees.set(location.asString(), new Tree(location, grid[y][x].charCodeAt(0) - '0'.charCodeAt(0)));
                }
            }

            return [trees, height, width];
        };

        const sweep = (origin: Point, direction: Size, trees: Map<string, Tree>) => {
            let location = origin;
            let current = trees.get(location.asString());
            current.markVisible();

            while (trees.has((location = Point.add(location, direction)).asString())) {
                const next = trees.get(location.asString());
                if (next.height > current.height) {
                    next.markVisible();
                    current = next;
                }
            }
        };

        const countVisibleTrees = (trees: Map<string, Tree>, height: number, width: number) => {
            const topLeft = new Point(0, 0);
            const topRight = new Point(width - 1, 0);
            const bottomLeft = new Point(width - 1, height - 1);

            const directions: [origin: Point, next: Size, forward: Size][] = [
                [topLeft, down, right], // Left to right
                [topRight, down, left], // Right to left
                [topLeft, right, down], // Top to bottom
                [bottomLeft, left, up], // Bottom to top
            ];

            for (const [first, next, direction] of directions) {
                let origin = first;
                do {
                    sweep(origin, direction, trees);
                    origin = Point.add(origin, next);
                } while (trees.has(origin.asString()));
            }

            let count = 0;

            for (const tree of trees.values()) {
                if (tree.isVisible) {
                    count++;
                }
            }

            return count;
        };

        const score = (origin: Tree, direction: Size, trees: Map<string, Tree>) => {
            let location = origin.location;
            let score = 0;

            while (trees.has((location = Point.add(location, direction)).asString())) {
                const next = trees.get(location.asString());
                score++;

                if (next.height >= origin.height) {
                    break;
                }
            }

            return score;
        };

        const getMaximumScenicScore = (trees: Map<string, Tree>, height: number, width: number) => {
            const directions = [up, down, left, right];

            let maximum = 0;

            for (let y = 0; y < height; y++) {
                for (let x = 0; x < width; x++) {
                    const tree = trees.get(new Point(x, y).asString());

                    if (tree.isVisible) {
                        let scenicScore = 1;

                        for (const direction of directions) {
                            scenicScore *= score(tree, direction, trees);
                        }

                        maximum = Math.max(scenicScore, maximum);
                    }
                }
            }

            return maximum;
        };

        const [trees, height, width] = parse(grid);
        const count = countVisibleTrees(trees, height, width);
        const maximum = getMaximumScenicScore(trees, height, width);

        return [count, maximum];
    }

    override solveCore(_: string[]) {
        const grid = this.readResourceAsLines();

        [this.visibleTrees, this.maximumScenicScore] = Day08.countVisibleTrees(grid);

        console.info(`There are ${this.visibleTrees} trees visible from outside the grid.`);
        console.info(`The highest scenic score is ${this.maximumScenicScore}.`);

        return this.createResult([this.visibleTrees, this.maximumScenicScore]);
    }
}

class Tree {
    private isTreeVisible: boolean;

    constructor(public readonly location: Point, public readonly height: number) {
        this.isTreeVisible = false;
    }

    public get isVisible() {
        return this.isTreeVisible;
    }

    public markVisible() {
        this.isTreeVisible = true;
    }
}
