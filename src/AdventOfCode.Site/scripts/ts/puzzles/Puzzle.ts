// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Solution } from '../models/Solution';

export abstract class Puzzle {
    resource: string | null;

    abstract get name(): string;
    abstract get day(): number;
    abstract get year(): number;

    protected get minimumArguments(): number {
        return 0;
    }

    async solve(inputs: string[]): Promise<Solution> {
        return await this.solveCore(inputs);
    }

    abstract solveCore(inputs: string[]): Promise<Solution>;
}
