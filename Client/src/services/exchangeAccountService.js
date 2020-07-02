import baseService from './baseService'

const exchangeAccountService = baseService.getInstance({ baseURL: 'http://localhost:5000' });

function getMy() {
    return exchangeAccountService.get('/api/exchangeAccounts/getMy');
}

function getAccountDetailsById(accountId) {
    return exchangeAccountService.get('/api/exchangeAccounts/getAccountTransactions', { params:  { accountId } });
}

function getActiveByClientForTransaction() {
    return exchangeAccountService.get('/api/exchangeAccounts/GetActiveByClientForTransaction');
}

export default {
    getMy,
    getAccountDetailsById,
    getActiveByClientForTransaction
}