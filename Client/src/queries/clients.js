import axios from 'axios'

const axiosInstance = axios.create({
    baseURL: 'http://localhost:5000'
});

axiosInstance.interceptors.request.use(function (config) {
    const token = localStorage.getItem('token');
    config.headers.Authorization =  token ? `Bearer ${token}` : '';

    return config;
  });

function create(clientData) {
    return axiosInstance.post('/api/clients/create', clientData);
}

function getIdByUserId() {
    return axiosInstance.get('/api/clients/getClientId');
}

export default {
    create,
    getIdByUserId
}