import axios from 'axios'
import { authHeader } from '../helpers/auth-header'

const axiosInstance = axios.create({
    baseURL: 'http://localhost:57765',
    // timeout: 1000
});

function getByUser() {
    return axiosInstance.get('/api/exchangeAccounts/getByUser', { headers: authHeader() });
}

function getDetailsById(accountId) {
    return axiosInstance.get('/api/exchangeAccounts/getAccountTransactions', { params:  { accountId: accountId }, headers: authHeader() });
}

function getUserActiveAccountsForTransaction() {
    return axiosInstance.get('/api/exchangeAccounts/GetActiveByUserForTransaction', { headers: authHeader() });
}

export default {
    getByUser: getByUser,
    getDetailsById: getDetailsById,
    getUserActiveAccountsForTransaction: getUserActiveAccountsForTransaction
}