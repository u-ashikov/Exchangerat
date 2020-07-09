import baseService from './baseService'

const requestService = baseService.getInstance({ baseURL: 'http://localhost:5002'});

function create(data) {
    return requestService.post('/api/exchangeratRequests/create', data);
}

export default {
    create
}