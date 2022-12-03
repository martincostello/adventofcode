// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from, IEnumerable } from 'linq-to-typescript';

export class Maths {
    static getPermutations<T>(collection: T[]): IEnumerable<IEnumerable<T>> {
        return Maths.getPermutationsWithCount(collection, collection.length);
    }

    static getPermutationsWithCount<T>(collection: T[], count: number): IEnumerable<IEnumerable<T>> {
        let enumerable = from(collection);
        if (count === 1) {
            return enumerable.select((p: T) => from([p]));
        }

        const permutations = Maths.getPermutationsWithCount(collection, count - 1);

        const result: IEnumerable<T>[] = [];

        for (const permutation of permutations) {
            for (const item of collection) {
                if (!permutation.contains(item)) {
                    result.push(permutation.concatenate(from([item])));
                }
            }
        }

        return from(result);
    }
}
