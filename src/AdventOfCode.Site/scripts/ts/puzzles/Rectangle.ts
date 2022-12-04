// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

export class Rectangle {
    constructor(public readonly x: number, public readonly y: number, public readonly width: number, public readonly height: number) {}

    static fromLTRB(left: number, top: number, right: number, bottom: number) {
        return new Rectangle(left, top, right - left, bottom - top);
    }

    equals(other: any) {
        if (other instanceof Rectangle) {
            return this.x === other.x && this.y === other.y && this.width === other.width && this.height === other.height;
        }
        return false;
    }
}
