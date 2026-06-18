import { Page, Locator, expect } from '@playwright/test';

export class DashboardPage {
    readonly page: Page;
    readonly dashboard: Locator;

    constructor(page: Page) {
        this.page = page;
        this.dashboard = page.locator('xpath=//section[@id="dashboard"]')
    }

    async createdEventIsPresentOnDashboard(eventName: string) {
        await Promise.all([
            expect(this.page.locator('xpath=//section[@id="dashboard"]/div[@class="event"]/p[@class="title"]', { hasText: eventName })).toBeVisible(),
            expect(this.page.locator('xpath=//section[@id="dashboard"]/div[@class="event"]/p[@class="title"]', { hasText: eventName })).toHaveCount(1)
        ]);
    }
}