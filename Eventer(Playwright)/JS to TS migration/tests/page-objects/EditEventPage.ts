import { Page, Locator, expect } from '@playwright/test';

export class EditEventPage {
  readonly page: Page;
  readonly editButton: Locator;
  readonly deleteButton: Locator;
  readonly eventNameField: Locator;
  readonly eventImageUrlField: Locator;
  readonly eventCategoryField: Locator;
  readonly eventDescriptionField: Locator;
  readonly eventWhenField: Locator;

  constructor(page: Page) {
    this.page = page;

    this.editButton = page.getByRole('button', { name: 'Edit' })
    this.deleteButton = page.getByRole('link', { name: 'Delete' });

    this.eventNameField = page.getByRole('textbox', { name: 'Event', exact: true });
    this.eventImageUrlField = page.getByRole('textbox', { name: 'Event Image' })
    this.eventCategoryField = page.getByRole('textbox', { name: 'Category' });
    this.eventDescriptionField = page.getByRole('textbox', { name: 'Description' });
    this.eventWhenField = page.getByRole('textbox', { name: 'When?' });
  }

  async editEventFormFill(eventName: string, randomId: string): Promise<void> {

    await this.eventNameField.fill(eventName + "_$edited");
    await this.eventImageUrlField.fill('https://wegotthiscovered.com/wp-content/uploads/2022/09/best-star-wars-meme.jpg?resize=1536,864');
    await this.eventCategoryField.fill(`Category_id${randomId}` + "_$edited");
    await this.eventDescriptionField.fill(`Description_id${randomId}` + "_$edited");
    await this.eventWhenField.fill(`Date_id${randomId}` + "_$edited");

    await this.editButton.click()
  }
}