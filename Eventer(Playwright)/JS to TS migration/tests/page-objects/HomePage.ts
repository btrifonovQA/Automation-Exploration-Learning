import { Page, Locator, expect } from '@playwright/test';

export class HomePage {
    readonly page: Page;
    readonly loginBtn: Locator;
    readonly registerBtn: Locator;
    readonly eventsBtn: Locator;

    constructor(page: Page) {
        this.page = page;
        this.loginBtn = page.getByRole('link', { name: 'Login' });
        this.registerBtn = page.getByRole('link', { name: 'Register' })
        this.eventsBtn = page.getByRole('link', { name: 'Events', exact: true })
    }

    async expectHomePageNavBarIsVisible() {
        await Promise.all([
            await expect(this.loginBtn).toBeVisible(),
            await expect(this.registerBtn).toBeVisible(),
            await expect(this.eventsBtn).toBeVisible()
        ]);
    }
}