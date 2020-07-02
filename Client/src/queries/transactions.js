import axios from 'axios'

const axiosInstance = axios.create({
    baseURL: 'http://localhost:5000'
});

axiosInstance.interceptors.request.use(function (config) {
    const token = localStorage.getItem('token');
    config.headers.Authorization =  token ? `Bearer ${token}` : '';

    return config;
});

function create(data) {
    return axiosInstance.post('/api/transactions/create', data);
}

export default {
    create
}