// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Point } from './Point';
import { Size } from './Size';
import { WeightedGraph } from './WeightedGraph';

export abstract class SquareGrid implements WeightedGraph<Point> {
    private static readonly vectors = [new Size(0, 1), new Size(1, 0), new Size(0, -1), new Size(-1, 0)];

    readonly borders = new Set<string>();
    readonly locations = new Set<string>();

    protected constructor(public readonly width: number, public readonly height: number) {}

    inBounds(id: Point) {
        return id.x >= 0 && id.x < this.width && id.y >= 0 && id.y < this.height;
    }

    isPassable(id: Point) {
        return !this.borders.has(id.toString());
    }

    abstract cost(a: Point, b: Point): number;

    neighbors(id: Point): Iterable<Point> {
        function* generator(grid: SquareGrid) {
            for (const vector of SquareGrid.vectors) {
                const next = Point.add(id, vector);
                if (grid.inBounds(next) && grid.isPassable(next)) {
                    yield next;
                }
            }
        }

        return generator(this);
    }
}
