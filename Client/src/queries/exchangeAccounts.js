import axios from 'axios'

const axiosInstance = axios.create({
    baseURL: 'http://localhost:57765',
    // timeout: 1000
});

function getByUser(requestOptions) {
    return axiosInstance.get('/api/exchangeAccounts/getByUser', { headers: requestOptions });
}

export default {
    getByUser: getByUser
}