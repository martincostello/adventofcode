// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { ApiClient } from '../client/ApiClient';
import { ProblemDetails } from '../models/ProblemDetails';
import { Puzzle } from '../models/Puzzle';
import { Solution } from '../models/Solution';
import { UIElements } from './UIElements';

export class Solver {

    private static hideClass: string = 'd-none';

    private readonly client: ApiClient;
    private readonly elements: UIElements;
    private readonly puzzlesByYear: Map<string, Puzzle[]>;

    constructor() {
        this.elements = new UIElements();
        this.client = new ApiClient();
        this.puzzlesByYear = new Map<string, Puzzle[]>();
    }

    async initialize(): Promise<void> {

        this.elements.years.addEventListener('change', () => {
            this.onYearChanged();
        });

        this.elements.days.addEventListener('change', () => {
            this.onDayChanged();
        });

        this.elements.solve.addEventListener('click', () => {
            this.onSolve();
        });

        this.elements.form.addEventListener('submit', (event) => {
            event.preventDefault();
            return false;
        });

        const puzzles = await this.client.getPuzzles();

        puzzles.forEach((puzzle) => {

            const year = puzzle.year.toString();

            let puzzlesForYear: Puzzle[];

            if (!this.puzzlesByYear.has(year)) {
                puzzlesForYear = [];
                this.puzzlesByYear.set(year, puzzlesForYear);
            } else {
                puzzlesForYear = this.puzzlesByYear.get(year);
            }

            puzzlesForYear.push(puzzle);

            if (puzzlesForYear.length === 1) {

                const element = document.createElement('option');

                element.setAttribute('data-year', year);
                element.setAttribute('value', year);

                element.text = year;

                this.elements.years.appendChild(element);

                if (puzzle.year === new Date().getFullYear()) {
                    element.selected = true;
                }
            }
        });

        this.elements.years.dispatchEvent(new Event('change'));
    }

    private onDayChanged() {

        const option = this.elements.days.selectedOptions[0];

        this.elements.arguments.value = '';
        this.elements.inputFile.value = '';

        this.hide(this.elements.error);
        this.elements.error.textContent = '';

        this.hide(this.elements.solutionContainer);
        this.elements.solution.innerHTML = '';
        this.elements.timeToSolve.textContent = '';
        this.elements.visualization.innerHTML = '';

        this.elements.form.setAttribute('action', option.getAttribute('data-location'));

        const minimumArguments = parseInt(option.getAttribute('data-minimum-arguments'), 10);

        if (minimumArguments === 0) {
            this.hide(this.elements.argumentsContainer);
            this.elements.arguments.setAttribute('placeholder', '');
            this.elements.arguments.setAttribute('rows', '0');
            this.elements.arguments.removeAttribute('required');
        } else {
            this.show(this.elements.argumentsContainer);
            this.elements.arguments.setAttribute('placeholder', `Requires ${minimumArguments} argument${minimumArguments === 1 ? '' : 's'}`);
            this.elements.arguments.setAttribute('rows', minimumArguments.toString(10));
            this.elements.arguments.setAttribute('required', '');
        }

        if (option.getAttribute('data-requires-data') === 'true') {
            this.elements.inputFile.setAttribute('required', '');
            this.show(this.elements.inputFileContainer);
        } else {
            this.elements.inputFile.removeAttribute('required');
            this.hide(this.elements.inputFileContainer);
        }
    }

    private onYearChanged() {

        this.elements.days.innerHTML = '';

        const puzzlesForYear = this.puzzlesByYear.get(this.elements.years.value);

        for (let i = 0; i < puzzlesForYear.length; i++) {

            const element = document.createElement('option');
            const puzzle = puzzlesForYear[i];

            const day = puzzle.day.toString(10);

            element.setAttribute('data-day', day);
            element.setAttribute('data-location', puzzle.location);
            element.setAttribute('data-minimum-arguments', puzzle.minimumArguments.toString(10));
            element.setAttribute('data-requires-data', puzzle.requiresData.toString());
            element.setAttribute('data-year', puzzle.year.toString(10));

            element.setAttribute('value', day);
            element.text = day;

            this.elements.days.appendChild(element);

            const now = new Date();

            if (puzzle.year === now.getFullYear() && puzzle.day === now.getDate() && now.getMonth() === 11) {
                element.selected = true;
            }
        }

        this.elements.days.dispatchEvent(new Event('change'));
    }

    private hide(element: Element) {
        element.classList.add(Solver.hideClass);
    }

    private show(element: Element) {
        element.classList.remove(Solver.hideClass);
    }

    private async onSolve(): Promise<void> {

        this.elements.solve.disabled = true;
        this.elements.solveText.innerText = 'Solving...';
        this.show(this.elements.spinner);

        const url = this.elements.form.getAttribute('action');
        const form = new FormData();

        if (this.elements.arguments.value) {
            const split = this.elements.arguments.value.split('\n');

            split.forEach((value) => {
                form.append('arguments', value);
            });
        }

        if (this.elements.inputFile.files.length === 1) {

            const file = this.elements.inputFile.files[0];
            const reader = new FileReader();

            reader.readAsText(file, 'UTF-8');

            reader.onload = (event) => {
                form.append('resource', event.target.result as string);
                this.solve(url, form);
            };

            form.append('resource', file);
        } else {
            await this.solve(url, form);
        }
    }

    private async solve(url: string, form: FormData): Promise<void> {

        this.hide(this.elements.error);
        this.hide(this.elements.solutionContainer);

        this.elements.error.textContent = '';
        this.elements.solution.innerHTML = '';
        this.elements.timeToSolve.textContent = '';
        this.elements.visualization.innerHTML = '';

        const result = await this.client.solve(url, form);

        const error = result as ProblemDetails;
        const solution = result as Solution;

        if (error.status) {
            this.hide(this.elements.solutionContainer);
            this.show(this.elements.error);
            this.elements.error.textContent = error.detail;
        } else {

            this.hide(this.elements.error);
            this.elements.timeToSolve.textContent = `${solution.timeToSolve.toFixed(2)} milliseconds`;

            this.elements.solution.innerHTML = '';
            this.elements.solutionHeader.textContent = `Advent of Code ${solution.year} Day ${solution.day} Solution`;

            solution.solutions.forEach((value) => {
                const element = document.createElement('li');
                element.classList.add('text-monospace');
                element.textContent = value;
                this.elements.solution.appendChild(element);
            });

            solution.visualizations.forEach((value) => {

                const pre = document.createElement('pre');
                pre.textContent = value;

                if (value.split('\n').length > 100) {
                    pre.classList.add('pre-scrollable');
                } else {
                    pre.classList.remove('pre-scrollable');
                }

                const code = document.createElement('code');
                code.appendChild(pre);

                const li = document.createElement('li');
                li.appendChild(code);

                this.elements.visualization.appendChild(li);
            });

            this.show(this.elements.solutionContainer);
        }

        this.hide(this.elements.spinner);
        this.elements.solveText.innerText = 'Solve!';
        this.elements.solve.disabled = false;
    }
}
