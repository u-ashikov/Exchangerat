import baseService from './baseService'

const transactionService = baseService.getInstance({ baseURL: 'http://localhost:5000' });

function create(data) {
    return transactionService.post('/api/transactions/create', data);
}

export default {
    create
}