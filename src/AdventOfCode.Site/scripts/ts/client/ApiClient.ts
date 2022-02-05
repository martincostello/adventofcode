// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { ProblemDetails } from '../models/ProblemDetails';
import { Puzzle } from '../models/Puzzle';
import { Solution } from '../models/Solution';

export class ApiClient {

    async getPuzzles(): Promise<Puzzle[]> {

        const response = await fetch('/api/puzzles');

        if (!response.ok) {
            throw new Error(response.status.toString(10));
        }

        return await response.json();
    }

    async solve(url: string, form: FormData): Promise<Solution | ProblemDetails> {

        const init = {
            method: 'POST',
            body: form
        };

        const response = await fetch(url, init);
        const content = await response.json();

        if (response.ok) {
            return content as Solution;
        } else {
            return content as ProblemDetails;
        }
    }
}
