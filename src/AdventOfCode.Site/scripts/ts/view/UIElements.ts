// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

export class UIElements {

    readonly arguments: HTMLInputElement;
    readonly argumentsContainer: HTMLElement;
    readonly inputFile: HTMLInputElement;
    readonly inputFileLabel: HTMLElement;
    readonly inputFileContainer: HTMLElement;
    readonly solution: HTMLElement;
    readonly solutionContainer: HTMLElement;
    readonly solutionHeader: HTMLElement;
    readonly spinner: HTMLElement;
    readonly visualization: HTMLElement;
    readonly days: HTMLSelectElement;
    readonly error: HTMLElement;
    readonly form: HTMLElement;
    readonly solve: HTMLButtonElement;
    readonly solveText: HTMLElement;
    readonly timeToSolve: HTMLElement;
    readonly years: HTMLInputElement;

    constructor() {

        this.arguments = <HTMLInputElement>document.getElementById('arguments');
        this.argumentsContainer = document.getElementById('arguments-container');

        this.inputFile = <HTMLInputElement>document.getElementById('resource');
        this.inputFileLabel = document.getElementById('resource-label');
        this.inputFileContainer = document.getElementById('file-container');

        this.solution = document.getElementById('solution');
        this.solutionContainer = document.getElementById('solution-container');
        this.solutionHeader = document.getElementById('solution-header');

        this.spinner = document.getElementById('spinner');
        this.visualization = document.getElementById('visualization');

        this.days = <HTMLSelectElement>document.getElementById('day');
        this.error = document.getElementById('error');
        this.form = document.getElementById('form');
        this.solve = <HTMLButtonElement>document.getElementById('solve');
        this.solveText = document.getElementById('solve-text');
        this.timeToSolve = document.getElementById('time-to-solve');
        this.years = <HTMLInputElement>document.getElementById('year');
    }
}
