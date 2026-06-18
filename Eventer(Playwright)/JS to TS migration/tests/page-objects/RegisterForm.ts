import { Page, Locator, expect } from '@playwright/test';

export class RegisterForm {
    readonly page: Page;
    readonly emailField: Locator;
    readonly passwordField: Locator;
    readonly repeatPasswordField: Locator;
    readonly submitFormBtn: Locator;
    readonly registerFormElement: Locator;

    constructor(page: Page) {
        this.page = page;

        this.registerFormElement = page.locator('xpath=//main/section[@id="register"]')

        this.emailField = page.getByRole('textbox', { name: 'email' })
        this.passwordField = page.getByRole('textbox', { name: 'password', exact: true })
        this.repeatPasswordField = page.getByRole('textbox', { name: 'repeat password' })

        this.submitFormBtn = page.getByRole('button', { name: 'register' })
    }

    async expectRegisterFormIsVisible() {
        await Promise.all([
            expect(this.registerFormElement).toBeVisible(),
            expect(this.emailField).toBeVisible(),
            expect(this.passwordField).toBeVisible(),
            expect(this.repeatPasswordField).toBeVisible(),
            expect(this.submitFormBtn).toBeVisible()
        ]);
    }
}