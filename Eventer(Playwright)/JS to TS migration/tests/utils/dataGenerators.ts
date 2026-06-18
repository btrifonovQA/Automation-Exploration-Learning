export function generateRandomId(): string {
    return Math.floor(Math.random() * 1000000).toString();
}

export function generateEventName(): string {
    return `testEvent_${generateRandomId()}`;
}

export function generateRandomEmail(): string {
    return `test_${generateRandomId()}@mail.com`;
}