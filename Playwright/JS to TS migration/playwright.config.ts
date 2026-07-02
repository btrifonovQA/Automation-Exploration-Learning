import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
    // Look for test files in the "tests" directory, relative to this configuration file.
    testDir: 'tests',

    // Run all tests in parallel.
    fullyParallel: true,

    // Reporter to use
    reporter: 'html',

    // Configure projects for major browsers.
    projects: [
        {
            name: 'chromium',
            use: { ...devices['Desktop Chrome'] },
        },
    ],

    // Run your local dev server before starting the tests.
    webServer: [
        {
            command: 'npm run server',
            url: 'http://localhost:3030',
            env: { NODE_NO_WARNINGS: '1' },
            reuseExistingServer: !process.env.CI,
        },
        {
            command: 'npm run start',
            url: 'http://localhost:3000',
            env: { NODE_NO_WARNINGS: '1' },
            reuseExistingServer: !process.env.CI,
        }
    ]
});