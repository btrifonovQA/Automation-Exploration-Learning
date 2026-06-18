import { test, expect } from '@playwright/test';
import type { Page } from '@playwright/test';
import { MainPage } from './page-objects/MainPage';
import { HomePage } from './page-objects/HomePage';
import { LoginForm } from './page-objects/LoginForm';
import { RegisterForm } from './page-objects/RegisterForm';
import { AddEventPage } from './page-objects/AddEventPage';
import { DashboardPage } from './page-objects/DashboardPage';
import { EditEventPage } from './page-objects/EditEventPage';
import { EditPreviewPage } from './page-objects/EditPreviewPage';
import { generateRandomId, generateEventName } from './utils/dataGenerators';
import { createUser } from './utils/testData';

const host: string = 'http://localhost:3000'

async function loginUser(page: Page, email: string, password: string) {
    const homePage: HomePage = new HomePage(page)
    const loginForm: LoginForm = new LoginForm(page)

    await page.goto(host)

    //press Login
    await homePage.expectHomePageNavBarIsVisible()
    await homePage.loginBtn.click()

    //fill login fields
    await expect(page).toHaveURL(host + '/login')
    await loginForm.expectLoginFormIsVisible()

    await loginForm.emailField.fill(email)
    await loginForm.passwordField.fill(password)
    await loginForm.loginSubmitBtn.click()
}

async function registerUser(page: Page, user: { email: string; password: string; confirmPass: string; }) {
    const homePage: HomePage = new HomePage(page)
    const registerForm: RegisterForm = new RegisterForm(page)

    await page.goto(host)

    await homePage.expectHomePageNavBarIsVisible();
    await homePage.registerBtn.click()

    await expect(page).toHaveURL(host + '/register')
    await registerForm.expectRegisterFormIsVisible()

    await registerForm.emailField.fill(user.email)
    await registerForm.passwordField.fill(user.password)
    await registerForm.repeatPasswordField.fill(user.confirmPass)

    await registerForm.submitFormBtn.click()

    return user
}

async function logoutUser(page: Page) {
    const mainPage: MainPage = new MainPage(page)

    await mainPage.logoutBtn.click()
}

async function createEvent(page: Page, eventName: string, randomId: string) {
    const mainPage: MainPage = new MainPage(page)
    const addEventPage: AddEventPage = new AddEventPage(page)

    await mainPage.addEventBtn.click()

    await expect(page).toHaveURL(host + '/add-event')
    await expect(addEventPage.addEventForm).toBeVisible()

    await addEventPage.fillEventForm(eventName, randomId)
    await addEventPage.submitForm()
}

async function editEvent(page: Page, eventName: string, randomId: string) {
    const editEventPage: EditEventPage = new EditEventPage(page)
    const editPreviewPage: EditPreviewPage = new EditPreviewPage(page)

    const eventCard = page.locator(`xpath=//div[@class="event"]/p[@class="title"][text()="${eventName}"]/..`)
    const eventCardDetailsBtn = eventCard.getByRole('link', { name: 'Details' })
    const href = await eventCardDetailsBtn.getAttribute('href')
    await eventCardDetailsBtn.click()

    await expect(page).toHaveURL(host + href)
    await expect(editPreviewPage.eventPreviewElement).toBeVisible()

    await editPreviewPage.editPreviewButton.click()

    await editEventPage.editEventFormFill(eventName, randomId)

    await expect(page).toHaveURL(host + href)
}
async function deleteEvent(page: Page, eventName: string) {
    const editPreviewPage: EditPreviewPage = new EditPreviewPage(page)
    const dashboardPage: DashboardPage = new DashboardPage(page)

    await page.goto(host + '/dashboard')

    const eventCard = page.locator(`xpath=//div[@class="event"]/p[@class="title"][text()="${eventName}"]/..`)
    const eventCardDetailsBtn = eventCard.getByRole('link', { name: 'Details' })
    const href = await eventCardDetailsBtn.getAttribute('href')
    await eventCardDetailsBtn.click()

    await expect(page).toHaveURL(host + href)
    await expect(editPreviewPage.eventPreviewElement).toBeVisible()

    page.once('dialog', dialog => dialog.accept());
    await editPreviewPage.deletePreviewButton.click()

    await expect(page).toHaveURL(host + '/dashboard')
    await expect(dashboardPage.dashboard).toBeVisible()

    await expect(eventCard).toHaveCount(0)
}

