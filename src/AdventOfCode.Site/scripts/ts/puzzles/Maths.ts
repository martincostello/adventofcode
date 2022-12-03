// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { from, IEnumerable } from 'linq-to-typescript';

export class Maths {
    static getPermutations<T>(collection: T[]): IEnumerable<IEnumerable<T>> {
        return Maths.getPermutationsWithCount(collection, collection.length);
    }

    static getPermutationsWithCount<T>(collection: T[], count: number): IEnumerable<IEnumerable<T>> {
        const enumerable = from(collection);
        if (count === 1) {
            return enumerable.select((p: T) => from([p]));
        }

        const permutations = Maths.getPermutationsWithCount(collection, count - 1);
        return Maths.selectMany(
            permutations,
            (p) => enumerable.except(p),
            (set, value) => Maths.append(set, value)
        );
    }

    private static append<T>(source: IEnumerable<T>, value: T): IEnumerable<T> {
        return source.concatenate(from([value]));
    }

    // Adapted from https://github.com/arogozine/LinqToTypeScript/blob/9f6fc3bac2f48f746257f9a41a4192c02b3aa733/src/sync/_private/selectMany.ts

    private static selectMany<TSource, TCollection, TResult>(
        source: IEnumerable<TSource>,
        collectionSelector: (x: TSource) => IEnumerable<TCollection>,
        resultSelector: (x: TSource, collection: TCollection) => IEnumerable<TResult>
    ) {
        function* iterator() {
            for (const element of source) {
                for (const subElement of collectionSelector(element)) {
                    yield resultSelector(element, subElement);
                }
            }
        }

        return from(new BasicEnumerable(iterator));
    }
}

class BasicEnumerable<TSource> implements Iterable<TSource> {
    public constructor(private readonly iterator: () => IterableIterator<TSource>) {}

    public [Symbol.iterator](): IterableIterator<TSource> {
        return this.iterator();
    }
}
