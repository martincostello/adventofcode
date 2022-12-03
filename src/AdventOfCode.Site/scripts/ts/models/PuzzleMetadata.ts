// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

export interface PuzzleMetadata {
    name: string;
    year: number;
    day: number;
    minimumArguments: number;
    requiresData: boolean;
    location: string;
}
