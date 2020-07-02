import axios from 'axios'

function getInstance(instanceConfig) {
    if (!instanceConfig || !instanceConfig.baseURL) {
        throw new Error('Please provide a valid config with at last baseURL parameter.');
    }

    const instance = axios.create({
        baseURL: instanceConfig.baseURL
    });

    instance.interceptors.request.use(function (config) {
        const token = localStorage.getItem('token');

        if (token) {
            config.headers.Authorization =  `Bearer ${token}`;
        } else {
            delete config.headers.common['Authorization'];
        }
    
        return config;
    });

    return instance;
}

export default {
    getInstance
}

