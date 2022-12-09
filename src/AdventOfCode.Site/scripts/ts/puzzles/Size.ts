// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

export class Size {
    static readonly zero = new Size(0, 0);

    constructor(public readonly width: number, public readonly height: number) {}

    divide(value: number) {
        return new Size(this.width / value, this.height / value);
    }

    equals(other: any) {
        if (other instanceof Size) {
            return this.width === other.width && this.height === other.height;
        }
        return false;
    }
}
