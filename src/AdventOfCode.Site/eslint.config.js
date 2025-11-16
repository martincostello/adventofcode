import { FlatCompat } from "@eslint/eslintrc";
import js from "@eslint/js";
import stylistic from "@stylistic/eslint-plugin";
import typescriptEslint from "@typescript-eslint/eslint-plugin";
import tsParser from "@typescript-eslint/parser";
import vitest from '@vitest/eslint-plugin';
import globals from "globals";
import path from "node:path";
import { fileURLToPath } from "node:url";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const compat = new FlatCompat({
    baseDirectory: __dirname,
    recommendedConfig: js.configs.recommended,
    allConfig: js.configs.all
});

export default [...compat.extends("prettier"), {
    files: ['**/*.cjs', '**/*.js', '**/*.ts'],
    ignores: [
        'bin/*',
        'coverage/*',
        'node_modules/*',
        'obj/*',
        'wwwroot/*'
    ],
    plugins: {
        "@stylistic": stylistic,
        "@typescript-eslint": typescriptEslint,
        vitest,
    },
    languageOptions: {
        globals: {
            ...globals.browser,
            ...vitest.environments.env.globals,
            ...globals.node,
        },
        parser: tsParser,
        ecmaVersion: 5,
        sourceType: "module",
        parserOptions: {
            project: "./tsconfig.json",
        },
    },
    rules: {
        "@stylistic/indent": ["error", 4, {
            SwitchCase: 1,
        }],
        "@stylistic/member-delimiter-style": "error",
        "@stylistic/quotes": ["error", "single", {
            avoidEscape: true,
        }],
        "@stylistic/semi": ["error", "always"],
        "@stylistic/type-annotation-spacing": "error",
        "@typescript-eslint/naming-convention": "error",
        "@typescript-eslint/prefer-namespace-keyword": "error",
        "brace-style": ["error", "1tbs"],
        eqeqeq: ["error", "smart"],
        "id-blacklist": [
            "error",
            "any",
            "Number",
            "number",
            "String",
            "string",
            "Boolean",
            "boolean",
            "Undefined",
            "undefined",
        ],
        "id-match": "error",
        "max-len": ["error", {
            code: 140,
            ignoreUrls: true,
            tabWidth: 4,
        }],
        "no-eval": "error",
        "no-redeclare": "error",
        "no-tabs": ["error", {
            allowIndentationTabs: true,
        }],
        "no-trailing-spaces": "error",
        "no-underscore-dangle": "error",
        "no-var": "error",
        "spaced-comment": ["error", "always", {
            markers: ["/"],
        }],
    },
}];
