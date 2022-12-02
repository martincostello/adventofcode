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

    protected get requiresData(): boolean {
        return false;
    }

    async solve(inputs: string[]): Promise<Solution> {
        if (!this.ensureArguments(inputs)) {
            const singular = this.minimumArguments === 1;
            const message = `At least ${this.minimumArguments} argument${singular ? '' : 's'} ${singular ? 'is' : 'are'} required.`;
            throw new Error(message);
        }

        if (this.requiresData && !this.resource) {
            throw new Error('A puzzle input is required.');
        }

        return await this.solveCore(inputs);
    }

    abstract solveCore(inputs: string[]): Promise<Solution>;

    private ensureArguments(inputs: string[]): boolean {
        return inputs && inputs.length >= this.minimumArguments;
    }
}
