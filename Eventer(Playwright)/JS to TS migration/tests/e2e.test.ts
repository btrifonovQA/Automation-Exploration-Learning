import { test, expect } from '@playwright/test';
import type { Locator, Page } from '@playwright/test';
import { MainPage } from './page-objects/MainPage';
import { HomePage } from './page-objects/HomePage';
import { LoginForm } from './page-objects/LoginForm';
import { RegisterForm } from './page-objects/RegisterForm';

const host: string = 'http://localhost:3000'

let user = {
    email: "",
    password: "123456",
    confirmPass: "123456",
};

let randomNum: number = Math.floor(Math.random() * 1000000)
user.email = `testaccount_id${randomNum}@domain.com`

let eventName: string = "";
let createdEventHREF: string = "";

async function loginUser(page: Page, email: string, password: string) {
    const homePage: HomePage = new HomePage(page)
    const loginForm: LoginForm = new LoginForm(page)

    await page.goto(host)

    //press Login
    await homePage.expectHomePageNavBarIsVisible()
    await homePage.loginBtn.click()

    //fill login fields
    await expect(page).toHaveURL(host + '/login')
    loginForm.expectLoginFormIsVisible()

    await loginForm.emailField.fill(email)
    await loginForm.passwordField.fill(password)
    await loginForm.loginSubmitBtn.click()
}

async function registerUser(page: Page, email: string, password: string, confirmPass: string) {
    const homePage: HomePage = new HomePage(page)
    const registerForm: RegisterForm = new RegisterForm(page)
    const mainPage: MainPage = new MainPage(page)

    await page.goto(host)

    homePage.expectHomePageNavBarIsVisible();
    await homePage.registerBtn.click()

    await expect(page).toHaveURL(host + '/register')
    await registerForm.expectRegisterFormIsVisible()

    await registerForm.emailField.fill(user.email)
    await registerForm.passwordField.fill(user.password)
    await registerForm.repeatPasswordField.fill(user.confirmPass)

    await registerForm.submitFormBtn.click()
}

// createEvent()
// editEvent()
// openEvent()  

