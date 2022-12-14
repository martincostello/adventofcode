// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Point } from './Point';

export class Rectangle {
    constructor(readonly x: number, readonly y: number, readonly width: number, readonly height: number) {}

    get left() {
        return this.x;
    }

    get right() {
        return this.x + this.width;
    }

    get top() {
        return this.y;
    }

    get bottom() {
        return this.y + this.height;
    }

    static fromLTRB(left: number, top: number, right: number, bottom: number) {
        return new Rectangle(left, top, right - left, bottom - top);
    }

    contains(point: Point) {
        return this.x <= point.x && point.x < this.x + this.width && this.y <= point.y && point.y < this.y + this.height;
    }

    equals(other: any) {
        if (other instanceof Rectangle) {
            return this.x === other.x && this.y === other.y && this.width === other.width && this.height === other.height;
        }
        return false;
    }
}
