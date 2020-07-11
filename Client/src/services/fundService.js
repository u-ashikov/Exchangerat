import baseService from './baseService'

var fundService = baseService.getInstance({ baseURL: 'http://localhost:5000'});

function add(fundData) {
    return fundService.post('/api/funds/add', fundData);
}

export default {
    add
}