// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { repeat } from 'linq-to-typescript';
import { Point } from '../Point';
import { Puzzle } from '../Puzzle';
import { Size } from '../Size';
import { Puzzle2022 } from './Puzzle2022';

export class Day09 extends Puzzle2022 {
    positionsVisited2: number;
    positionsVisited10: number;

    override get name() {
        return 'Rope Bridge';
    }

    override get day() {
        return 9;
    }

    protected override get requiresData() {
        return true;
    }

    static move(moves: string[], knots: number): number {
        const parse = (moves: string[]) => {
            const result: Size[] = [];

            for (const move of moves) {
                const direction = move[0];
                const distance = Puzzle.parse(move.slice(1));

                let d: Size;

                switch (direction) {
                    case 'U':
                        d = new Size(0, distance);
                        break;
                    case 'D':
                        d = new Size(0, -distance);
                        break;
                    case 'L':
                        d = new Size(-distance, 0);
                        break;
                    case 'R':
                        d = new Size(distance, 0);
                        break;
                    default:
                        throw new Error(`Invalid direction '${direction}'.`);
                }

                result.push(d);
            }

            return result;
        };

        const directions = parse(moves);
        const rope = new Rope(repeat(Point.empty, knots).toArray());

        const positions = new Set<string>();
        positions.add(rope.tail.asString());

        for (const direction of directions) {
            rope.move(direction, (tail) => positions.add(tail.asString()));
        }

        return positions.size;
    }

    override solveCore(_: string[]) {
        const grid = this.readResourceAsLines();

        this.positionsVisited2 = Day09.move(grid, 2);
        this.positionsVisited10 = Day09.move(grid, 10);

        console.info(`The tail of the rope with two knots visits ${this.positionsVisited2} positions at least once.`);
        console.info(`The tail of the rope with ten knots visits ${this.positionsVisited10} positions at least once.`);

        return this.createResult([this.positionsVisited2, this.positionsVisited10]);
    }
}

class Rope {
    constructor(private readonly knots: Point[]) {}

    get count() {
        return this.knots.length;
    }

    get head() {
        return this.knots[0];
    }

    private set head(value: Point) {
        this.knots[0] = value;
    }

    get tail() {
        return this.knots[this.knots.length - 1];
    }

    move(direction: Size, onMoveTail: (tail: Point) => void) {
        const magnitude = Math.max(Math.abs(direction.width), Math.abs(direction.height));
        const unit = direction.divide(magnitude);

        for (let i = 0; i < magnitude; i++) {
            const previousTail = this.tail;
            this.head = Point.add(this.head, unit);

            for (let j = 1; j < this.count; j++) {
                const leader = this.knots[j - 1];
                let follower = this.knots[j];

                const deltaX = leader.x - follower.x;
                const deltaY = leader.y - follower.y;

                if (Math.abs(deltaX) > 1 || Math.abs(deltaY) > 1) {
                    follower = Point.add(follower, new Size(Math.sign(deltaX), Math.sign(deltaY)));
                }

                this.knots[j] = follower;
            }

            if (!previousTail.equals(this.tail)) {
                onMoveTail(this.tail);
            }
        }
    }
}
