import { Page, Locator } from '@playwright/test';

export class AddEventPage {
    readonly page: Page;

    readonly addEventForm: Locator;
    readonly eventNameInput: Locator;
    readonly eventImageUrlInput: Locator;
    readonly eventCategoryInput: Locator;
    readonly eventDescriptionInput: Locator;
    readonly eventWhenInput: Locator;
    readonly submitBtn: Locator;

    constructor(page: Page) {
        this.page = page;

        this.addEventForm = page.locator('xpath=//section[@id="create"]');
        this.eventNameInput = page.getByRole('textbox', { name: 'Event', exact: true });
        this.eventImageUrlInput = page.getByRole('textbox', { name: 'Event Image URL' });
        this.eventCategoryInput = page.getByRole('textbox', { name: 'Category' });
        this.eventDescriptionInput = page.getByRole('textbox', { name: 'Description' });
        this.eventWhenInput = page.getByRole('textbox', { name: 'When?' });
        this.submitBtn = page.getByRole('button', { name: 'Add' });
    }


    async fillEventForm(eventName: string, randomId: string): Promise<void> {
        await this.eventNameInput.fill(eventName);
        await this.eventImageUrlInput.fill('https://www.boredpanda.com/blog/wp-content/uploads/2022/09/relatable-funny-memes-22-63284d45ebe28__700.jpg');
        await this.eventCategoryInput.fill(`Category_id${randomId}`);
        await this.eventDescriptionInput.fill(`Description_id${randomId}`);
        await this.eventWhenInput.fill(`Date_id${randomId}`);
    }

    async submitForm(): Promise<void> {
        await this.submitBtn.click();
    }
}