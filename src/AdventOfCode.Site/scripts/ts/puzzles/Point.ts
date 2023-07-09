// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Size } from './Size';

export class Point extends Object {
    static readonly empty = new Point(0, 0);

    constructor(
        readonly x: number,
        readonly y: number
    ) {
        super();
    }

    static add(point: Point, size: Size) {
        return new Point(point.x + size.width, point.y + size.height);
    }

    static manhattanDistance(x: Point, y: Point) {
        return Math.abs(x.x - y.x) + Math.abs(x.y - y.y);
    }

    override toString() {
        return `${this.x},${this.y}`;
    }

    equals(other: any) {
        if (other instanceof Point) {
            return this.x === other.x && this.y === other.y;
        }
        return false;
    }
}
