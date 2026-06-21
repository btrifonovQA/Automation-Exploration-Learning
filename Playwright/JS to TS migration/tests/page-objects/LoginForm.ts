import { Page, Locator, expect } from '@playwright/test';

export class LoginForm {
  readonly page: Page;
  readonly emailField: Locator;
  readonly passwordField: Locator;
  readonly loginSubmitBtn: Locator;
  readonly loginFormElement: Locator;

  constructor(page: Page) {
    this.page = page;

    this.loginFormElement = page.locator('xpath=//main/section[@id="login"]')

    this.emailField = page.getByRole('textbox', { name: 'email' })
    this.passwordField = page.getByRole('textbox', { name: 'password' })

    this.loginSubmitBtn = page.getByRole('button', { name: 'Login' })
  }

  async expectLoginFormIsVisible() {
    await Promise.all([
      expect(this.loginFormElement).toBeVisible(),
      expect(this.emailField).toBeVisible(),
      expect(this.passwordField).toBeVisible(),
      expect(this.loginSubmitBtn).toBeVisible()
    ]);
  }
}