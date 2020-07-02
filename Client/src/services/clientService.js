import baseService from './baseService'

const clientService = baseService.getInstance({ baseURL: 'http://localhost:5000' });

function create(clientData) {
    return clientService.post('/api/clients/create', clientData);
}

function getIdByUserId() {
    return clientService.get('/api/clients/getClientId');
}

export default {
    create,
    getIdByUserId
}