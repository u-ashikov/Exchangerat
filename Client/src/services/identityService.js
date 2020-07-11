import baseService from './baseService.js'

const identityService = baseService.getInstance({ baseURL: 'http://localhost:5001' });

function login(userData) {
    return identityService.post('/api/identity/login', userData);
}

function register(userData) {
    return identityService.post('/api/identity/register', userData);
}

export default {
    login,
    register
}