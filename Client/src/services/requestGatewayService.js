import baseService from './baseService'

var requestGatewayService = baseService.getInstance({ baseURL: 'http://localhost:5004' });

function getMy() {
    return requestGatewayService.get('/api/requests/getmy');
}

export default {
    getMy
}