test.describe("e2e tests", () => {

    test.describe("authentication", () => {
        test('Registration with Valid Data', async ({ page }) => {
            const homePage: HomePage = new HomePage(page)

            await registerUser(page, createUser())
            await logoutUser(page)

            await expect(page).toHaveURL(host + '/')
            await homePage.expectHomePageNavBarIsVisible();
        })

        test('Login with Valid Data', async ({ page }) => {
            const mainPage: MainPage = new MainPage(page)

            let user = await registerUser(page, createUser())
            await logoutUser(page)

            await loginUser(page, user.email, user.password)

            await expect(page).toHaveURL(host + '/')
            await expect(mainPage.logoutBtn).toBeVisible()
        })

        test('Logout from the Application', async ({ page }) => {
            const mainPage: MainPage = new MainPage(page)
            const homePage: HomePage = new HomePage(page)

            let user = await registerUser(page, createUser())
            await logoutUser(page)

            await loginUser(page, user.email, user.password)

            await mainPage.logoutBtn.click()

            await expect(homePage.loginBtn).toBeVisible()
        })
    });

    test.describe("navbar", () => {
        test('Navigation for Logged-In User', async ({ page }) => {
            const mainPage: MainPage = new MainPage(page)
            const homePage: HomePage = new HomePage(page)

            let user = await registerUser(page, createUser())
            await logoutUser(page)

            await loginUser(page, user.email, user.password)

            await mainPage.expectMainPageNavBarIsVisible()

            await expect(homePage.loginBtn).toBeHidden()
            await expect(homePage.registerBtn).toBeHidden()
        })

        test('Navigation for Guest User', async ({ page }) => {
            const mainPage: MainPage = new MainPage(page)
            const homePage: HomePage = new HomePage(page)

            await page.goto(host)

            await homePage.expectHomePageNavBarIsVisible()

            await expect(mainPage.addEventBtn).toBeHidden()
            await expect(mainPage.logoutBtn).toBeHidden()
        })
    });

    test.describe("CRUD", () => {
        test('Add an Event', async ({ page }) => {
            const dashboardPage: DashboardPage = new DashboardPage(page)

            let eventName: string = generateEventName()
            let randomId: string = generateRandomId()
            let user = await registerUser(page, createUser())
            await logoutUser(page)

            await loginUser(page, user.email, user.password)
            await createEvent(page, eventName, randomId)

            await expect(page).toHaveURL(host + '/dashboard')
            await expect(dashboardPage.dashboard).toBeVisible()

            await dashboardPage.createdEventIsPresentOnDashboard(eventName)

            await deleteEvent(page, eventName)
        })

        test('Edit an Event', async ({ page }) => {
            const editPreviewPage: EditPreviewPage = new EditPreviewPage(page)

            let eventName: string = generateEventName()
            let randomId: string = generateRandomId()

            let user = await registerUser(page, createUser())
            await logoutUser(page)

            await loginUser(page, user.email, user.password)

            await createEvent(page, eventName, randomId)

            await editEvent(page, eventName, randomId)

            await expect(editPreviewPage.eventPreviewElement).toBeVisible()
            await expect(editPreviewPage.eventPreviewName).toContainText(eventName)

            await deleteEvent(page, `${eventName+"_$edited"}`)
        })

        test('Delete an Event', async ({ page }) => {
            let eventName: string = generateEventName()
            let randomId: string = generateRandomId()

            let user = await registerUser(page, createUser())
            await logoutUser(page)

            await loginUser(page, user.email, user.password)

            await createEvent(page, eventName, randomId)

            await deleteEvent(page, eventName)
        })
    });
});