import axios from 'axios'

const axiosInstance = axios.create({
    baseURL: 'http://localhost:5001',
    // timeout: 1000
});

function login(userData) {
    return axiosInstance.post('/api/identity/login', userData);
}

function register(userData) {
    return axiosInstance.post('/api/identity/register', userData);
}

export default {
    login,
    register
}