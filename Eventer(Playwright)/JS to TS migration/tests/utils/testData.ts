import { generateRandomEmail } from './dataGenerators';

export function createUser() {
    return {
        email: generateRandomEmail(),
        password: '123456',
        confirmPass: '123456'
    };
}