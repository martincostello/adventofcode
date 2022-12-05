// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { ProblemDetails, Solution } from '../models/index';
import { Puzzle } from '../puzzles/index';
import { Solver } from './Solver';

export class ClientSolver implements Solver {
    constructor(private readonly puzzle: Puzzle) {}

    async solve(inputs: string[], resource: string | null): Promise<ProblemDetails | Solution> {
        if (resource) {
            this.puzzle.resource = resource;
        }

        try {
            return await this.puzzle.solve(inputs);
        } catch (err: unknown) {
            return {
                type: '',
                title: 'Bad Request',
                status: 400,
                detail: err.toString(),
                instance: this.puzzle.name,
            };
        }
    }
}
