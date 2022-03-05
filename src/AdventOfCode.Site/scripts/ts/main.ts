// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Solver } from './view/Solver';

document.addEventListener('DOMContentLoaded', () => {
    const baseUrl = document.querySelector('link[rel="api-base"]');
    if (baseUrl) {
        const solver = new Solver(baseUrl.getAttribute('href'));
        solver.initialize();
    }
});
