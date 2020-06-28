import axios from 'axios'
import { authHeader } from '../helpers/auth-header'

const axiosInstance = axios.create({
    baseURL: 'http://localhost:57765'
});

function create(data) {
    return axiosInstance.post('/api/transactions/create', data, { headers: authHeader() });
}

export default {
    create: create
}