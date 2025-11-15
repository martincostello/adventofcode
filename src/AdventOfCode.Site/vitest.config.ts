import { defineConfig } from 'vitest/config';

export default defineConfig({
    test: {
        clearMocks: true,
        coverage: {
            enabled: true,
            provider: 'v8',
            include: ['scripts/**/*.ts'],
            reporter: ['text', 'json', 'html', 'lcov'],
        },
        reporters: ['default', 'github-actions'],
        testTimeout: 60000,
    },
});
