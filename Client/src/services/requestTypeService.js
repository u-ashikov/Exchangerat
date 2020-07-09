import baseService from './baseService'

const requestTypeService = baseService.getInstance({ baseURL: 'http://localhost:5002'});

function getAll() {
    return requestTypeService.get('/api/requestTypes/getAll');
}

export default {
    getAll
}