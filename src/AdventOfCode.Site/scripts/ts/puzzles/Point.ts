// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Size } from './Size';

export class Point {
    static readonly empty = new Point(0, 0);

    constructor(public readonly x: number, public readonly y: number) {}

    static add(point: Point, size: Size): Point {
        return new Point(point.x + size.width, point.y + size.height);
    }

    static manhattanDistance(x: Point, y: Point): number {
        return Math.abs(x.x - y.x) + Math.abs(x.y - y.y);
    }

    equals(other: any) {
        if (other instanceof Point) {
            return this.x === other.x && this.y === other.y;
        }
        return false;
    }
}
