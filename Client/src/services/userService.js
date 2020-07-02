import baseService from './baseService.js'

const userService = baseService.getInstance({ baseURL: 'http://localhost:5001' });

function login(userData) {
    return userService.post('/api/identity/login', userData);
}

function register(userData) {
    return userService.post('/api/identity/register', userData);
}

export default {
    login,
    register
}