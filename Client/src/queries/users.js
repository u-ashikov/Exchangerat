import axios from 'axios'

const axiosInstance = axios.create({
    baseURL: 'http://localhost:57765',
    // timeout: 1000
});

function register(username, email, password, confirmPassword, firstName, lastName, address) {
    return axiosInstance.post('/api/identity/register', { username, email, password, confirmPassword, firstName, lastName, address });
}

export default {
    register
}