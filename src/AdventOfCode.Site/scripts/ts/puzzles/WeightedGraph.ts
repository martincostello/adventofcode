// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { Graph } from './Graph';

export interface WeightedGraph<T> extends Graph<T> {
    cost(a: T, b: T): number;
}
