// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { ApiClient } from '../client/index';
import { ProblemDetails, Solution } from '../models/index';
import { Solver } from './Solver';

export class ServerSolver implements Solver {
    constructor(
        private readonly client: ApiClient,
        private readonly form: HTMLElement,
        private readonly inputFile: HTMLInputElement
    ) {}

    async solve(inputs: string[], resource: string | null): Promise<ProblemDetails | Solution> {
        const url = this.form.getAttribute('action');
        const form = new FormData();

        if (inputs) {
            inputs.forEach((value) => {
                form.append('arguments', value);
            });
        }

        if (resource && resource.length > 0) {
            const file = this.inputFile.files[0];
            form.append('resource', file, 'input.txt');
            form.append('resource', resource);
        }

        return await this.client.solve(url, form);
    }
}
