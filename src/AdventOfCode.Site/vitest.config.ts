import { defineConfig } from 'vitest/config';

export default defineConfig({
    test: {
        clearMocks: true,
        coverage: {
            enabled: true,
            provider: 'v8',
            include: ['scripts/**/*.ts'],
            reporter: ['html', 'json', 'lcov', 'text'],
        },
        reporters: ['default', 'github-actions'],
        testTimeout: 60000,
    },
});
