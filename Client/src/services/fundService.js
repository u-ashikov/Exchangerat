import baseService from './baseService'

var fundService = baseService.getInstance({ baseURL: 'http://localhost:5000'});

function add(fundData) {
    return fundService.post('/api/funds/add', fundData);
}

function getMy() {
    return fundService.get('/api/funds/getMy');
}

export default {
    add,
    getMy
}