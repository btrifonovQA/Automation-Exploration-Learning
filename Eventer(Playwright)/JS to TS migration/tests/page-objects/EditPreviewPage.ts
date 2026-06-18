import { Page, Locator, expect } from '@playwright/test';

export class EditPreviewPage {
    readonly page: Page;
    readonly deletePreviewButton: Locator;
    readonly editPreviewButton: Locator;

    readonly eventPreviewName: Locator;
    readonly eventPreviewElement: Locator;

    constructor(page: Page) {
        this.page = page;
        this.eventPreviewElement = page.locator('xpath=//section[@id="details"]')
        this.eventPreviewName = page.locator('xpath=//section[@id="details"]//p[@id="details-title"]')

        this.editPreviewButton = page.getByRole('link', { name: 'Edit' })
        this.deletePreviewButton = page.getByRole('link', { name: 'Delete' });
    }
}