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

function getDetailsById(accountId) {
    return axiosInstance.get('/api/exchangeAccounts/getAccountTransactions', { params:  { accountId: accountId } });
}

function getUserActiveAccountsForTransaction() {
    return axiosInstance.get('/api/exchangeAccounts/GetActiveByUserForTransaction');
}

export default {
    getMy: getMy,
    getDetailsById: getDetailsById,
    getUserActiveAccountsForTransaction: getUserActiveAccountsForTransaction
}