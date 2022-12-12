// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Graph } from './Graph';

export class NodeGraph<T> implements Graph<T> {
    edges: Map<T, T[]>;

    neighbors(id: T): Iterable<T> {
        return this.edges.get(id);
    }
}
