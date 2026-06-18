const { test, describe, beforeEach, afterEach, beforeAll, afterAll, expect } = require('@playwright/test');
const { chromium } = require('playwright');

const host = 'http://localhost:3000';

let browser;
let context;
let page;

let user = {
    email: "",
    password: "123456",
    confirmPass: "123456",
};

let eventName = "";
let createdEventHREF = "";

async function loginUser(page, email, password) {
    await page.goto(host)

    //press Login
    const loginBtnLocator = await page.getByRole('link', { name: 'Login' })
    await loginBtnLocator.click()
    await page.waitForURL(host + '/login')
    expect(page.url()).toBe(host + '/login')
    await expect(page.locator('#login > div.form')).toBeVisible()

    //fill login fields
    const emailFieldLocator = await page.getByRole('textbox', { name: 'email' })
    const passwordFieldLocator = await page.getByRole('textbox', { name: 'password' })

    await emailFieldLocator.fill(email)
    await passwordFieldLocator.fill(password)

    const loginSubmitBtnLocator = await page.getByRole('button', { name: 'Login' })
    await loginSubmitBtnLocator.click()

    await page.waitForURL(host + '/')
    expect(page.url()).toBe(host + '/')
    await expect(page.getByRole('link', { name: 'Logout' })).toBeVisible()
}

