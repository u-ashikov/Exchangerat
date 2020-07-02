import axios from 'axios'

const axiosInstance = axios.create({
    baseURL: 'http://localhost:5000'
    // timeout: 1000
});

axiosInstance.interceptors.request.use(function (config) {
    const token = localStorage.getItem('token');
    config.headers.Authorization =  token ? `Bearer ${token}` : '';

    return config;
});

function getMy() {
    return axiosInstance.get('/api/exchangeAccounts/getMy');
}

function getAccountDetailsById(accountId) {
    return axiosInstance.get('/api/exchangeAccounts/getAccountTransactions', { params:  { accountId } });
}

function getActiveByClientForTransaction() {
    return axiosInstance.get('/api/exchangeAccounts/GetActiveByClientForTransaction');
}

export default {
    getMy,
    getAccountDetailsById,
    getActiveByClientForTransaction
}