test.describe("e2e tests", () => {

    test.describe("authentication", () => {
        test('Registration with Valid Data', async ({ page }) => {
            const mainPage: MainPage = new MainPage(page)

            registerUser(page, user.email, user.password, user.confirmPass)

            await expect(page).toHaveURL(host + '/')
            await mainPage.expectMainPageNavBarIsVisible()
        })

        test('Login with Valid Data', async ({ page }) => {
            const mainPage: MainPage = new MainPage(page)

            await loginUser(page, user.email, user.password)

            await expect(page).toHaveURL(host + '/')
            await expect(mainPage.logoutBtn).toBeVisible()
        })

        test('Logout from the Application', async ({ page }) => {
            const mainPage: MainPage = new MainPage(page)
            const homePage: HomePage = new HomePage(page)

            await loginUser(page, user.email, user.password)

            await mainPage.logoutBtn.click()

            await expect(homePage.loginBtn).toBeVisible()
        })
    });

    test.describe("navbar", () => {
        test('Navigation for Logged-In User', async ({ page }) => {
            const mainPage: MainPage = new MainPage(page)
            const homePage: HomePage = new HomePage(page)

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
        test('Add an Event', async ({page}) => {
            await loginUser(page, user.email, user.password)

            const addEventBtnLocator: Locator = await page.getByRole('link', { name: 'Add Event' })
            await addEventBtnLocator.click()

            await page.waitForURL(host + '/add-event')
            expect(page.url()).toBe(host + '/add-event')
            await expect(page.locator('#create > div.form')).toBeVisible()

            let random = Math.floor(Math.random() * 1000000)
            eventName = `testEvent_id${random}`

            //event fields locate + fill
            const eventNameInputFieldLocator: Locator = await page.getByRole('textbox', { name: 'Event', exact: true })
            await eventNameInputFieldLocator.fill(eventName)

            const eventImageUrlInputFieldLocator: Locator = await page.getByRole('textbox', { name: 'Event Image URL' })
            await eventImageUrlInputFieldLocator.fill('https://www.boredpanda.com/blog/wp-content/uploads/2022/09/relatable-funny-memes-22-63284d45ebe28__700.jpg')

            const eventCategoryInputFieldLocator: Locator = await page.getByRole('textbox', { name: 'Category' })
            await eventCategoryInputFieldLocator.fill(`Category_id${random}`)

            const eventDescriptionInputFieldLocator: Locator = await page.getByRole('textbox', { name: 'Description' })
            await eventDescriptionInputFieldLocator.fill(`Description_id${random}`)

            const eventWhenInputFieldLocator: Locator = await page.getByRole('textbox', { name: 'When?' })
            await eventWhenInputFieldLocator.fill(`Date_id${random}`)

            const addEventSubmitBtnLocator: Locator = await page.getByRole('button', { name: 'Add' })
            await addEventSubmitBtnLocator.click()

            await page.waitForURL(host + '/dashboard')
            expect(page.url()).toBe(host + '/dashboard')
            await expect(page.locator('#dashboard')).toBeVisible()

            await expect(page.locator('#dashboard div.event > p.title', { hasText: eventName })).toBeVisible()
            await expect(page.locator('#dashboard div.event > p.title', { hasText: eventName })).toHaveCount(1)
        })

        test('Edit an Event', async () => {
            await loginUser(page, user.email, user.password)

            const eventsBtnLocator: Locator = await page.getByRole('link', { name: 'Events', exact: true })
            await eventsBtnLocator.click()

            await page.waitForURL(host + '/dashboard')
            expect(page.url()).toBe(host + '/dashboard')
            await expect(page.locator('#dashboard')).toBeVisible()

            //find href of edit button for created event
            createdEventHREF = await page.evaluate(eventName => {
                const allEvents = Array.from(document.querySelectorAll('div.event > p.title'))
                const eventElement = allEvents.find(title => title.textContent.trim() === eventName)
                const parentEventElement = eventElement.parentElement
                const detailsBtn = parentEventElement.querySelector('a.details-btn')

                return detailsBtn.href
            }, eventName);

            //replace turns [href="http://localhost:3000/details/unique-ID"] into [href="/details/unique-ID"]
            const createdEventBtnLocator: Locator = await page.locator(`div.event > a[href="${createdEventHREF.replace('http://localhost:3000', '')}"]`)
            await createdEventBtnLocator.click()

            await page.waitForURL(createdEventHREF)
            expect(page.url()).toBe(createdEventHREF)
            await expect(page.locator('#details-wrapper')).toBeVisible()

            const editBtnLocator: Locator = await page.getByRole('link', { name: 'Edit' })
            await editBtnLocator.click()

            const editBtnHREF = await editBtnLocator.evaluate(el => el.getAttribute('href'))
            await page.waitForURL(`${host + editBtnHREF}`)
            expect(page.url()).toBe(`${host + editBtnHREF}`)
            await expect(page.locator('#edit > div.form')).toBeVisible()

            //find name and edit it to new value
            const eventNameInputFieldLocator: Locator = await page.getByRole('textbox', { name: 'Event', exact: true })
            eventName += '_$edited'
            await eventNameInputFieldLocator.fill(eventName)

            const addEventSubmitBtnLocator: Locator = await page.getByRole('button', { name: 'Edit' })
            await addEventSubmitBtnLocator.click()

            await page.waitForURL(createdEventHREF)
            expect(page.url()).toBe(createdEventHREF)
            await expect(page.locator('#details-wrapper')).toBeVisible()

            await expect(page.locator('#details-wrapper > #details-title', { hasText: eventName })).toBeVisible()
            await expect(page.locator('#details-wrapper > #details-title', { hasText: eventName })).toContainText(eventName)
        })

        test('Delete an Event', async () => {
            await loginUser(page, user.email, user.password)

            const eventsBtnLocator: Locator = await page.getByRole('link', { name: 'Events', exact: true })
            await eventsBtnLocator.click()

            await page.waitForURL(host + '/dashboard')
            expect(page.url()).toBe(host + '/dashboard')
            await expect(page.locator('#dashboard')).toBeVisible()

            //replace turns [href="http://localhost:3000/details/unique-ID"] into [href="/details/unique-ID"]
            const createdEventBtnLocator: Locator = await page.locator(`div.event > a[href="${createdEventHREF.replace('http://localhost:3000', '')}"]`)
            await createdEventBtnLocator.click()

            await page.waitForURL(createdEventHREF)
            expect(page.url()).toBe(createdEventHREF)
            await expect(page.locator('#details-wrapper')).toBeVisible()

            page.on('dialog', dialog => dialog.accept());
            const deleteBtnLocator: Locator = await page.getByRole('link', { name: 'Delete' })
            await deleteBtnLocator.click()

            await page.waitForURL(host + '/dashboard')
            expect(page.url()).toBe(host + '/dashboard')
            await expect(page.locator('#dashboard')).toBeVisible()

            await expect(page.locator('div.event > p.title', { hasText: eventName })).toHaveCount(0)
        })
    });
});