describe("e2e tests", () => {
    beforeAll(async () => {
        browser = await chromium.launch();
    });

    afterAll(async () => {
        await browser.close();
    });

    beforeEach(async () => {
        context = await browser.newContext();
        page = await context.newPage();
    });

    afterEach(async () => {
        await page.close();
        await context.close();
    });


    describe("authentication", () => {
        test('Registration with Valid Data', async () => {
            await page.goto(host)

            const registerBtnLocator = await page.getByRole('link', { name: 'Register' })
            await registerBtnLocator.click()

            await page.waitForURL(host + '/register')
            expect(page.url()).toBe(host + '/register')
            await expect(page.locator('#register > div.form')).toBeVisible()

            let random = Math.floor(Math.random() * 1000000)
            user.email = `testaccount_id${random}@softuni.bg`

            const emailFieldLocator = await page.getByRole('textbox', { name: 'email' })
            await emailFieldLocator.fill(user.email)

            const passwordFieldLocator = await page.getByRole('textbox', { name: 'password', exact: true })
            await passwordFieldLocator.fill(user.password)

            const repeatPasswordFieldLocator = await page.getByRole('textbox', { name: 'repeat password' })
            await repeatPasswordFieldLocator.fill(user.confirmPass)

            const registerSubmitBtnLocator = await page.getByRole('button', { name: 'register' })
            await registerSubmitBtnLocator.click()

            await page.waitForURL(host + '/')
            expect(page.url()).toBe(host + '/')

            await expect(page.getByRole('link', { name: 'Logout' })).toBeVisible()
        })

        test('Login with Valid Data', async () => {
            await loginUser(page, user.email, user.password)
        })

        test('Logout from the Application', async () => {
            await loginUser(page, user.email, user.password)

            await page.getByRole('link', { name: 'Logout' }).click()

            await expect(page.getByRole('link', { name: 'Login' })).toBeVisible()
        })
    });

    describe("navbar", () => {
        test('Navigation for Logged-In User', async () => {
            await loginUser(page, user.email, user.password)

            const eventsBtnLocator = await page.getByRole('link', { name: 'Events', exact: true })
            const addEventBtnLocator = await page.getByRole('link', { name: 'Add Event' })
            const logoutBtnLocator = await page.getByRole('link', { name: 'Logout' })

            await expect(eventsBtnLocator).toBeVisible()
            await expect(addEventBtnLocator).toBeVisible()
            await expect(logoutBtnLocator).toBeVisible()

            const loginBtnLocator = await page.getByRole('link', { name: 'Login' })
            const registerBtnLocator = await page.getByRole('link', { name: 'Register' })

            await expect(loginBtnLocator).toBeHidden()
            await expect(registerBtnLocator).toBeHidden()
        })

        test('Navigation for Guest User', async () => {
            await page.goto(host)

            const loginBtnLocator = await page.getByRole('link', { name: 'Login' })
            const registerBtnLocator = await page.getByRole('link', { name: 'Register' })
            const eventsBtnLocator = await page.getByRole('link', { name: 'Events', exact: true })

            await expect(loginBtnLocator).toBeVisible()
            await expect(registerBtnLocator).toBeVisible()
            await expect(eventsBtnLocator).toBeVisible()

            const addEventBtnLocator = await page.getByRole('link', { name: 'Add Event' })
            const logoutBtnLocator = await page.getByRole('link', { name: 'Logout' })
            await expect(addEventBtnLocator).toBeHidden()
            await expect(logoutBtnLocator).toBeHidden()
        })
    });

    describe("CRUD", () => {
        test('Add an Event', async () => {
            await loginUser(page, user.email, user.password)

            const addEventBtnLocator = await page.getByRole('link', { name: 'Add Event' })
            await addEventBtnLocator.click()

            await page.waitForURL(host + '/add-event')
            expect(page.url()).toBe(host + '/add-event')
            await expect(page.locator('#create > div.form')).toBeVisible()

            let random = Math.floor(Math.random() * 1000000)
            eventName = `testEvent_id${random}`

            //event fields locate + fill
            const eventNameInputFieldLocator = await page.getByRole('textbox', { name: 'Event', exact: true })
            await eventNameInputFieldLocator.fill(eventName)

            const eventImageUrlInputFieldLocator = await page.getByRole('textbox', { name: 'Event Image URL' })
            await eventImageUrlInputFieldLocator.fill('https://www.boredpanda.com/blog/wp-content/uploads/2022/09/relatable-funny-memes-22-63284d45ebe28__700.jpg')

            const eventCategoryInputFieldLocator = await page.getByRole('textbox', { name: 'Category' })
            await eventCategoryInputFieldLocator.fill(`Category_id${random}`)

            const eventDescriptionInputFieldLocator = await page.getByRole('textbox', { name: 'Description' })
            await eventDescriptionInputFieldLocator.fill(`Description_id${random}`)

            const eventWhenInputFieldLocator = await page.getByRole('textbox', { name: 'When?' })
            await eventWhenInputFieldLocator.fill(`Date_id${random}`)

            const addEventSubmitBtnLocator = await page.getByRole('button', { name: 'Add' })
            await addEventSubmitBtnLocator.click()

            await page.waitForURL(host + '/dashboard')
            expect(page.url()).toBe(host + '/dashboard')
            await expect(page.locator('#dashboard')).toBeVisible()

            await expect(page.locator('#dashboard div.event > p.title', { hasText: eventName })).toBeVisible()
            await expect(page.locator('#dashboard div.event > p.title', { hasText: eventName })).toHaveCount(1)
        })

        test('Edit an Event', async () => {
            await loginUser(page, user.email, user.password)

            const eventsBtnLocator = await page.getByRole('link', { name: 'Events', exact: true })
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
            const createdEventBtnLocator = await page.locator(`div.event > a[href="${createdEventHREF.replace('http://localhost:3000', '')}"]`)
            await createdEventBtnLocator.click()

            await page.waitForURL(createdEventHREF)
            expect(page.url()).toBe(createdEventHREF)
            await expect(page.locator('#details-wrapper')).toBeVisible()

            const editBtnLocator = await page.getByRole('link', { name: 'Edit' })
            await editBtnLocator.click()
            
            const editBtnHREF = await editBtnLocator.evaluate(el => el.getAttribute('href'))
            await page.waitForURL(`${host + editBtnHREF}`)
            expect(page.url()).toBe(`${host + editBtnHREF}`)
            await expect(page.locator('#edit > div.form')).toBeVisible()

            //find name and edit it to new value
            const eventNameInputFieldLocator = await page.getByRole('textbox', { name: 'Event', exact: true })
            eventName += '_$edited'
            await eventNameInputFieldLocator.fill(eventName)

            const addEventSubmitBtnLocator = await page.getByRole('button', { name: 'Edit' })
            await addEventSubmitBtnLocator.click()

            await page.waitForURL(createdEventHREF)
            expect(page.url()).toBe(createdEventHREF)
            await expect(page.locator('#details-wrapper')).toBeVisible()

            await expect(page.locator('#details-wrapper > #details-title', { hasText: eventName })).toBeVisible()
            await expect(page.locator('#details-wrapper > #details-title', { hasText: eventName })).toContainText(eventName)
        })

        test('Delete an Event', async () => {
            await loginUser(page, user.email, user.password)

            const eventsBtnLocator = await page.getByRole('link', { name: 'Events', exact: true })
            await eventsBtnLocator.click()

            await page.waitForURL(host + '/dashboard')
            expect(page.url()).toBe(host + '/dashboard')
            await expect(page.locator('#dashboard')).toBeVisible()

                                                                            //replace turns [href="http://localhost:3000/details/unique-ID"] into [href="/details/unique-ID"]
            const createdEventBtnLocator = await page.locator(`div.event > a[href="${createdEventHREF.replace('http://localhost:3000', '')}"]`)
            await createdEventBtnLocator.click()

            await page.waitForURL(createdEventHREF)
            expect(page.url()).toBe(createdEventHREF)
            await expect(page.locator('#details-wrapper')).toBeVisible()

            page.on('dialog', dialog => dialog.accept());
            const deleteBtnLocator = await page.getByRole('link', { name: 'Delete' })
            await deleteBtnLocator.click()

            await page.waitForURL(host + '/dashboard')
            expect(page.url()).toBe(host + '/dashboard')
            await expect(page.locator('#dashboard')).toBeVisible()

            await expect(page.locator('div.event > p.title', { hasText: eventName })).toHaveCount(0)
        })
    });
});