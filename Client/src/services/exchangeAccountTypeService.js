import baseService from '../services/baseService'

const instance = baseService.getInstance({ baseURL: 'http://localhost:5000'});

function getAll() {
    return instance.get('/api/exchangeAccountTypes/getAll');
}

export default {
    getAll
}