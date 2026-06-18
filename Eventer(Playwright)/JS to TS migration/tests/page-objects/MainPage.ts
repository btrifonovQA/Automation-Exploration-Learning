import { Page, Locator, expect } from '@playwright/test';

export class MainPage {
  readonly page: Page;
  readonly eventsBtn: Locator;
  readonly addEventBtn: Locator;
  readonly logoutBtn: Locator;

  constructor(page: Page) {
    this.page = page;
    this.eventsBtn = page.getByRole('link', { name: 'Events', exact: true });
    this.addEventBtn = page.getByRole('link', { name: 'Add Event' });
    this.logoutBtn = page.getByRole('link', { name: 'Logout' });
  }

  async expectMainPageNavBarIsVisible() {
    await Promise.all([
      expect(this.eventsBtn).toBeVisible(),
      expect(this.addEventBtn).toBeVisible(),
      expect(this.logoutBtn).toBeVisible(),
    ]);
  }
}