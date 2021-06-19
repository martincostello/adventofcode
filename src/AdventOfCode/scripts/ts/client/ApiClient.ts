// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

export class ApiClient {

    async solve(url: string, formData: FormData): Promise<Response> {

        const init = {
            method: 'POST',
            body: formData
        };

        const response = await fetch(url, init);
        const content = await response.json();

        return content;
    }
}
