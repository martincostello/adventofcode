// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

export class Size {
    static zero = new Size(0, 0);

    width: number;
    height: number;

    constructor(width: number, height: number) {
        this.width = width;
        this.height = height;
    }

    equals(other: any) {
        if (other instanceof Size) {
            return this.width === other.width && this.height === other.height;
        }
        return false;
    }
}
